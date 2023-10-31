using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ABPvNextOrangeAdmin.Constans;
using ABPvNextOrangeAdmin.Utils;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.Menu;

public sealed class SysMenu : FullAuditedEntity<long>
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
    public int ParentId { get; set; }

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
    /// 描述信息
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 子菜单 
    /// </summary>
    public IEnumerable<SysMenu> Children { get; set; } = new List<SysMenu>();

    public SysMenu()
    {
    }

    public SysMenu(long id)
    {
        Id = id;
    }

    public SysMenu(long id, string menuName, int parentId, int orderNum, string path,
        string component, string query, string isFrame, string isCache, string menuType, string visible, string status,
        string perms, string icon, string remark)
    {
        Id = id;
        MenuName = menuName;
        ParentId = parentId;
        OrderNum = orderNum;
        Path = path;
        Component = component;
        Query = query;
        IsFrame = isFrame;
        IsCache = isCache;
        MenuType = menuType;
        Visible = visible;
        Status = status;
        Perms = perms;
        Icon = icon;
        Remark = remark;
    }

    public SysMenu(int id, string menuName, string path, string component, string query, string visible, string status,
        string perms, string isFrame, string isCache, int orderNum, string menuType, string icon, int parentId) : this()
    {
        Id = id;
        MenuName = menuName;
        Path = path;
        Component = component;
        Query = query;
        Visible = visible;
        Status = status;
        Perms = perms;
        IsFrame = isFrame;
        IsCache = isCache;
        OrderNum = orderNum;
        MenuType = menuType;
        Icon = icon;
        ParentId = parentId;
    }

    /**
     * 获取路由名称
     * 
     * @param menu 菜单信息
     * @return 路由名称
     */
    public String GetRouteName()
    {
        String routerName = StringUtils.Capitalize(Path);
        // 非外链并且是一级目录（类型为目录）
        if (IsMenuFrame())
        {
            routerName = StringUtils.EMPTY;
        }

        return routerName;
    }


    /**
     * 获取路由地址
     * 
     * @param menu 菜单信息
     * @return 路由地址
     */
    public String GetRouterPath()
    {
        String routerPath = Path;
        // 内链打开外网方式
        if (ParentId != 0 && IsInnerLink())
        {
            routerPath = InnerLinkReplaceEach(routerPath);
        }

        // 非外链并且是一级目录（类型为目录）
        if (0 == ParentId && UserConstants.TYPE_DIR.Equals(MenuType)
                          && UserConstants.NO_FRAME.Equals(IsFrame))
        {
            routerPath = "/" + Path;
        }
        // 非外链并且是一级目录（类型为菜单）
        else if (IsMenuFrame())
        {
            routerPath = "/";
        }

        return routerPath;
    }

    /**
     * 获取组件信息
     * 
     * @param menu 菜单信息
     * @return 组件信息
     */
    public String GetComponent()
    {
        String component = UserConstants.LAYOUT;
        if (StringUtils.IsNotEmpty(Component) && !IsMenuFrame())
        {
            component = Component;
        }
        else if (StringUtils.IsEmpty(Component) && ParentId != 0 && IsInnerLink())
        {
            component = UserConstants.INNER_LINK;
        }
        else if (StringUtils.IsEmpty(Component) && IsParentView())
        {
            component = UserConstants.PARENT_VIEW;
        }

        return component;
    }

    /**
     * 是否为parent_view组件
     * 
     * @param menu 菜单信息
     * @return 结果
     */
    public Boolean IsParentView()
    {
        return ParentId != 0 && UserConstants.TYPE_DIR.Equals(MenuType);
    }

    /**
     * 是否为菜单内部跳转
     * 
     * @param menu 菜单信息
     * @return 结果
     */
    public Boolean IsMenuFrame()
    {
        return Convert.ToInt32(ParentId) == 0 && UserConstants.TYPE_MENU.Equals(MenuType)
                                                && IsFrame.Equals(UserConstants.NO_FRAME);
    }

    /**
     * 是否为内链组件
     * 
     * @param menu 菜单信息
     * @return 结果
     */
    public Boolean IsInnerLink()
    {
        return IsFrame.Equals(UserConstants.NO_FRAME) && StringUtils.IsHTTP(Path);
    }

    /**
     * 内链域名特殊字符替换
     * 
     * @return
     */
    public String InnerLinkReplaceEach(String path)
    {
        return StringUtils.ReplaceEach(path, new String[] {CommonConstants.HTTP, CommonConstants.HTTPS},
            new String[] {"", ""});
    }
}