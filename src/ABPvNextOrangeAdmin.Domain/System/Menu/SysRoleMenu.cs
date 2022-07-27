using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.Menu;

public class SysRoleMenu : FullAuditedEntity
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// 菜单ID
    /// </summary>
    public int MenuId { get; set; }

    public override object[] GetKeys()
    {
        return new object[]{ RoleId, MenuId};
    }
}