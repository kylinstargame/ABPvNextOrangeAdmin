using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;


namespace ABPvNextOrangeAdmin.EntityFrameworkCore.Repository;

public class EfCoreUserRepository : EfCoreRepository<ABPvNextOrangeAdminDbContext, SysUser, long>, IUserRepository
{
    public EfCoreUserRepository(IDbContextProvider<ABPvNextOrangeAdminDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public async Task<SysUser> FindByNormalizedUserNameAsync(string userName, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            // .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                u => u.UserName == userName,
                GetCancellationToken(cancellationToken)
            );
    }

    public async Task<List<string>> GetRoleNamesAsync(long id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        // 用户角色
        var query = from userRole in dbContext.Set<SysUserRole>()
            join role in dbContext.Roles on userRole.RoleId equals role.Id
            where userRole.UserId == id
            select role.RoleName;
        var deptIds = dbContext.Set<SysUserDept>().Where(q => q.UserId == id).Select(q => q.DeptId).ToArray();
        
        //部门角色
        var deptRoleIds = await (
            from roleDept in dbContext.Set<SysRoleDept>()
            join dept in dbContext.Set<SysDept>() on roleDept.DeptId equals dept.Id
            where deptIds.Contains(roleDept.DeptId)
            select roleDept.RoleId
        ).ToListAsync(GetCancellationToken(cancellationToken));

        var deptRoleNameQuery = dbContext.Roles.Where(r => deptRoleIds.Contains(r.Id)).Select(n => n.RoleName);
        var resultQuery = query.Union(deptRoleNameQuery);
        return await resultQuery.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public Task<List<string>> GetRoleNamesInOrganizationUnitAsync(long id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<SysUser> FindByLoginAsync(string loginProvider, string providerKey, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<SysUser> FindByNormalizedEmailAsync(string normalizedEmail, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<SysUser>> GetListByClaimAsync(Claim claim, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<SysUser>> GetListByNormalizedRoleNameAsync(string normalizedRoleName, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<SysUser>> GetListAsync(string sorting = null, int maxResultCount = Int32.MaxValue,
        int skipCount = 0, string filter = null,
        bool includeDetails = false, Guid? roleId = null, Guid? organizationUnitId = null, string userName = null,
        string phoneNumber = null, string emailAddress = null, bool? isLockedOut = null, bool? notActive = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<SysRole>> GetRolesAsync(Guid id, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<SysUser>> GetUsersInOrganizationUnitAsync(Guid organizationUnitId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<SysUser>> GetUsersInOrganizationsListAsync(List<Guid> organizationUnitIds,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<SysUser>> GetUsersInOrganizationUnitWithChildrenAsync(string code,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<long> GetCountAsync(string filter = null, long roleId = -1, long deptId = -1,
        string userName = null,
        string phoneNumber = null, string emailAddress = null, bool? isLockedOut = null, bool? notActive = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.UserName.Contains(filter) ||
                    u.Email.Contains(filter) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
            )
            .WhereIf(roleId >= 0, sysUser => sysUser.Roles.Any(x => x.RoleId == roleId))
            .WhereIf(deptId >= 0,
                identityUser =>
                    identityUser.Depts.Any(x => x.Id == deptId))
            .WhereIf(!string.IsNullOrWhiteSpace(userName), x => x.UserName == userName)
            .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), x => x.PhoneNumber == phoneNumber)
            .WhereIf(!string.IsNullOrWhiteSpace(emailAddress), x => x.Email == emailAddress)
            .WhereIf(isLockedOut == true, x => x.LockoutEnabled && x.LockoutEnd > DateTimeOffset.UtcNow)
            .WhereIf(notActive == true, x => !x.IsActive)
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }
}