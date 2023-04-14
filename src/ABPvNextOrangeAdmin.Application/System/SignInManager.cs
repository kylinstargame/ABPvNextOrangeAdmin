using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.ExternalLogin;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using UserManager = ABPvNextOrangeAdmin.System.User.UserManager;

namespace ABPvNextOrangeAdmin.System;

public class SignInManager : SignInManager<SysUser>, ITransientDependency
{
    public SignInManager(UserManager userManager, IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<SysUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<SysUser>> logger, IAuthenticationSchemeProvider schemes,
        IUserConfirmation<SysUser> confirmation, IOptions<SignInOptions> signInOption)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
        SignInOptions = signInOption.Value;
    }


    protected SignInOptions SignInOptions{ get; }

    protected override async Task<SignInResult> PreSignInCheck(SysUser user)
    {
        if (!user.IsActive)
        {
            Logger.LogWarning("The user is not active therefore cannot login! (username: {0}, id:{1})", user.UserName, user.Id);
            return SignInResult.NotAllowed;
        }
        
        return  await base.PreSignInCheck(user);
    }

    /// <summary>
    /// 密码登录
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="isPersistent">记住我</param>
    /// <param name="lockoutOnFailure">登录失败是否锁定账户</param>
    /// <returns></returns>
    public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent,
        bool lockoutOnFailure)
    {
        foreach (var externalLoginProviderInfo in SignInOptions.ExternalLoginProviders.Values)
        {
            var externalLoginProvider = (IExternalLoginProvider)Context.RequestServices
                .GetRequiredService(externalLoginProviderInfo.Type);
            if (await externalLoginProvider.TryAuthenticateAsync(userName, password))
            {
                var user = await UserManager.FindByNameAsync(userName);
                if (user == null)
                {
                    if (externalLoginProvider is IExternalLoginProviderWithPassword externalLoginProviderWithPassword)
                    {
                        user = await externalLoginProviderWithPassword.CreateUserAsync(userName,
                            externalLoginProviderInfo.Name, password);
                    }
                    else
                    {
                        user = await externalLoginProvider.CreateUserAsync(userName, externalLoginProviderInfo.Name);
                    }
                }
                else
                {
                    var externalLoginProviderWithPassword = externalLoginProvider as IExternalLoginProviderWithPassword;
                    if (externalLoginProviderWithPassword != null)
                    {
                        await externalLoginProviderWithPassword.UpdateUserAsync(user, externalLoginProviderInfo.Name,
                            password);
                    }
                    else
                    {
                        await externalLoginProvider.UpdateUserAsync(user, externalLoginProviderInfo.Name);
                    }
                }

                return await SignInOrTwoFactorAsync(user, isPersistent);
            }
        }

        return await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
    }
}