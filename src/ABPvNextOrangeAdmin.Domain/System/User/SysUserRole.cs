using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.Roles;

/// <summary>
/// 用户角色关联表
/// </summary>
public class SysUserRole : FullAuditedEntity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    public long RoleId { get; set; }


    public override object[] GetKeys()
    {
        return new object[] {UserId, RoleId};
    }

    private SysUserRole()
    {
    }

    public static List<SysUserRole> CreateInstances(long userId, int[] roleIds)
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
}