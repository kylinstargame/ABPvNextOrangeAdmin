using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.Roles;

public class SysRoleStore : IRoleStore<SysRole>, ITransientDependency
{
    public SysRoleStore(IRoleRepository roleRepository, ILookupNormalizer lookupNormalizer)
    {
        RoleRepository = roleRepository;
        LookupNormalizer = lookupNormalizer;
    }

    private IRoleRepository RoleRepository { get; set; }

    private ILookupNormalizer LookupNormalizer { get; set; }

    public bool AutoSaveChanges { get; set; } = true;

    public void Dispose()
    {
    }

    #region 角色相关

    public async Task<IdentityResult> CreateAsync(SysRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        await RoleRepository.InsertAsync(role, AutoSaveChanges, cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(SysRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        await RoleRepository.UpdateAsync(role, AutoSaveChanges, cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(SysRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        await RoleRepository.DeleteAsync(role, AutoSaveChanges, cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<SysRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var role = await RoleRepository.FindAsync(long.Parse(roleId), AutoSaveChanges, cancellationToken);
        return role;
    }

    public async Task<SysRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var role = await RoleRepository.FindByNameAsync(normalizedRoleName, AutoSaveChanges, cancellationToken);
        return role;
    }

    #endregion

    #region 角色信息

    public Task<string> GetRoleIdAsync(SysRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(role, nameof(role));

        return Task.FromResult(role.Id.ToString());
    }

    public Task<string> GetRoleNameAsync(SysRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(role, nameof(role));

        return Task.FromResult(role.RoleName.ToString());
    }

    public Task SetRoleNameAsync(SysRole role, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        role.RoleName = roleName;

        return Task.CompletedTask;
    }

    public Task<string> GetNormalizedRoleNameAsync(SysRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(role, nameof(role));

        return Task.FromResult(LookupNormalizer.NormalizeName(role.RoleName));
    }

    public Task SetNormalizedRoleNameAsync(SysRole role, string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        role.RoleName = LookupNormalizer.NormalizeName(normalizedName);

        return Task.CompletedTask;
    }

    #endregion
}