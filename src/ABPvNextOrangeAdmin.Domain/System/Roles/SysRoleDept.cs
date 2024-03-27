using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ABPvNextOrangeAdmin.System.Organization;

public class SysRoleDept : FullAuditedEntity, IMultiTenant
{
    private SysRoleDept(long roleId, long deptId, Guid? tenantId)
    {
        RoleId = roleId;
        DeptId = deptId;
        TenantId = tenantId;
    }

    public static SysRoleDept CreateInstance(long roleId, long deptId, Guid? tenantId)
    {
        return new SysRoleDept(roleId, deptId, tenantId);
    }
    
    public static List<SysRoleDept> CreateInstances(long roleId, long[] deptIds, Guid? tenantId)
    {
        List<SysRoleDept> roleDepts = new List<SysRoleDept>();

        foreach (var deptId in deptIds)
        {
            roleDepts.Add(SysRoleDept.CreateInstance(roleId,deptId,tenantId));
        }
        return roleDepts;
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