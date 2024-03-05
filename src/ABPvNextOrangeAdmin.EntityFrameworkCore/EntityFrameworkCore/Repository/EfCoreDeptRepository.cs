using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Dept;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ABPvNextOrangeAdmin.EntityFrameworkCore.Repository;

public class EfCoreDeptRepository : EfCoreRepository<ABPvNextOrangeAdminDbContext, SysDept, long>, IDeptRepository
{
    private IRepository<SysRoleDept> roleDeptRepository;

    public EfCoreDeptRepository(IDbContextProvider<ABPvNextOrangeAdminDbContext> dbContextProvider,
        IRepository<SysRoleDept> roleDeptRepository) : base(
        dbContextProvider)
    {
        this.roleDeptRepository = roleDeptRepository;
    }

    #region 通用方法

    public async Task<List<SysDept>> GetChildrenAsync(long? parentId, bool includeDetails = false)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.ParentId == parentId)
            .ToListAsync();
    }

    public async Task<List<SysDept>> GetChildrenWithParentCodeAsync(string codeOrDefault, long? parentId,
        bool includeDetails)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(dept => dept.Code.StartsWith(codeOrDefault) && dept.Id != parentId.Value)
            .ToListAsync(GetCancellationToken());
    }

    public async Task<SysDept> GetDeptByNameAsync(string name, bool includeDetails = true)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(dept => dept.DeptName == name)
            .FirstAsync();
    }

    public async Task<SysDept> GetDeptByIdAsync(int Id, bool includeDetails = true)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(dept => dept.Id == Id)
            .FirstAsync();
    }

    public async Task<List<SysDept>> GetListAsync(string sorting = null, int maxResultCount = Int32.MaxValue,
        int skipCount = 0,
        bool includeDetails = false)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(SysDept.DeptName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(GetCancellationToken()));
    }

    public async Task<List<SysDept>> GetListAsync(IEnumerable<Guid> ids, bool includeDetails = false)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .ToListAsync(GetCancellationToken(GetCancellationToken()));
    }

    #endregion

    public async Task<int> GetRolesCountAsync(SysDept dept, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = from deptRole in dbContext.Set<SysRoleDept>()
            join role in dbContext.Roles on deptRole.RoleId equals role.Id
            where deptRole.RoleId == dept.Id
            select role;

        return await query.CountAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<List<long>> GetdDeptIdsForRole(long roleId)
    {
        var roleDepts = await roleDeptRepository.GetListAsync(a => a.RoleId == roleId);
        var deptIds = roleDepts.Select(a => a.DeptId).ToList();
        return deptIds;
    }

    #region 角色相关

    public async Task<List<SysRole>> GetUnaddedRolesAsync(SysDept dept, string sorting = null,
        int maxResultCount = Int32.MaxValue, int skipCount = 0,
        string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var roleIds = dept.Roles.Select(r => r.RoleId).ToList();
        var dbContext = await GetDbContextAsync();

        return await dbContext.Roles
            .Where(r => !roleIds.Contains(r.Id))
            .IncludeDetails(includeDetails)
            .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.RoleName.Contains(filter))
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(SysRole.RoleName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<int> GetUnaddedRolesCountAsync(SysDept dept, string filter = null,
        CancellationToken cancellationToken = default)
    {
        var roleIds = dept.Roles.Select(r => r.RoleId).ToList();
        var dbContext = await GetDbContextAsync();

        return await dbContext.Roles
            .Where(r => !roleIds.Contains(r.Id))
            .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.RoleName.Contains(filter))
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async Task RemoveAllRolesAsync(SysDept dept)
    {
        var dbContext = await GetDbContextAsync();

        var ouMembersQuery = await dbContext.Set<SysRoleDept>()
            .Where(sysDept => sysDept.DeptId == dept.Id)
            .ToListAsync();
        dbContext.Set<SysRoleDept>().RemoveRange(ouMembersQuery);
    }

    #endregion


    #region 用户相关

    public virtual async Task<List<SysUser>> GetMembersAsync(
        SysDept dept,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var query = await CreateGetMembersFilteredQueryAsync(dept, filter);

        return await query.IncludeDetails(includeDetails)
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(SysUser.UserName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetMembersCountAsync(
        SysDept dept,
        string filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = await CreateGetMembersFilteredQueryAsync(dept, filter);

        return await query.CountAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<List<SysUser>> GetUnaddedUsersAsync(SysDept dept, string sorting = null,
        int maxResultCount = Int32.MaxValue, int skipCount = 0,
        string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var userIdsInDept = dbContext.Set<SysUserDept>()
            .Where(x => x.DeptId == dept.Id)
            .Select(x => x.UserId);

        return await dbContext.Set<SysUser>().IncludeDetails(includeDetails)
            .Where(x => !userIdsInDept.Contains(x.Id))
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.UserName.Contains(filter) || x.PhoneNumber.Contains(filter) || x.Email.Contains(filter))
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUnaddedUsersCountAsync(SysDept dept, string filter = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var userIdsInDept = dbContext.Set<SysUserDept>()
            .Where(x => x.DeptId == dept.Id)
            .Select(x => x.UserId);

        return await dbContext.Set<SysUser>()
            .Where(x => !userIdsInDept.Contains(x.Id))
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.UserName.Contains(filter) || x.PhoneNumber.Contains(filter) || x.Email.Contains(filter))
            .CountAsync(cancellationToken);
    }

    public async Task RemoveAllMembersAsync(SysDept dept)
    {
        var dbContext = await GetDbContextAsync();

        var ouMembersQuery = await dbContext.Set<SysUserDept>()
            .Where(sysDept => sysDept.DeptId == dept.Id)
            .ToListAsync();

        dbContext.Set<SysUserDept>().RemoveRange(ouMembersQuery);
    }

    protected virtual async Task<IQueryable<SysUser>> CreateGetMembersFilteredQueryAsync(SysDept dept,
        string filter = null)
    {
        var dbContext = await GetDbContextAsync();

        var query = from userDept in dbContext.Set<SysUserDept>()
            join user in dbContext.Users on userDept.UserId equals user.Id
            where userDept.DeptId == dept.Id
            select user;

        if (!filter.IsNullOrWhiteSpace())
        {
            query = query.Where(u =>
                u.UserName.Contains(filter) ||
                u.Email.Contains(filter) ||
                (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
            );
        }

        return query;
    }

    #endregion
}