using System;
using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.Roles;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.User;

public class SysUserDept : FullAuditedEntity
{
    private readonly Guid? _tenantId;

    /// <summary>
    /// 用户ID
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    public long DeptId { get; set; }


    public override object[] GetKeys()
    {
        return new object[] { UserId, DeptId };
    }

    private SysUserDept()
    {
    }

    public SysUserDept(Guid userId, long deptId, Guid? tenantId)
    {
        _tenantId = tenantId;
        UserId = userId;
        DeptId = deptId;
    }

    public static List<SysUserDept> CreateInstances(Guid userId, int[] deptIds)
    {
        List<SysUserDept> userDepts = new List<SysUserDept>();
        foreach (var deptId in deptIds)
        {
            userDepts.Add(new SysUserDept()
            {
                DeptId = deptId,
                UserId = userId
            });
        }

        return userDepts;
    }
}