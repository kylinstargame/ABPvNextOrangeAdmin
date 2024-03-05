using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity.Settings;

namespace ABPvNextOrangeAdmin.System.Dept;

public interface IDeptRepository : IBasicRepository<SysDept, long>
{
    #region 通用方法

    /// <summary>
    /// 根Id获取所有子部门
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="includeDetails"></param>
    /// <returns></returns>
    Task<List<SysDept>> GetChildrenAsync(long? parentId, bool includeDetails);

    /// <summary>
    /// 根据组织代码获取子部门 
    /// </summary>
    /// <param name="codeOrDefault"></param>
    /// <param name="parentId"></param>
    /// <param name="includeDetails"></param>
    /// <returns></returns>
    Task<List<SysDept>> GetChildrenWithParentCodeAsync(string codeOrDefault, long? parentId, bool includeDetails);

    Task<SysDept> GetDeptByNameAsync(
        string name,
        bool includeDetails = true
    );

    Task<SysDept> GetDeptByIdAsync(
        int Id,
        bool includeDetails = true
    );
    Task<List<SysDept>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = false
    );

    Task<List<SysDept>> GetListAsync(
        IEnumerable<Guid> ids,
        bool includeDetails = false
    );

    #endregion

    #region 角色相关

    Task<int> GetRolesCountAsync(
        SysDept dept,
        CancellationToken cancellationToken = default
    );

    Task<List<long>> GetdDeptIdsForRole(long roleId);

    Task<List<SysRole>> GetUnaddedRolesAsync(
        SysDept dept,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<int> GetUnaddedRolesCountAsync(
        SysDept dept,
        string filter = null,
        CancellationToken cancellationToken = default
    );

    public Task RemoveAllRolesAsync(SysDept dept);
    
    #endregion

    #region 用户相关

    Task<List<SysUser>> GetMembersAsync(
        SysDept dept,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<int> GetMembersCountAsync(
        SysDept dept,
        string filter = null,
        CancellationToken cancellationToken = default
    );

    Task<List<SysUser>> GetUnaddedUsersAsync(
        SysDept dept,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<int> GetUnaddedUsersCountAsync(
        SysDept dept,
        string filter = null,
        CancellationToken cancellationToken = default
    );

    Task RemoveAllMembersAsync(SysDept dept);

    #endregion
}