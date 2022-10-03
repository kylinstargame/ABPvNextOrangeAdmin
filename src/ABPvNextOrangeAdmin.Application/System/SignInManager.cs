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

namespace ABPvNextOrangeAdmin.System;

public class SignInManager : SignInManager<SysUser>, ITransientDependency
{
    public SignInManager(UserManager<SysUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<SysUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<SysUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<SysUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }

    public SignInManager(UserManager userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<SysUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<SysUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<SysUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }

    protected SysUserOptions Options { get; }

    protected override async Task<SignInResult> PreSignInCheck(SysUser user)
    {
        if (user.IsActive)
        {
            Logger.LogWarning($"The user is not active therefore cannot login! (username: {user.UserName}, id:{user.Id})");
            return SignInResult.NotAllowed;
        }
        return await base.PreSignInCheck(user);
    }

    public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
    {
        foreach (var externalLoginProviderInfo in Options.ExternalLoginProviders.Values)
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
                        user = await externalLoginProviderWithPassword.CreateUserAsync(userName, externalLoginProviderInfo.Name, password);
                    }
                    else
                    {
                        user = await externalLoginProvider.CreateUserAsync(userName, externalLoginProviderInfo.Name);
                    }
                }
                else
                {
                    if (externalLoginProvider is IExternalLoginProviderWithPassword externalLoginProviderWithPassword)
                    {
                        await externalLoginProviderWithPassword.UpdateUserAsync(user, externalLoginProviderInfo.Name, password);
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