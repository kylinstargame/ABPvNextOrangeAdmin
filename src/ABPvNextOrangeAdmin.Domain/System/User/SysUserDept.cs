using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ABPvNextOrangeAdmin.System.Dept;
using ABPvNextOrangeAdmin.System.Roles;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.User;
[Table("sys_user_dept")]
public class SysUserDept : AuditedEntity
{
    private readonly Guid? _tenantId;



    public Guid UserId  { get; set; }
    
    
    public long DeptId  { get; set; }
    
    /// <summary>
    /// 用户ID
    /// </summary>
    public SysUser User { get; set; }
    /// <summary>
    ///部门ID
    /// </summary>a
    public SysDept Dept { get; set; }


    public override object[] GetKeys()
    {
        return new object[] { UserId, DeptId };
    }
    public ICollection<SysUser> Users { get;  set; }
  
    public SysUserDept(Guid userId, long deptId, Guid? tenantId)
    {
        _tenantId = tenantId;
        UserId = userId;
        DeptId = deptId;
    }
    public SysUserDept(Guid userId, long deptId)
    {
        UserId = userId;
        DeptId = deptId;
    }

    public static List<SysUserDept> CreateInstances(Guid userId, int[] deptIds)
    {
        List<SysUserDept> userDepts = new List<SysUserDept>();
        foreach (var deptId in deptIds)
        {
            userDepts.Add(new SysUserDept(userId,deptId));
        }

        return userDepts;
    }
}