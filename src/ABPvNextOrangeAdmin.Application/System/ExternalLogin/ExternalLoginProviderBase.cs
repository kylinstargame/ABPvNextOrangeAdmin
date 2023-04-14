using System;
using System.Linq;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.User;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.ExternalLogin;

public abstract class ExternalLoginProviderBase : IExternalLoginProvider
{
    private IGuidGenerator GuidGenerator;
    protected ExternalLoginProviderBase(IOptions<IdentityOptions> identityOptions, ICurrentTenant currentTenant, IUserRepository userRepository, UserManager userManager, IGuidGenerator guidGenerator)
    {
        IdentityOptions = identityOptions;
        CurrentTenant = currentTenant;
        UserRepository = userRepository;
        UserManager = userManager;
        GuidGenerator = guidGenerator;
    }

    public abstract Task<bool> TryAuthenticateAsync(string userName, string plainPassword);
    protected IOptions<IdentityOptions> IdentityOptions { get; }

    protected UserManager UserManager { get; }
    
    protected IUserRepository UserRepository { get; }

    protected ICurrentTenant CurrentTenant { get; }

    public abstract Task<bool> IsEnabledAsync();

    public async Task<SysUser> CreateUserAsync(string userName, string providerName)
    {
        await IdentityOptions.SetAsync();

        var externalUser = await GetUserInfoAsync(userName);

        return await CreateUserAsync(externalUser, userName, providerName);
    }

    protected virtual async Task<SysUser> CreateUserAsync(ExternalLoginUserInfo externalUser, string userName,
        string providerName)
    {
        NormalizeExternalLoginUserInfo(externalUser, userName);

        var user = new SysUser(
            GuidGenerator.Create(),
            userName,
            externalUser.Email,
            "",
            tenantId: CurrentTenant.Id
        );

        user.UserName = externalUser.Name;

        // user.IsExternal = true;

        // user.SetEmailConfirmed(externalUser.EmailConfirmed ?? false);
        // user.SetPhoneNumber(externalUser.PhoneNumber, externalUser.PhoneNumberConfirmed ?? false);

        (await UserManager.CreateAsync(user)).CheckErrors();

        if (externalUser.TwoFactorEnabled != null)
        {
            (await UserManager.SetTwoFactorEnabledAsync(user, externalUser.TwoFactorEnabled.Value)).CheckErrors();
        }

        (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();
        (await UserManager.AddLoginAsync(
                    user,
                    new UserLoginInfo(
                        providerName,
                        externalUser.ProviderKey,
                        providerName
                    )
                )
            ).CheckErrors();

        return user;
    }

    public async Task UpdateUserAsync(SysUser user, string providerName)
    {
        await IdentityOptions.SetAsync();

        var externalUser = await GetUserInfoAsync(user);

        await UpdateUserAsync(user, externalUser, providerName);
    }

    protected virtual async Task UpdateUserAsync(SysUser user, ExternalLoginUserInfo externalUser,
        string providerName)
    {
             NormalizeExternalLoginUserInfo(externalUser, user.UserName);

        if (!externalUser.Name.IsNullOrWhiteSpace())
        {
            user.UserName = externalUser.Name;
        }

        // if (!externalUser.Surname.IsNullOrWhiteSpace())
        // {
        //     user.Surname = externalUser.Surname;
        // }

        if (user.PhoneNumber != externalUser.PhoneNumber)
        {
            if (!externalUser.PhoneNumber.IsNullOrWhiteSpace())
            {
                await UserManager.SetPhoneNumberAsync(user, externalUser.PhoneNumber);
                // user.SetPhoneNumberConfirmed(externalUser.PhoneNumberConfirmed == true);
            }
        }
        // else
        // {
        //     if (!user.PhoneNumber.IsNullOrWhiteSpace() &&
        //         user.PhoneNumberConfirmed == false &&
        //         externalUser.PhoneNumberConfirmed == true)
        //     {
        //         user.SetPhoneNumberConfirmed(true);
        //     }
        // }

        if (!string.Equals(user.Email, externalUser.Email, StringComparison.OrdinalIgnoreCase))
        {
            (await UserManager.SetEmailAsync(user, externalUser.Email)).CheckErrors();
            // user.SetEmailConfirmed(externalUser.EmailConfirmed ?? false);
        }

        if (externalUser.TwoFactorEnabled != null)
        {
            (await UserManager.SetTwoFactorEnabledAsync(user, externalUser.TwoFactorEnabled.Value)).CheckErrors();
        }

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins);

        var userLogin = user.Logins.FirstOrDefault(l => l.LoginProvider == providerName);
        if (userLogin != null)
        {
            if (userLogin.ProviderKey != externalUser.ProviderKey)
            {
                (await UserManager.RemoveLoginAsync(user, providerName, userLogin.ProviderKey)).CheckErrors();
                (await UserManager.AddLoginAsync(user, new UserLoginInfo(providerName, externalUser.ProviderKey, providerName))).CheckErrors();
            }
        }
        else
        {
            (await UserManager.AddLoginAsync(user, new UserLoginInfo(providerName, externalUser.ProviderKey, providerName))).CheckErrors();
        }

        // user.IsExternal = true;

        (await UserManager.UpdateAsync(user)).CheckErrors();
    }

    protected abstract Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName);
    
    protected virtual Task<ExternalLoginUserInfo> GetUserInfoAsync(SysUser user)
    {
        return GetUserInfoAsync(user.UserName);
    }

    private static void NormalizeExternalLoginUserInfo(ExternalLoginUserInfo externalUser, string userName)
    {
        if (String.IsNullOrWhiteSpace(externalUser.ProviderKey))
        {
            externalUser.ProviderKey = userName;
        }
    }
}

public class ExternalLoginUserInfo
{
    [CanBeNull] public string Name { get; set; }

    [CanBeNull] public string PhoneNumber { get; set; }

    [NotNull] public string Email { get; private set; }

    [CanBeNull] public bool? PhoneNumberConfirmed { get; set; }

    [CanBeNull] public bool? EmailConfirmed { get; set; }

    [CanBeNull] public bool? TwoFactorEnabled { get; set; }

    [CanBeNull] public string ProviderKey { get; set; }

    public ExternalLoginUserInfo([NotNull] string email)
    {
        Email = Check.NotNullOrWhiteSpace(email, nameof(email));
    }
}