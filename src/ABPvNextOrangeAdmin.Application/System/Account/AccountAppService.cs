using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.Constans;
using ABPvNextOrangeAdmin.CustomException;
using ABPvNextOrangeAdmin.Exception;
using ABPvNextOrangeAdmin.Options;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Config;
using ABPvNextOrangeAdmin.Utils;
using ABPvNextOrangeAdmin.Utils.ImageProducer;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Account.Settings;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.AspNetIdentity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

// ReSharper disable StringLastIndexOfIsCultureSpecific.1

namespace ABPvNextOrangeAdmin.System.Account;

[Route("api/sys/account/[action]")]
public class AccountAppService : ApplicationService, IAccountAppService
{
    private JwtOptions JwtOptions { get; }
    protected SignInManager<IdentityUser> SignInManager { get; }

    private IIdentityRoleRepository RoleRepository { get; }

    private IdentityUserManager UserManager { get; }

    private IAccountEmailer AccountEmailer { get; }

    private IdentitySecurityLogManager IdentitySecurityLogManager { get; }

    private IOptions<IdentityOptions> IdentityOptions { get; }

    private DefaultCaptcha DefaultCaptcha { get; }

    private ConfigDomainService ConfigDomainService { get; }

    private IDistributedCache<String> DistributedCache { get; }

    private ITokenService TokenService { get; }

    private IRefreshTokenService RefreshTokenService { get; }


    public AccountAppService(IIdentityRoleRepository roleRepository, IdentityUserManager userManager,
        IAccountEmailer accountEmailer,IdentitySecurityLogManager identitySecurityLogManager,
        IOptions<IdentityOptions> identityOptions, DefaultCaptcha defaultCaptcha,
        ConfigDomainService configDomainService, IDistributedCache<String> distributedCache, ITokenService tokenService,
        IRefreshTokenService refreshTokenService, IOptions<JwtOptions> jwtOptions)
    {
        RoleRepository = roleRepository;
        UserManager = userManager;
        AccountEmailer = accountEmailer;
        IdentitySecurityLogManager = identitySecurityLogManager;
        IdentityOptions = identityOptions;
        DefaultCaptcha = defaultCaptcha;
        ConfigDomainService = configDomainService;
        DistributedCache = distributedCache;
        TokenService = tokenService;
        RefreshTokenService = refreshTokenService;
        JwtOptions = jwtOptions.Value;
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("register")]
    public async Task<CommonResult<IdentityUserDto>> RegisterAsync(RegisterInput input)
    {
        await CheckSelfRegistrationAsync();

        await IdentityOptions.SetAsync();

        //创建新用户
        var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);
        input.MapExtraPropertiesTo(user);

        (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

        await UserManager.SetEmailAsync(user, input.EmailAddress);
        await UserManager.AddDefaultRolesAsync(user);

        var identityUserDto = ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        return CommonResult<IdentityUserDto>.Success(identityUserDto, "注册账户完成");
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("login")]
    public async Task<CommonResult<String>> LoginAsync(LoginInput input)
    {
        Boolean captchaOnOff = await ConfigDomainService.SelectCaptchaOnOff();

        // 验证码开关
        if (captchaOnOff)
        {
            await ValidateCaptcha(input.UserNameOrEmailAddress, input.Code, input.Uuid);
        }

        await CheckLocalLoginAsync();

        ValidateLoginInfo(input);

        await ReplaceEmailToUsernameOfInputIfNeeds(input);
        var signInResult = await SignInManager.PasswordSignInAsync(
            input.UserNameOrEmailAddress,
            input.Password,
            input.RememberMe,
            true
        );

        var token = "";
        //生成AccessToken
        if (signInResult.Succeeded)
        { 
            token = GenerateAccessToken(await  UserManager.FindByNameAsync(input.UserNameOrEmailAddress));
        }

        //登录日志
        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = signInResult.ToIdentitySecurityLogAction(),
            UserName = input.UserNameOrEmailAddress
        });
        
        return CommonResult<String>.Success(signInResult.ToIdentitySecurityLogAction(), token);
    }

    /// <summary>
    /// 用户注销
    /// </summary>
    /// <returns></returns>
    [HttpGet]
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


    /// <summary>
    /// 修改密码 发送邮件
    /// </summary>
    /// <param name="input"></param>
    [HttpPost]
    [ActionName("passwordResetCode")]
    public async Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input)
    {
        var user = await GetUserByEmailAsync(input.Email);
        var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
        await AccountEmailer.SendPasswordResetLinkAsync(user, resetToken, input.AppName, input.ReturnUrl,
            input.ReturnUrlHash);
    }


    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="input"></param>
    [HttpPost]
    [ActionName("resetPassword")]
    public async Task ResetPasswordAsync(ResetPasswordDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(input.UserId);
        (await UserManager.ResetPasswordAsync(user, input.ResetToken, input.Password)).CheckErrors();

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.ChangePassword
        });
    }

    /// <summary>
    /// 生成验证码 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    [ActionName("captchaImage")]
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
            CaptchaCodeOutput.CreateInstance(guid.ToString(), capText), "生成验证码成功");
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
        String captcha = (await DistributedCache.GetAsync(verifyKey)).ToString();
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
    /// 检查用户是否启用本地注册
    /// </summary>
    /// <exception cref="UserFriendlyException"></exception>
    protected virtual async Task CheckSelfRegistrationAsync()
    {
        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled))
        {
            throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
        }
    }

    /// <summary>
    /// 检查是否启用本地登录
    /// </summary>
    /// <exception cref="UserFriendlyException"></exception>
    protected virtual async Task CheckLocalLoginAsync()
    {
        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
        {
            throw new UserFriendlyException(L["LocalLoginDisabledMessage"]);
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
    protected virtual async Task<IdentityUser> GetUserByEmailAsync(string email)
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
    /// <param name="identityUser"></param>
    /// <returns></returns>
    private string GenerateAccessToken(IdentityUser identityUser)
    {
        var dateNow = DateTime.Now;
        var expirationTime = dateNow + TimeSpan.FromHours(JwtOptions.ExpirationTime);
        var key = Encoding.ASCII.GetBytes(JwtOptions.SecurityKey);
        
        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Audience, JwtOptions.Audience),
            new Claim(JwtClaimTypes.Issuer, JwtOptions.Issuer),
            new Claim(AbpClaimTypes.UserId, identityUser.Id.ToString()),
            new Claim(AbpClaimTypes.Name,  identityUser.Name),
            new Claim(AbpClaimTypes.UserName,  identityUser.UserName),
            new Claim(AbpClaimTypes.Email, identityUser.Email),
            new Claim(AbpClaimTypes.TenantId, identityUser.TenantId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expirationTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}