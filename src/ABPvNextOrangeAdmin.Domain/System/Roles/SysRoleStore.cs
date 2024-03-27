using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.Roles;

public class SysRoleStore : IRoleStore<SysRole>, ITransientDependency
{
    public SysRoleStore(IRoleRepository roleRepository, ILookupNormalizer lookupNormalizer,
        IRepository<SysUserRole> roleUserRepository, IRepository<SysUser> userRepository)
    {
        RoleRepository = roleRepository;
        LookupNormalizer = lookupNormalizer;
        UserRoleRepository = roleUserRepository;
        UserRepository = userRepository;
    }

    private IRoleRepository RoleRepository { get; set; }
    public IRepository<SysUserRole> UserRoleRepository { get; set; }
    public IRepository<SysUser> UserRepository { get; set; }

    private ILookupNormalizer LookupNormalizer { get; set; }

    public bool AutoSaveChanges { get; set; } = true;

    public void Dispose()
    {
    }

    #region 角色相关

    public async Task<IdentityResult> CreateAsync(SysRole role, CancellationToken cancellationToken = default)
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

    public async Task<IdentityResult> DeleteAsync(SysRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        await RoleRepository.DeleteAsync(role, AutoSaveChanges, cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<SysRole> FindByIdAsync(string roleId, CancellationToken cancellationToken = default)
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

    /// <summary>
    /// 获取角色的授权用户列表
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userName"></param>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public async Task<List<SysUser>> GetAllocatedUserList(long roleId, string userName, string phoneNumber)
    {
        var userIds = (await UserRoleRepository.GetListAsync(ur => ur.RoleId == roleId)).Select(ur => ur.UserId)
            .ToList();
        var users = (await UserRepository.GetListAsync()).ToList()
            .WhereIf(!String.IsNullOrEmpty(userName), u => u.UserName == userName)
            .WhereIf(!String.IsNullOrEmpty(phoneNumber), u => u.PhoneNumber == phoneNumber)
            .Where(u => userIds.Contains(u.Id)).ToList();

        return users;
    }

    /// <summary>
    ///  获取角色的未授权用户列表
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userName"></param>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public async Task<List<SysUser>> GetUnallocatedUserList(long roleId, string userName, string phoneNumber)
    {
        var userIds = (await UserRoleRepository.GetListAsync(ur => ur.RoleId == roleId)).Select(ur => ur.UserId)
            .ToList();
        var users = (await UserRepository.GetListAsync()).ToList()
            .WhereIf(!String.IsNullOrEmpty(userName), u => u.UserName == userName)
            .WhereIf(!String.IsNullOrEmpty(phoneNumber), u => u.PhoneNumber == phoneNumber)
            .Where(u => !userIds.Contains(u.Id)).ToList();

        return users;
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    public async Task CancelUserAth(string userId, long roleId)
    {
        await UserRoleRepository.DeleteAsync(a => a.RoleId == roleId && a.UserId == Guid.Parse(userId));
    }
    public async Task CancelUserAth(string[] userIds, long roleId)
    {
        var userRoles = SysUserRole.CreateInstances(userIds, roleId);
        await UserRoleRepository.DeleteManyAsync(userRoles);
    }

    public async Task ComfirmUserAth(string[] userIds, long roleId)
    {
        var userRoles = SysUserRole.CreateInstances(userIds, roleId);
        await UserRoleRepository.InsertManyAsync(userRoles);
    }
}