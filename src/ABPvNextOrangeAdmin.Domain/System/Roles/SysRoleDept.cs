using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.Organization;

public class SysRoleDept : FullAuditedEntity
{
     public long RoleId { get; set; }
 
     /// <summary>
     /// 菜单ID
     /// </summary>
     public long DeptId { get; set; }
 
     public override object[] GetKeys()
     {
         return new object[] { RoleId, DeptId };
     }
   
}