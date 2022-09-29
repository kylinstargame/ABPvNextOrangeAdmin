using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ABPvNextOrangeAdmin.System.Organization;

/// <summary>
/// 系统部门
/// </summary>
public class SysDept : FullAuditedAggregateRoot<long>, IMultiTenant
{
    /// <summary>
    /// 父部门ID
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 祖级列表 如 000.001.005
    /// </summary>
    public string Ancestors { get; set; }

    /// <summary>
    /// 部门名称 
    /// </summary>
    public string DeptName { get; set; }

    /// <summary>
    /// 显示顺序  
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 负责人姓名 
    /// </summary>
    public string Leader { get; set; }

    /// <summary>
    /// 联系电话 
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// 邮箱账号
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 部门状态:0正常,1停用 
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 租户ID
    /// </summary>
    public Guid? TenantId { get; }
}