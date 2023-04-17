using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Dept;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using ABPvNextOrangeAdmin.System.User.Exstension;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;


namespace ABPvNextOrangeAdmin.EntityFrameworkCore.Repository;

public class EfCoreUserRepository : EfCoreRepository<ABPvNextOrangeAdminDbContext, SysUser, Guid>, IUserRepository
{
    public EfCoreUserRepository(IDbContextProvider<ABPvNextOrangeAdminDbContext> dbContextProvider,
        ILookupNormalizer lookupNormalizer) : base(
        dbContextProvider)
    {
        LookNormalizer = lookupNormalizer;
    }

    public ILookupNormalizer LookNormalizer { get; set; }
    
    public async Task<SysUser> FindByNormalizedUserNameAsync(string userName, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                u => u.UserName == userName
            );
    }

    public async Task<List<string>> GetRoleNamesAsync(Guid id, CancellationToken cancellationToken = default)
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
        ).ToListAsync();

        var deptRoleNameQuery = dbContext.Roles.Where(r => deptRoleIds.Contains(r.Id)).Select(n => n.RoleName);
        var resultQuery = query.Union(deptRoleNameQuery);
        return await resultQuery.ToListAsync(GetCancellationToken());
    }

    public async Task<List<string>> GetRoleNamesInOrganizationUnitAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = from userDept in dbContext.Set<SysUserDept>()
            join roleDept in dbContext.Set<SysRoleDept>() on userDept.DeptId equals roleDept.DeptId
            join dept in dbContext.Set<SysDept>() on roleDept.DeptId equals dept.Id
            join roles in dbContext.Roles on roleDept.RoleId equals roles.Id
            where userDept.UserId == id
            select roles.RoleName;

        var result = await query.ToListAsync(GetCancellationToken());

        return result;
    }

    public async Task<List<SysUser>> GetListByNormalizedRoleNameAsync(string normalizedRoleName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return (await (await GetDbSetAsync()).OrderBy(x => x.Id).ToListAsync()).Where(
            u=>LookNormalizer.NormalizeName(u.UserName) == normalizedRoleName
        ).ToList();
    }

    public Task<SysUser> FindByLoginAsync(string loginProvider, string providerKey, bool includeDetails = true,
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
                    identityUser.Depts.Any(x => x.DeptId == deptId))
            .WhereIf(!string.IsNullOrWhiteSpace(userName), x => x.UserName == userName)
            .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), x => x.PhoneNumber == phoneNumber)
            .WhereIf(!string.IsNullOrWhiteSpace(emailAddress), x => x.Email == emailAddress)
            .WhereIf(isLockedOut == true, x => x.LockoutEnabled && x.LockoutEnd > DateTimeOffset.UtcNow)
            .WhereIf(notActive == true, x => !x.IsActive)
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }
}