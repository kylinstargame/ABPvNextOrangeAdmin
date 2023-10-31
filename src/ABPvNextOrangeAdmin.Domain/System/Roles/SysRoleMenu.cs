using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.Roles;

public class SysRoleMenu : FullAuditedEntity, IEntity<int>
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 菜单ID
    /// </summary>
    public long MenuId { get; set; }

    public override object[] GetKeys()
    {
        return new object[] { RoleId, MenuId };
    }

    private SysRoleMenu()
    {
    }

    public static List<SysRoleMenu> CreateInstances(long roleId, int[] menuIds)
    {
        List<SysRoleMenu> roleMenus = new List<SysRoleMenu>();
        foreach (var menuId in menuIds)
        {
            roleMenus.Add(new SysRoleMenu()
            {
                RoleId = roleId,
                MenuId = menuId
            });
        }

        return roleMenus;
    }

    public int Id { get; }
}