using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ABPvNextOrangeAdmin.System.Organization;

public class SysRoleDept : FullAuditedEntity, IMultiTenant
{
    public SysRoleDept(long roleId, long deptId, Guid? tenantId)
    {
        RoleId = roleId;
        DeptId = deptId;
        TenantId = tenantId;
    }

    public long RoleId { get; set; }
 
     /// <summary>
     /// 菜单ID
     /// </summary>
     public long DeptId { get; set; }
 
     public override object[] GetKeys()
     {
         return new object[] { RoleId, DeptId };
     }

     public Guid? TenantId { get; }
}