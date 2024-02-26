using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System.Account.Dto;

public class RouteOutput : EntityDto<int>
{
    /// <summary>
    /// 路由名字
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 是否隐藏路由，当设置 true 的时候该路由不会再侧边栏出现
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// 重定向地址，当设置 noRedirect 的时候该路由在面包屑导航中不可被点击
    /// </summary>
    public string Redirect { get; set; }

    /// <summary>
    /// 组件地址
    /// </summary>
    public string Component { get; set; }

    /// <summary>
    /// 路由参数：如 {"id": 1, "name": "ry"}
    /// </summary>
    public string Query { get; set; }

    /// <summary>
    /// 当你一个路由下面的 children 声明的路由大于1个时，自动会变成嵌套的模式--如组件页面
    /// </summary>
    public bool AlwaysShow { get; set; }

    /// <summary>
    /// 其他元素
    /// </summary>
    public MetaVo Meta { get; set; }

    /// <summary>
    /// 子路由
    /// </summary>
    public List<RouteOutput> Children { get; set; }

    public RouteOutput(string component, MetaVo meta)
    {
        this.Component = component;
        Meta = meta;
    }

    public RouteOutput(MetaVo meta)
    {
        Meta = meta;
        throw new NotImplementedException();
    }

    public RouteOutput()
    {
    }
}

public class MetaVo
{
    public MetaVo(string menuName, string menuIcon)
    {
        Title = menuName;
        Icon = menuIcon;
    }
    
    public MetaVo(string menuName, string menuIcon,  string menuPath)
    {
        Title = menuName;
        Icon = menuIcon;
        Link = menuPath;
    }

    public MetaVo(string menuName, string menuIcon, bool isCache, string menuPath)
    {
        Title = menuName;
        Icon = menuIcon;
        NoCache = isCache;
        Link = menuPath;
    }

    /// <summary>
    ///  设置该路由在侧边栏和面包屑中展示的名字
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 设置该路由的图标，对应路径src/assets/icons/svg
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 设置为true，则不会被 <keep-alive>缓存
    /// </summary>
    public bool NoCache { get; set; }

    /// <summary>
    /// 内链地址（http(s)://开头）
    /// </summary>
    public string Link { get; set; }
}
