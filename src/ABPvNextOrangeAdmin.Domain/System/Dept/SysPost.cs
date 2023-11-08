using System;
using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.User;
using Newtonsoft.Json;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ABPvNextOrangeAdmin.System.Organization;

/// <summary>
/// 系统岗位
/// </summary>
public class SysPost : FullAuditedEntity<long>,IMultiTenant
{
    public SysPost(Guid? tenantId)
    {
        TenantId = tenantId;
    }

    public SysPost(long id, string postCode, string postName) : base(id)
    {
        PostCode = postCode;
        PostName = postName;
    }

    /// <summary>
    /// 岗位编码 
    /// </summary>
    public string PostCode { get; set; }

    /// <summary>
    /// 岗位名称 
    /// </summary>
    public string PostName { get; set; }

    /// <summary>
    /// 岗位排序 
    /// </summary>
    public string Order { get; set; }

    /// <summary>
    /// 状态（0正常 1停用）
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 租户ID
    /// </summary>
    public Guid? TenantId { get; }

    public ICollection<SysUser> Users { get; set; }

    public override string ToString()
    {
       return JsonConvert.SerializeObject(this);
    }
}