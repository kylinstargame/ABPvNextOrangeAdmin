using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.User.Exstension;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace ABPvNextOrangeAdmin.System.User;

public class SysUserStore : IUserPasswordStore<SysUser>,
    IUserRoleStore<SysUser>,ITransientDependency
{
    
    public SysUserStore(IUserRepository userRepository,SysUserErrorDescriber errorDescriber, IRoleRepository roleRepository, ILookupNormalizer lookupNormalizer)
    {
        UserRepository = userRepository;
        AutoSaveChanges = true;
        ErrorDescriber = errorDescriber;
        RoleRepository = roleRepository;
        LookupNormalizer = lookupNormalizer;
    }

    private IUserRepository UserRepository { get; }

    private IRoleRepository RoleRepository { get; }


    private ILookupNormalizer LookupNormalizer { get; }
    
    /// <summary>
    /// Gets or sets a flag indicating if changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
    /// </summary>
    /// <value>
    /// True if changes should be automatically persisted, otherwise false.
    /// </value>
    public bool AutoSaveChanges { get; set; } = true;
    
    // protected ILogger<IRoleStore> Logger { get; }
    
    public IdentityErrorDescriber ErrorDescriber { get; set; }

    
    public void Dispose()
    {
    }

    #region 用户信息

    public Task<string> GetUserIdAsync(SysUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(SysUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult(user.UserName);
    }

    public Task SetUserNameAsync(SysUser user, string userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.UserName = userName;

        return Task.CompletedTask;
    }

    public Task<string> GetNormalizedUserNameAsync(SysUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        return Task.FromResult( LookupNormalizer.NormalizeName(user.UserName));
    }

    public Task SetNormalizedUserNameAsync(SysUser user, string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        user.UserName = LookupNormalizer.NormalizeName(normalizedName);

        return Task.CompletedTask;
    }

    #endregion

    #region 用户相关 

    public async Task<IdentityResult> CreateAsync(SysUser user, CancellationToken cancellationToken)
    {
        
        Check.NotNull(user, nameof(user));

        await UserRepository.InsertAsync(user, AutoSaveChanges, cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(SysUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        try
        {
            await UserRepository.UpdateAsync(user, AutoSaveChanges, cancellationToken);
        }
        catch (AbpDbConcurrencyException ex)
        {
            // Logger.LogWarning(ex.ToString()); //Trigger some AbpHandledException event
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(SysUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        try
        {
            await UserRepository.DeleteAsync(user, AutoSaveChanges, cancellationToken);
        }
        catch (AbpDbConcurrencyException ex)
        {
            // Logger.LogWarning(ex.ToString()); //Trigger some AbpHandledException event
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    public Task<SysUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return UserRepository.FindAsync(Guid.Parse(userId), cancellationToken: cancellationToken);
    }

    public Task<SysUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(normalizedUserName, nameof(normalizedUserName));

        return UserRepository.FindByNormalizedUserNameAsync(normalizedUserName, includeDetails: true, cancellationToken: cancellationToken);

    }

    #endregion

    #region 角色相关

    public async Task AddToRoleAsync(SysUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNull(roleName, nameof(roleName));

        if (await IsInRoleAsync(user, roleName, cancellationToken))
        {
            return;
        }

        var role = await RoleRepository.FindByNameAsync(roleName, cancellationToken: cancellationToken);
        if (role == null)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Role {0} does not exist!", roleName));
        }

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles, cancellationToken);

        user.AddRole(role.Id);
    }

    public async Task RemoveFromRoleAsync(SysUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNullOrWhiteSpace(roleName, nameof(roleName));

        var role = await RoleRepository.FindByNameAsync(roleName, cancellationToken: cancellationToken);
        if (role == null)
        {
            return;
        }

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles, cancellationToken);

        user.RemoveRole(role.Id);
    }

    public async Task<IList<string>> GetRolesAsync(SysUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        var userRoles = await UserRepository
            .GetRoleNamesAsync(user.Id, cancellationToken: cancellationToken);

        var userOrganizationUnitRoles = await UserRepository
            .GetRoleNamesInOrganizationUnitAsync(user.Id, cancellationToken: cancellationToken);

        return userRoles.Union(userOrganizationUnitRoles).ToList();
    }

    public async Task<bool> IsInRoleAsync(SysUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNullOrWhiteSpace(roleName, nameof(roleName));

        var roles = await GetRolesAsync(user, cancellationToken);

        return roles
            .Select(r => LookupNormalizer.NormalizeName(r))
            .Contains(roleName);
    }

    public async Task<IList<SysUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrEmpty(roleName))
        {
            throw new ArgumentNullException(nameof(roleName));
        }

        return await UserRepository.GetListByNormalizedRoleNameAsync(roleName, cancellationToken: cancellationToken);
    }

    #endregion

    public Task SetPasswordHashAsync(SysUser user, string passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string> GetPasswordHashAsync(SysUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(SysUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.PasswordHash != null);
    }
}