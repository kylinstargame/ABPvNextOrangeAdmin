using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.Roles;

/// <summary>
/// 用户角色关联表
/// </summary>
public class SysUserRole : FullAuditedEntity
{
    private readonly Guid? _tenantId;

    /// <summary>
    /// 用户ID
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    public long RoleId { get; set; }


    public override object[] GetKeys()
    {
        return new object[] { UserId, RoleId };
    }

    private SysUserRole()
    {
    }

    public SysUserRole(Guid userId, long roleId, Guid? tenantId)
    {
        _tenantId = tenantId;
        UserId = userId;
        RoleId = roleId;
    }

    public static List<SysUserRole> CreateInstances(Guid userId, long[] roleIds)
    {
        List<SysUserRole> userRoles = new List<SysUserRole>();
        foreach (var roleId in roleIds)
        {
            userRoles.Add(new SysUserRole()
            {
                RoleId = roleId,
                UserId = userId
            });
        }

        return userRoles;
    }

    public static List<SysUserRole> CreateInstances(String[] userIds, long roleId)
    {
        List<SysUserRole> userRoles = new List<SysUserRole>();
        foreach (var userId in userIds)
        {
            userRoles.Add(new SysUserRole()
            {
                RoleId = roleId,
                UserId = Guid.Parse(userId)
            });
        }

        return userRoles;
    }
}