using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace ABPvNextOrangeAdmin.System.Roles;

/// <summary>
/// 系统角色
/// </summary>
public class SysRole : FullAuditedAggregateRoot<long>, IMultiTenant
{
    public SysRole(string roleName, Guid? tenantId = null)
    {
        RoleName = roleName;
        TenantId = tenantId;
    }

    /// <summary>
    /// 角色名称
    /// </summary>
    public String RoleName { get; set; }

    /// <summary>
    /// 角色权限字符
    /// </summary>
    public String Permissions;
    
    public virtual bool IsDefault { get; set; }

    public SysRole(long roleName, Guid roleId, Guid? tenantId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 角色排序
    /// </summary>
    public String Order { get; set; }

    /// <summary>
    /// 数据范围（1：所有数据权限；2：自定义数据权限；3：本部门数据权限；4：本部门及以下数据权限；5：仅本人数据权限）
    /// </summary>
    public String DataScope { get; set; }

    /// <summary>
    /// 菜单树选择项是否关联显示（ 0：父子不互相关联显示 1：父子互相关联显示）
    /// </summary>
    public bool MenuCheckStrictly { get; set; }

    /// <summary>
    /// 部门树选择项是否关联显示（0：父子不互相关联显示 1：父子互相关联显示 ） 
    /// </summary>
    public bool DeptCheckStrictly { get; set; }

    /// <summary>
    /// 角色状态（0正常 1停用）
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 租户标识
    /// </summary>
    public Guid? TenantId { get; }
}