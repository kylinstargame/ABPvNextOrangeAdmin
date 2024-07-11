using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.CustomExtensions;
using ABPvNextOrangeAdmin.System.Roles;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using NotImplementedException = System.NotImplementedException;

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
            if (parentId != null && menuOutput != null && menuOutput.ParentId == parentId.Value)
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
        var menuTreeSelectOutputs = ObjectMapper.Map<List<SysMenu>, List<SysMenuTreeSelectOutput>>(menus);

        var menuTree = BuildMenuTree(menuTreeSelectOutputs, 0);
        return CommonResult<List<SysMenuTreeSelectOutput>>.Success(menuTree, "获取菜单树成功");
    }


    [HttpGet]
    [ActionName("list")]
    public async Task<CommonResult<List<SysMenuTreeSelectOutput>>> GetListAsync(MenuInput menuInput)
    {
        var menus = await MenuDomainService.GetMenuList();
        var menuTreeSelectOutputs = ObjectMapper.Map<List<SysMenu>, List<SysMenuTreeSelectOutput>>(menus);
        var menuTree = BuildMenuTree(menuTreeSelectOutputs, 0);
        return CommonResult<List<SysMenuTreeSelectOutput>>.Success(menuTree, "获取菜单列表成功");
    }
    
    [HttpGet]
    [ActionName("get")]
    
    public async Task<CommonResult<SysMenuOutput>> GetAsync(int id)
    {
        var menu = await MenuDomainService.FindById(id);
        var menuOutput = ObjectMapper.Map<SysMenu, SysMenuOutput>(menu);
        return CommonResult<SysMenuOutput>.Success(menuOutput, $"获取菜单{id}成功");
    }
    [HttpPost]
    [ActionName("add")]
    public async Task<CommonResult<string>> CreateAsync(SysMenuOutput input)
    {
        var menu = ObjectMapper.Map<SysMenuOutput, SysMenu>(input);
        await MenuDomainService.InsertAsync(menu);
        return CommonResult<String>.Success(null, "新增菜单成功");
    }

    [HttpPost]
    [ActionName("update")]
    public async Task<CommonResult<string>> UpdateAsync(SysMenuOutput input)
    {
        var menu = await MenuDomainService.FindById(input.Id);
        var newmenu = ObjectMapper.Map<SysMenuOutput, SysMenu>(input,menu);
        await MenuDomainService.UpdateAsync(newmenu);
        return CommonResult<String>.Success(null, "更新菜单成功");

    }
    [HttpPost]
    [ActionName("delete")]
    public async Task<CommonResult<string>> DeleteAsync(int id)
    {
        await MenuDomainService.DeleteAsync(id);
        return CommonResult<String>.Success(null, "删除菜单成功");
    }

    [HttpGet]
    [ActionName("treeSelectForRole")]
    public async Task<CommonResult<SysMenuTreeSelectForRoleOutput>> GetTreeSelectByRoleIdAsync(long roleId)
    {
        var menus = await MenuDomainService.GetMenuList();
        var menuTreeSelectOutputs = ObjectMapper.Map<List<SysMenu>, List<SysMenuTreeSelectOutput>>(menus);
        var menuTree = BuildMenuTree(menuTreeSelectOutputs, 0);
        List<long> roleMenuIds = await MenuDomainService.GetMenuListByRoleId(roleId);
        var menuTreeForRole = SysMenuTreeSelectForRoleOutput.CreateInstance(menuTree, roleMenuIds);
        return CommonResult<SysMenuTreeSelectForRoleOutput>.Success(menuTreeForRole, "获取菜单树成功");
    }
}