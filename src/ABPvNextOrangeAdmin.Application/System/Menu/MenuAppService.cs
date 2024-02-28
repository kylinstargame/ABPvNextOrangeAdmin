using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.CustomExtensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;

namespace ABPvNextOrangeAdmin.System.Menu;

[Authorize]
[Route("api/sys/menu/[action]")]
public class MenuAppService : ApplicationService, IMenuAppService
{
    public MenuAppService(MenuDomainService menuDomainService)
    {
        MenuDomainService = menuDomainService;
    }

    private MenuDomainService MenuDomainService { get; }
    
    private List<SysMenuTreeSelectOutput> BuildMenuTree(List<SysMenuTreeSelectOutput> menuOutputs, long? parentId)
    {
        List<SysMenuTreeSelectOutput> returnMenuOutputs = new List<SysMenuTreeSelectOutput>();
        foreach (var menuOutput in menuOutputs)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (menuOutput != null && menuOutput.ParentId==parentId.Value)
            {
                RecursionFn(menuOutputs, menuOutput);
                returnMenuOutputs.Add(menuOutput);
            }
        }

        return returnMenuOutputs;
    }

    private void RecursionFn(List<SysMenuTreeSelectOutput> depts, SysMenuTreeSelectOutput parent)
    {
        // 得到子节点列表
        List<SysMenuTreeSelectOutput> childDepts = GetChildList(depts, parent);
        parent.Children = childDepts;
        foreach (SysMenuTreeSelectOutput tChild in childDepts)
        {
            if (HasChild(depts, tChild))
            {
                RecursionFn(depts, tChild);
            }
        }
    }

    private bool HasChild(List<SysMenuTreeSelectOutput> depts, SysMenuOutput sysDept)
    {
        return GetChildList(depts, sysDept).Count > 0;
    }

    private List<SysMenuTreeSelectOutput> GetChildList(List<SysMenuTreeSelectOutput> menus, SysMenuOutput sysMenu)
    {
        List<SysMenuTreeSelectOutput> resultMenus = new List<SysMenuTreeSelectOutput>();
        IEnumerator it = menus.GetEnumerator();
        while (it.MoveNext())
        {
            SysMenuTreeSelectOutput current = (SysMenuTreeSelectOutput)it.Current;
            if (current != null && current.ParentId == sysMenu.Id)
            {
                resultMenus.Add(current);
            }
        }

        return resultMenus;
    }

    [HttpGet]
    [ActionName("treeselect")]
    public async Task<CommonResult<List<SysMenuTreeSelectOutput>>> GetTreeSelectAsync()
    {
        var menus = await MenuDomainService.GetMenuList();
        var menuTreeSelectOutputs=ObjectMapper.Map<List<SysMenu>, List<SysMenuTreeSelectOutput>>(menus);
        var menuTree = BuildMenuTree(menuTreeSelectOutputs,0);
        return CommonResult<List<SysMenuTreeSelectOutput>>.Success(menuTree,"获取菜单树成功");
    }
}