using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Roles;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace ABPvNextOrangeAdmin.System.User;

public class SysRoleStore :
    IRoleStore<SysRole>,
    // IRoleClaimStore<SysRole>,
    ITransientDependency
{
    public SysRoleStore(IRoleRepository roleRepository, ILogger<SysRoleStore> logger)
    {
        RoleRepository = roleRepository;
        Logger = logger;
    }

    public IRoleRepository RoleRepository { get; set; }

    protected ILogger<SysRoleStore> Logger { get; }

    public bool AutoSaveChanges { get; set; } = true;

    public IdentityErrorDescriber ErrorDescriber { get; set; }


    public async Task<IdentityResult> CreateAsync([NotNull] SysRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(role, nameof(role));

        await RoleRepository.InsertAsync(role, AutoSaveChanges, cancellationToken);

        return IdentityResult.Success;
    }


    public async Task<IdentityResult> UpdateAsync([NotNull] SysRole role, CancellationToken cancellationToken)
    {
        Check.NotNull(role, nameof(role));

        try
        {
            await RoleRepository.UpdateAsync(role, AutoSaveChanges, cancellationToken);
        }
        catch (global::System.Exception ex)
        {
            Logger.LogWarning(ex.ToString()); //Trigger some AbpHandledException event
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync([NotNull] SysRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(role, nameof(role));

        try
        {
            await RoleRepository.DeleteAsync(role, AutoSaveChanges, cancellationToken);
        }
        catch (global::System.Exception ex)
        {
            Logger.LogWarning(ex.ToString()); //Trigger some AbpHandledException event
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    public Task<string> GetRoleIdAsync([NotNull] SysRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(role, nameof(role));

        return Task.FromResult(role.Id.ToString());
    }

    public Task<string> GetRoleNameAsync([NotNull] SysRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(role, nameof(role));

        return Task.FromResult(role.RoleName);
    }

    public Task SetRoleNameAsync([NotNull] SysRole role, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(role, nameof(role));

        role.ChangeName(roleName);
        return Task.CompletedTask;
    }

    public Task<string> GetNormalizedRoleNameAsync([NotNull]SysRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(role, nameof(role));

        return Task.FromResult(role.RoleName);
    }
    
    public Task SetNormalizedRoleNameAsync(SysRole role, string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(role, nameof(role));

        role.RoleName = normalizedName;

        return Task.CompletedTask;
    }

    public Task<SysRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return RoleRepository.FindAsync(int.Parse(roleId), cancellationToken: cancellationToken);
    }

    public Task<SysRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(normalizedRoleName, nameof(normalizedRoleName));

        return RoleRepository.FindByNameAsync(normalizedRoleName, cancellationToken: cancellationToken);
    }

    public virtual void Dispose()
    {
    }

    // public Task<IList<Claim>> GetClaimsAsync(SysRole role,
    //     CancellationToken cancellationToken = new CancellationToken())
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task AddClaimAsync(SysRole role, Claim claim, CancellationToken cancellationToken = new CancellationToken())
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task RemoveClaimAsync(SysRole role, Claim claim,
    //     CancellationToken cancellationToken = new CancellationToken())
    // {
    //     throw new NotImplementedException();
    // }
}