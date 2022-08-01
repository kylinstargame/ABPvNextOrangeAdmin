using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.Menu;

public class MenuDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private IRepository<SysMenu> _menuRepository;

    public MenuDataSeedContributor(IRepository<SysMenu> menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        List<SysMenu> menus = new List<SysMenu>();
        // "menu_id", "menu_name", "parent_id", "order_num", "path", "component", "query", "is_frame", "is_cache", "menu_type", "visible", "status", "perms", "icon", "create_by", "create_time", "update_by", "update_time", "remark"

        #region 系统管理

        menus.Add(new SysMenu(1, "系统管理", 0, 1, "system", "", "", "1", "0", "M", "0", "0", "", "system", "系统管理目录"));
        menus.Add(new SysMenu(2, "系统监控", 0, 2, "monitor", "","", "1", "0", "M", "0", "0", "", "monitor", "系统监控目录"));
        menus.Add(new SysMenu(3, "系统工具", 0, 3, "tool", "","", "1", "0", "M", "0", "0", "", "tool",  "系统工具目录"));
        menus.Add(new SysMenu(4, "橙卡官网", 0, 4, "http://ruoyi.vip", "","", "0", "0", "M", "0", "0", "", "guide", "橙卡官网地址"));

        #endregion

        menus.Add(new SysMenu(100, "用户管理", 1, 1, "user", "system/user/index", "", "1", "0", "C", "0", "0",
            "system:user:list", "user", "用户管理菜单"));
        menus.Add(new SysMenu(101, "角色管理", 1, 2, "role", "system/role/index", "", "1", "0", "C", "0", "0",
            "system:role:list", "peoples", "角色管理菜单"));
        menus.Add(new SysMenu(102, "菜单管理", 1, 3, "menu", "system/menu/index", "", "1", "0", "C", "0", "0",
            "system:menu:list", "tree-table", "菜单管理菜单"));
        menus.Add(new SysMenu(103, "部门管理", 1, 4, "dept", "system/dept/index", "", "1", "0", "C", "0", "0",
            "system:dept:list", "tree", "部门管理菜单"));
        menus.Add(new SysMenu(104, "岗位管理", 1, 5, "post", "system/post/index", "", "1", "0", "C", "0", "0",
            "system:post:list", "post", "岗位管理菜单"));
        menus.Add(new SysMenu(105, "字典管理", 1, 6, "dict", "system/dict/index", "", "1", "0", "C", "0", "0",
            "system:dict:list", "dict", "字典管理菜单"));
        menus.Add(new SysMenu(106, "参数设置", 1, 7, "config", "system/config/index", "", "1", "0", "C", "0", "0",
            "system:config:list", "edit", "参数设置菜单"));
        menus.Add(new SysMenu(107, "通知公告", 1, 8, "notice", "system/notice/index", "", "1", "0", "C", "0", "0",
            "system:notice:list", "message", "通知公告菜单"));
        menus.Add(new SysMenu(108, "日志管理", 1, 9, "log", "", "", "1", "0", "M", "0", "0", "", "log", "日志管理菜单"));
        menus.Add(new SysMenu(109, "在线用户", 2, 1, "online", "monitor/online/index", "", "1", "0", "C", "0", "0",
            "monitor:online:list", "online", "在线用户菜单"));
        menus.Add(new SysMenu(110, "定时任务", 2, 2, "job", "monitor/job/index", "", "1", "0", "C", "0", "0",
            "monitor:job:list", "job", "定时任务菜单"));

        #region 系统监控

        menus.Add(new SysMenu(111, "数据监控", 2, 3, "druid", "monitor/druid/index", "", "1", "0", "C", "0", "0",
            "monitor:druid:list", "druid", "数据监控菜单"));
        menus.Add(new SysMenu(112, "服务监控", 2, 4, "server", "monitor/server/index", "", "1", "0", "C", "0", "0",
            "monitor:server:list", "server", "服务监控菜单"));
        menus.Add(new SysMenu(113, "缓存监控", 2, 5, "cache", "monitor/cache/index", "", "1", "0", "C", "0", "0",
            "monitor:cache:list", "redis", "缓存监控菜单"));
        menus.Add(new SysMenu(114, "表单构建", 3, 1, "build", "tool/build/index", "", "1", "0", "C", "0", "0",
            "tool:build:list", "build", "表单构建菜单"));

        #endregion

        #region 代码生成

        menus.Add(new SysMenu(115, "代码生成", 3, 2, "gen", "tool/gen/index", "", "1", "0", "C", "0", "0", "tool:gen:list",
            "code", "代码生成菜单"));
        menus.Add(new SysMenu(116, "系统接口", 3, 3, "swagger", "tool/swagger/index", "", "1", "0", "C", "0", "0",
            "tool:swagger:list", "swagger", "系统接口菜单"));

        #endregion

        #region 系统日志

        menus.Add(new SysMenu(500, "操作日志", 108, 1, "operlog", "monitor/operlog/index", "", "1", "0", "C", "0", "0",
            "monitor:operlog:list", "form", "操作日志菜单"));
        menus.Add(new SysMenu(501, "登录日志", 108, 2, "logininfor", "monitor/logininfor/index", "", "1", "0", "C", "0",
            "0", "monitor:logininfor:list", "logininfor", "登录日志菜单"));

        #endregion

        #region 角色用户

        menus.Add(new SysMenu(1000, "用户查询", 100, 1, "", "", "", "1", "0", "F", "0", "0", "system:user:query", "#", ""));
        menus.Add(new SysMenu(1001, "用户新增", 100, 2, "", "", "", "1", "0", "F", "0", "0", "system:user:add", "#", ""));
        menus.Add(new SysMenu(1002, "用户修改", 100, 3, "", "", "", "1", "0", "F", "0", "0", "system:user:edit", "#", ""));
        menus.Add(new SysMenu(1003, "用户删除", 100, 4, "", "", "", "1", "0", "F", "0", "0", "system:user:remove", "#",
            ""));
        menus.Add(new SysMenu(1004, "用户导出", 100, 5, "", "", "", "1", "0", "F", "0", "0", "system:user:export", "#",
            ""));
        menus.Add(new SysMenu(1005, "用户导入", 100, 6, "", "", "", "1", "0", "F", "0", "0", "system:user:import", "#",
            ""));
        menus.Add(new SysMenu(1006, "重置密码", 100, 7, "", "", "", "1", "0", "F", "0", "0", "system:user:resetPwd", "#",
            ""));
        menus.Add(new SysMenu(1007, "角色查询", 101, 1, "", "", "", "1", "0", "F", "0", "0", "system:role:query", "#", ""));
        menus.Add(new SysMenu(1008, "角色新增", 101, 2, "", "", "", "1", "0", "F", "0", "0", "system:role:add", "#", ""));
        menus.Add(new SysMenu(1009, "角色修改", 101, 3, "", "", "", "1", "0", "F", "0", "0", "system:role:edit", "#", ""));
        menus.Add(new SysMenu(1010, "角色删除", 101, 4, "", "", "", "1", "0", "F", "0", "0", "system:role:remove", "#",
            ""));
        menus.Add(new SysMenu(1011, "角色导出", 101, 5, "", "", "", "1", "0", "F", "0", "0", "system:role:export", "#",
            ""));

        #endregion

        #region 菜单管理

        menus.Add(new SysMenu(1012, "菜单查询", 102, 1, "", "", "", "1", "0", "F", "0", "0", "system:menu:query", "#", ""));
        menus.Add(new SysMenu(1013, "菜单新增", 102, 2, "", "", "", "1", "0", "F", "0", "0", "system:menu:add", "#", ""));
        menus.Add(new SysMenu(1014, "菜单修改", 102, 3, "", "", "", "1", "0", "F", "0", "0", "system:menu:edit", "#", ""));
        menus.Add(new SysMenu(1015, "菜单删除", 102, 4, "", "", "", "1", "0", "F", "0", "0", "system:menu:remove", "#",
            ""));

        #endregion

        #region 部门岗位

        menus.Add(new SysMenu(1016, "部门查询", 103, 1, "", "", "", "1", "0", "F", "0", "0", "system:dept:query", "#", ""));
        menus.Add(new SysMenu(1017, "部门新增", 103, 2, "", "", "", "1", "0", "F", "0", "0", "system:dept:add", "#", ""));
        menus.Add(new SysMenu(1018, "部门修改", 103, 3, "", "", "", "1", "0", "F", "0", "0", "system:dept:edit", "#", ""));
        menus.Add(
            new SysMenu(1019, "部门删除", 103, 4, "", "", "", "1", "0", "F", "0", "0", "system:dept:remove", "", ""));
        menus.Add(new SysMenu(1020, "岗位查询", 104, 1, "", "", "", "1", "0", "F", "0", "0", "system:post:query", "#", ""));
        menus.Add(new SysMenu(1021, "岗位新增", 104, 2, "", "", "", "1", "0", "F", "0", "0", "system:post:add", "#", ""));
        menus.Add(new SysMenu(1022, "岗位修改", 104, 3, "", "", "", "1", "0", "F", "0", "0", "system:post:edit", "#", ""));
        menus.Add(new SysMenu(1023, "岗位删除", 104, 4, "", "", "", "1", "0", "F", "0", "0", "system:post:remove", "#",
            ""));
        menus.Add(new SysMenu(1024, "岗位导出", 104, 5, "", "", "", "1", "0", "F", "0", "0", "system:post:export", "#",
            ""));

        #endregion

        #region 字典

        menus.Add(new SysMenu(1025, "字典查询", 105, 1, "#", "", "", "1", "0", "F", "0", "0", "system:dict:query", "#",
            ""));
        menus.Add(new SysMenu(1026, "字典新增", 105, 2, "#", "", "", "1", "0", "F", "0", "0", "system:dict:add", "#", ""));
        menus.Add(new SysMenu(1027, "字典修改", 105, 3, "#", "", "", "1", "0", "F", "0", "0", "system:dict:edit", "#", ""));
        menus.Add(
            new SysMenu(1028, "字典删除", 105, 4, "#", "", "", "1", "0", "F", "0", "0", "system:dict:remove", "#", ""));
        menus.Add(
            new SysMenu(1029, "字典导出", 105, 5, "#", "", "", "1", "0", "F", "0", "0", "system:dict:export", "#", ""));

        #endregion

        #region 系统参数

        menus.Add(new SysMenu(1030, "参数查询", 106, 1, "#", "", "", "1", "0", "F", "0", "0", "system:config:query", "#",
            ""));
        menus.Add(new SysMenu(1031, "参数新增", 106, 2, "#", "", "", "1", "0", "F", "0", "0", "system:config:add", "#",
            ""));
        menus.Add(
            new SysMenu(1032, "参数修改", 106, 3, "#", "", "", "1", "0", "F", "0", "0", "system:config:edit", "#", ""));
        menus.Add(new SysMenu(1033, "参数删除", 106, 4, "#", "", "", "1", "0", "F", "0", "0", "system:config:remove", "#",
            ""));
        menus.Add(new SysMenu(1034, "参数导出", 106, 5, "#", "", "", "1", "0", "F", "0", "0", "system:config:export", "#",
            ""));

        #endregion

        #region 公告

        menus.Add(new SysMenu(1035, "公告查询", 107, 1, "#", "", "", "1", "0", "F", "0", "0", "system:notice:query", "#",
            ""));
        menus.Add(new SysMenu(1036, "公告新增", 107, 2, "#", "", "", "1", "0", "F", "0", "0", "system:notice:add", "#",
            ""));
        menus.Add(
            new SysMenu(1037, "公告修改", 107, 3, "#", "", "", "1", "0", "F", "0", "0", "system:notice:edit", "#", ""));
        menus.Add(new SysMenu(1038, "公告删除", 107, 4, "#", "", "", "1", "0", "F", "0", "0", "system:notice:remove", "#",
            ""));

        #endregion

        #region 日志

        menus.Add(new SysMenu(1039, "操作查询", 500, 1, "#", "", "", "1", "0", "F", "0", "0", "monitor:operlog:query", "#",
            ""));
        menus.Add(new SysMenu(1040, "操作删除", 500, 2, "#", "", "", "1", "0", "F", "0", "0", "monitor:operlog:remove", "#",
            ""));
        menus.Add(new SysMenu(1041, "日志导出", 500, 4, "#", "", "", "1", "0", "F", "0", "0", "monitor:operlog:export", "#",
            ""));
        menus.Add(new SysMenu(1042, "登录查询", 501, 1, "#", "", "", "1", "0", "F", "0", "0", "monitor:logininfor:query",
            "#", ""));
        menus.Add(new SysMenu(1043, "登录删除", 501, 2, "#", "", "", "1", "0", "F", "0", "0", "monitor:logininfor:remove",
            "#", ""));
        menus.Add(new SysMenu(1044, "日志导出", 501, 3, "#", "", "", "1", "0", "F", "0", "0", "monitor:logininfor:export",
            "#", ""));

        #endregion

        #region 后台任务

        menus.Add(new SysMenu(1045, "在线查询", 109, 1, "#", "", "", "1", "0", "F", "0", "0", "monitor:online:query", "#",
            ""));
        menus.Add(new SysMenu(1046, "批量强退", 109, 2, "#", "", "", "1", "0", "F", "0", "0", "monitor:online:batchLogout",
            "#", ""));
        menus.Add(new SysMenu(1047, "单条强退", 109, 3, "#", "", "", "1", "0", "F", "0", "0", "monitor:online:forceLogout",
            "#", ""));
        menus.Add(new SysMenu(1048, "任务查询", 110, 1, "#", "", "", "1", "0", "F", "0", "0", "monitor:job:query", "#",
            ""));
        menus.Add(new SysMenu(1049, "任务新增", 110, 2, "#", "", "", "1", "0", "F", "0", "0", "monitor:job:add", "#", ""));
        menus.Add(new SysMenu(1050, "任务修改", 110, 3, "#", "", "", "1", "0", "F", "0", "0", "monitor:job:edit", "#", ""));
        menus.Add(
            new SysMenu(1051, "任务删除", 110, 4, "#", "", "", "1", "0", "F", "0", "0", "monitor:job:remove", "#", ""));
        menus.Add(new SysMenu(1052, "状态修改", 110, 5, "#", "", "", "1", "0", "F", "0", "0", "monitor:job:changeStatus",
            "#", ""));
        menus.Add(
            new SysMenu(1053, "任务导出", 110, 7, "#", "", "", "1", "0", "F", "0", "0", "monitor:job:export", "#", ""));

        #endregion

        #region 代码生成

        menus.Add(new SysMenu(1054, "生成查询", 115, 1, "#", "", "", "1", "0", "F", "0", "0", "tool:gen:query", "#", ""));
        menus.Add(new SysMenu(1055, "生成修改", 115, 2, "#", "", "", "1", "0", "F", "0", "0", "tool:gen:edit", "#", ""));
        menus.Add(new SysMenu(1056, "生成删除", 115, 3, "#", "", "", "1", "0", "F", "0", "0", "tool:gen:remove", "#", ""));
        menus.Add(new SysMenu(1057, "导入代码", 115, 2, "#", "", "", "1", "0", "F", "0", "0", "tool:gen:import", "#", ""));
        menus.Add(new SysMenu(1058, "预览代码", 115, 4, "#", "", "", "1", "0", "F", "0", "0", "tool:gen:preview", "#", ""));
        menus.Add(new SysMenu(1059, "生成代码", 115, 5, "#", "", "", "1", "0", "F", "0", "0", "tool:gen:code", "#", ""));

        #endregion

        await _menuRepository.InsertManyAsync(menus);
    }
}