using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.Constans;
using ABPvNextOrangeAdmin.CustomException;
using ABPvNextOrangeAdmin.CustomExtensions;
using ABPvNextOrangeAdmin.Exception;
using ABPvNextOrangeAdmin.Options;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Config;
using ABPvNextOrangeAdmin.System.Menu;
using ABPvNextOrangeAdmin.System.User;
using ABPvNextOrangeAdmin.Utils;
using ABPvNextOrangeAdmin.Utils.ImageProducer;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Security.Claims;
using Volo.Abp.Validation;


namespace ABPvNextOrangeAdmin.System.Account;

[Authorize]
[Route("api/sys/account/[action]")]
public class AccountAppService : ApplicationService, IAccountAppService
{
    private JwtOptions JwtOptions { get; set; }

    private SignInManager SignInManager { get; set; }

    // private UserRoleFinder UserRoleFinder { get; set; }

    private UserManager UserManager { get; set; }

    // private IAccountEmailer AccountEmailer { get; set; }

    private IdentitySecurityLogManager IdentitySecurityLogManager { get; set; }

    private IOptions<IdentityOptions> IdentityOptions { get; set; }

    private DefaultCaptcha DefaultCaptcha { get; set; }

    private ConfigDomainService ConfigDomainService { get; set; }

    private IDistributedCache<String> DistributedCache { get; set; }

    // private ITokenService TokenService { get; set; }

    // private IRefreshTokenService RefreshTokenService { get; set; }


    // private PermissionManager PermissionManager { get; }

    private MenuDomainService MenuDomainService { get; }


    public AccountAppService(UserManager userManager,
        /*IAccountEmailer accountEmailer,*//*IdentitySecurityLogManager identitySecurityLogManager,*/
        IOptions<IdentityOptions> identityOptions, DefaultCaptcha defaultCaptcha,
        ConfigDomainService configDomainService, IDistributedCache<String> distributedCache, /*ITokenService tokenService,*/
        /*IRefreshTokenService refreshTokenService,*/ IOptions<JwtOptions> jwtOptions,
        /*PermissionManager permissionManager,*/ /* UserRoleFinder userRoleFinder,*/
        SignInManager signInManager, MenuDomainService menuDomainService)
    {
        UserManager = userManager;
        // AccountEmailer = accountEmailer;
        // IdentitySecurityLogManager = identitySecurityLogManager;
        IdentityOptions = identityOptions;
        DefaultCaptcha = defaultCaptcha;
        ConfigDomainService = configDomainService;
        DistributedCache = distributedCache;
        // TokenService = tokenService;
        // RefreshTokenService = refreshTokenService;
        // PermissionManager = permissionManager;
        // UserRoleFinder = userRoleFinder;
        SignInManager = signInManager;
        MenuDomainService = menuDomainService;
        JwtOptions = jwtOptions.Value;
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ActionName("register")]
    public async Task<CommonResult<IdentityUserDto>> RegisterAsync(RegisterInput input)
    {
        await IdentityOptions.SetAsync();

        //创建新用户
        var user = new SysUser( input.UserName, input.EmailAddress, CurrentTenant.Id);
        input.MapExtraPropertiesTo(user);

        (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

        await UserManager.SetEmailAsync(user, input.EmailAddress);
        await UserManager.AddDefaultRolesAsync(user);

        var identityUserDto = ObjectMapper.Map<SysUser, IdentityUserDto>(user);
        return CommonResult<IdentityUserDto>.Success(identityUserDto, "注册账户完成");
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("login")]
    [AllowAnonymous]
    public async Task<CommonResult<String>> LoginAsync(LoginInput input)
    {
        Boolean captchaOnOff = await ConfigDomainService.SelectCaptchaOnOff();

        // 验证码开关
        if (captchaOnOff)
        {
            await ValidateCaptcha(input.UserNameOrEmailAddress, input.Code, input.Uuid);
        }

        ValidateLoginInfo(input);

        await ReplaceEmailToUsernameOfInputIfNeeds(input);
        var signInResult = await SignInManager.PasswordSignInAsync(
            input.UserNameOrEmailAddress,
            input.Password,
            input.RememberMe,
            true
        );

        if (!signInResult.Succeeded)
        {
            return CommonResult<String>.Failed("账号或密码错误");
        }

        var token = GenerateAccessToken(await UserManager.FindByNameAsync(input.UserNameOrEmailAddress));

        //登录日志
        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = SignInResultExtensions.ToIdentitySecurityLogAction(signInResult),
            UserName = input.UserNameOrEmailAddress
        });

        return CommonResult<String>.Success(signInResult.Succeeded ? token : "", "");
    }

    /// <summary>
    /// 用户注销
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ActionName("logout")]
    public async Task<CommonResult<string>> LogoutAsync()
    {
        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.Logout
        });


        await SignInManager.SignOutAsync();

        return CommonResult<String>.Success(null, "账户登出完成");
    }


    // /// <summary>
    // /// 修改密码 发送邮件
    // /// </summary>
    // /// <param name="input"></param>
    // [HttpPost]
    // [ActionName("passwordResetCode")]
    // public async Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input)
    // {
    //     var user = await GetUserByEmailAsync(input.Email);
    //     var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
    //     await AccountEmailer.SendPasswordResetLinkAsync(user, resetToken, input.AppName, input.ReturnUrl,
    //         input.ReturnUrlHash);
    // }


    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="input"></param>
    // [HttpPost]
    // [ActionName("resetPassword")]
    // public async Task ResetPasswordAsync(ResetPasswordDto input)
    // {
    //     await IdentityOptions.SetAsync();
    //
    //     var user = await UserManager.GetByIdAsync(input.UserId);
    //     (await UserManager.ResetPasswordAsync(user, input.ResetToken, input.Password)).CheckErrors();
    //
    //     await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
    //     {
    //         Identity = IdentitySecurityLogIdentityConsts.Identity,
    //         Action = IdentitySecurityLogActionConsts.ChangePassword
    //     });
    // }

    /// <summary>
    /// 生成验证码 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    [ActionName("captchaImage")]
    [AllowAnonymous]
    public async Task<CommonResult<CaptchaCodeOutput>> GetCaptchaImageAsync()
    {
        var guid = GuidGenerator.Create();
        String capText = DefaultCaptcha.createText();

        byte[] image = DefaultCaptcha.createImage(capText);
        var uuid = GuidGenerator.Create();
        String verifyKey = CommonConstants.CAPTCHA_CODE_KEY + uuid;
        await DistributedCache.SetAsync(verifyKey, capText,
            new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(120) });
        return CommonResult<CaptchaCodeOutput>.Success(
            CaptchaCodeOutput.CreateInstance(uuid.ToString(), image), "生成验证码成功");
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ActionName("getUserInfo")]
    public async Task<CommonResult<UserWithRoleAndPermissionOutput>> GetUserInfoAsync()
    {
        Debug.Assert(CurrentUser.Id != null, (string)"CurrentUser.Id != null");
        var identityUser = await UserManager.GetByIdAsync(CurrentUser.Id.Value);


        var user = ObjectMapper.Map<SysUser, IdentityUserDto>(identityUser);
        //获取角色名称
        String[] roleNamnes; //=await UserRoleFinder.GetRolesAsync((Guid)CurrentUser.Id); //IdentityUserRepository.GetRoleNamesAsync((Guid)CurrentUser.Id);


        HashSet<String> permissionNames = new HashSet<string>();
        // 获取角色权限
        // foreach (var roleName in roleNamnes)
        // {
        //     var withGrantedRolePermissions = await PermissionManager.GetAllAsync(PermissionConstans.Role, roleName);
        //     foreach (var rolePermission in withGrantedRolePermissions)
        //     {
        //         if (rolePermission.IsGranted)
        //         {
        //             permissionNames.Add(rolePermission.Name);
        //         }
        //     }
        // }

        // 获取用户权限
        // var withGrantedUserPermissions =
        //     await PermissionManager.GetAllAsync(PermissionConstans.User, CurrentUser.Id.ToString());
        // foreach (var userPermission in withGrantedUserPermissions)
        // {
        //     if (userPermission.IsGranted)
        //     {
        //         permissionNames.Add(userPermission.Name);
        //     }
        // }

        return CommonResult<UserWithRoleAndPermissionOutput>.Success(
            UserWithRoleAndPermissionOutput.CreateInstance(user, new string[] { }/*roleNamnes*/, permissionNames.ToArray()), "获取用户信息");
    }

    [HttpGet]
    [ActionName("getRouters")]
    public async Task<CommonResult<List<RouteOutput>>> GetRouters()
    {
        List<SysMenu> menus =await MenuDomainService.GetMenuTreeByUserId(0,CurrentUser.IsAdmin());
        List<RouteOutput>  routes  =BuildMenuRoutes(menus);
        return CommonResult<List<RouteOutput>>.Success(routes,"获取路由信息成功" );
    }

    /// <summary>
    /// 校验验证码
    /// </summary>
    /// <param name="username"></param>
    /// <param name="code"></param>
    /// <param name="uuid"></param>
    /// <exception cref="CaptchaExpireException"></exception>
    private async Task ValidateCaptcha(String username, String code, String uuid)
    {
        String verifyKey = CommonConstants.CAPTCHA_CODE_KEY + StringUtils.Nvl(uuid, "");
        String captcha = (await DistributedCache.GetAsync(verifyKey));
        await DistributedCache.RemoveAsync(verifyKey);
        if (captcha == null)
        {
            throw new CaptchaExpireException();
        }

        if (!code.Equals(captcha))
        {
            throw new CaptchaException();
        }
    }
    
    /// <summary>
    /// 检验用户信息
    /// </summary>
    /// <param name="login"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    protected virtual void ValidateLoginInfo(LoginInput login)
    {
        if (login == null)
        {
            throw new ArgumentException(nameof(login));
        }

        if (login.UserNameOrEmailAddress.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(login.UserNameOrEmailAddress));
        }

        if (login.Password.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(login.Password));
        }
    }

    /// <summary>
    /// 如果没有传用户名，则使用用户邮箱查找用户
    /// </summary>
    /// <param name="login"></param>
    protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(LoginInput login)
    {
        if (!ValidationHelper.IsValidEmailAddress(login.UserNameOrEmailAddress))
        {
            return;
        }

        var userByUsername = await UserManager.FindByNameAsync(login.UserNameOrEmailAddress);
        if (userByUsername != null)
        {
            return;
        }

        var userByEmail = await UserManager.FindByEmailAsync(login.UserNameOrEmailAddress);
        if (userByEmail == null)
        {
            return;
        }

        login.UserNameOrEmailAddress = userByEmail.UserName;
    }

    /// <summary>
    /// 根据邮箱获取用户
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    protected virtual async Task<SysUser> GetUserByEmailAsync(string email)
    {
        var user = await UserManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new UserFriendlyException(L["Volo.Account:InvalidEmailAddress", email]);
        }

        return user;
    }

    /// <summary>
    /// 生成IdentityUser AccessToken
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private string GenerateAccessToken(SysUser user)
    {
        var dateNow = DateTime.Now;
        var expirationTime = dateNow + TimeSpan.FromHours(JwtOptions.ExpirationTime);
        var key = Encoding.ASCII.GetBytes(JwtOptions.SecurityKey);

        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Audience, JwtOptions.Audience),
            new Claim(JwtClaimTypes.Issuer, JwtOptions.Issuer),
            new Claim(AbpClaimTypes.UserId, user.Id.ToString()),
            new Claim(AbpClaimTypes.Name, user.NickName),
            new Claim(AbpClaimTypes.UserName, user.UserName),
            new Claim(AbpClaimTypes.Email, user.Email),
            new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expirationTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key){},
                SecurityAlgorithms.HmacSha256Signature)
        };
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
    
    
    /// <summary>
    /// 构建前端路由所需要的菜单
    /// </summary>
    /// <param name="menus"></param>
    /// <returns></returns>
    private  List<RouteOutput> BuildMenuRoutes(List<SysMenu> menus)
    {
        List<RouteOutput> routers = new List<RouteOutput>();
        foreach (SysMenu menu in menus)
        {
            RouteOutput router = new RouteOutput();
            router.Hidden = "1".Equals(menu.Visible);
            router.Name = menu.GetRouteName();
            router.Path = menu.GetRouterPath();
            router.Component = menu.GetComponent();;
            router.Query = menu.Query;
            router.Meta = new MetaVo(menu.MenuName, menu.Icon, StringUtils.Equals("0", menu.IsCache), menu.Path);
            List<SysMenu> cMenus = menu.Children.ToList();
            if (!cMenus.IsNullOrEmpty() && cMenus.Count > -1 && UserConstants.TYPE_DIR.Equals(menu.MenuType))
            {
                router.AlwaysShow = true;
                router.Redirect = "noRedirect";
                router.Children = BuildMenuRoutes(cMenus);
            }
            else if (menu.IsMenuFrame())
            {
                router.Meta = null;
                List<RouteOutput> childrenList = new List<RouteOutput>();
                RouteOutput children = new RouteOutput();
                children.Name = menu.GetRouteName();
                children.Path = menu.GetRouterPath();
                children.Component = menu.GetComponent();
                children.Meta = new MetaVo(menu.MenuName, menu.Icon, StringUtils.Equals("0", menu.IsCache), menu.Path);
                children.Query = menu.Query;
                childrenList.Add(children);
                router.Children = childrenList;
            }
            else if (menu.ParentId == -1 && menu.IsInnerLink())
            {
                router.Meta = new MetaVo(menu.MenuName, menu.Icon);
                router.Path = "/";
                var childrenList = new List<RouteOutput>();
                var children = new RouteOutput();
                String routerPath = menu.InnerLinkReplaceEach(menu.Path);
                children.Path = routerPath;
                children.Component = UserConstants.INNER_LINK;
                children.Name = StringUtils.Capitalize(routerPath);
                children.Meta = new MetaVo(menu.MenuName, menu.Icon, menu.Path);
                childrenList.Add(children);
                router.Children = childrenList;
            }
            routers.Add(router);
        }
        return routers;
    }
}