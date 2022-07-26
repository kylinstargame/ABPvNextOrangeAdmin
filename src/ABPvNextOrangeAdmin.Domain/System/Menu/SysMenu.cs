using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.Menu;

public class SysMenu : FullAuditedEntity<Guid>
{
    /// <summary>
    /// 菜单名称
    /// </summary>
    public String MenuName { get; set; }

    /// <summary>
    /// 父菜单名称 
    /// </summary>
    public String ParentName { get; set; }

    /// <summary>
    /// 父菜单ID 
    /// </summary>
    public Guid ParentId { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    public int OrderNum { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    public String Path { get; set; }

    /// <summary>
    /// 组件路径  
    /// </summary>
    public String Component { get; set; }

    /// <summary>
    /// 路由参数 
    /// </summary>
    public String Query { get; set; }

    /// <summary>
    /// 是否为外链（0是 1否）
    /// </summary>
    public string IsFrame { get; set; }

    /// <summary>
    /// 是否缓存（0缓存 1不缓存）
    /// </summary>
    public string IsCache { get; set; }

    /// <summary>
    /// 类型（M目录 C菜单 F按钮） 
    /// </summary>
    public string MenuType { get; set; }
    
    /// <summary>
    /// 显示状态（0显示 1隐藏）
    /// </summary>
    public string Visible { get; set; }
    
    /// <summary>
    /// 菜单状态（0显示 1隐藏）
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 权限字符串 
    /// </summary>
    public string Perms { get; set; }

    /// <summary>
    /// 菜单图标 
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 子菜单 
    /// </summary>
    public IEnumerable<SysMenu> Children { get; set; } = new List<SysMenu>();
}