using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Specifications;
using IdentityRole = Volo.Abp.Identity.IdentityRole;

namespace ABPvNextOrangeAdmin.System.Menu;

public class MenuDomainService : DomainService
{
    private readonly IRepository<SysMenu> _menuRepository;
    private readonly IRepository<SysRoleMenu> _roleMenuRepository;
    private readonly IRepository<IdentityRole> _roleRepository;
    private readonly IRepository<IdentityUserRole> _userRoleRepository;

    public MenuDomainService(IRepository<SysMenu> menuRepository, IRepository<SysRoleMenu> roleMenuRepository,
        IRepository<IdentityUserRole> userRoleRepository, IRepository<IdentityRole> roleRepository)
    {
        _menuRepository = menuRepository;
        _roleMenuRepository = roleMenuRepository;
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
    }

    /// <summary>
    /// 根据用户查询系统菜单列表
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<SysMenu>> GetMenuList(Guid userId)
    {
        return await GetMenuList(userId, new SysMenu());
    }

    /// <summary>
    /// 查询系统菜单列表
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="menu"></param>
    /// <returns></returns>
    public async Task<List<SysMenu>> GetMenuList(Guid userId, SysMenu menu)
    {
        var menus = await _menuRepository.WithDetailsAsync();
        var roleMenus = await _roleMenuRepository.WithDetailsAsync();
        var userRoles = await _userRoleRepository.WithDetailsAsync();
        var roles = await _roleRepository.WithDetailsAsync();
        var result = menus.Join(roleMenus, menu => menu.Id, roleMenu => roleMenu.MenuId,
                (menu, roleMenu) => new { menu = menu, roleMenu })
            .Join(userRoles, x => x.roleMenu.RoleId, userRole => userRole.RoleId,
                (x, userRole) => new { x.menu, x.roleMenu, userRole })
            .Join(roles, x => x.userRole.RoleId, role => role.Id,
                (x, role) => new { x.menu, x.roleMenu, x.userRole, role })
            .Where(x => x.userRole.UserId == userId)
            .Select(x => new SysMenu
            {
                MenuName = x.menu.MenuName,
                Path = x.menu.Path,
                Component = x.menu.Component,
                Query = x.menu.Query,
                Visible = x.menu.Visible,
                Status = x.menu.Status,
                Perms = x.menu.Perms ?? "",
                IsFrame = x.menu.IsFrame,
                IsCache = x.menu.IsCache,
                OrderNum = x.menu.OrderNum,
                MenuType = x.menu.MenuType,
            })
            .Distinct()
            .WhereIf(!String.IsNullOrEmpty(menu.MenuName), x => x.MenuName.Contains(menu.MenuName))
            .WhereIf(!String.IsNullOrEmpty(menu.Visible), x => x.Visible == menu.Visible)
            .WhereIf(!String.IsNullOrEmpty(menu.Status), x => x.Status == menu.Status)
            .OrderBy(x => x.ParentId)
            .ThenBy(x => x.OrderNum)
            .ToList();
        return result;
    }

    /// <summary>
    /// 根据用户ID查询权限
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<String>> GetMenuPermissionsByUserId(Guid userId)
    {
        var menus = await _menuRepository.WithDetailsAsync();
        var roleMenus = await _roleMenuRepository.WithDetailsAsync();
        var userRoles = await _userRoleRepository.WithDetailsAsync();
        var roles = await _roleRepository.WithDetailsAsync();
        var result = menus.Join(roleMenus, menu => menu.Id, roleMenu => roleMenu.MenuId,
                (menu, roleMenu) => new { menu, roleMenu })
            .Join(userRoles, x => x.roleMenu.RoleId, userRole => userRole.RoleId,
                (x, userRole) => new { x.menu, x.roleMenu, userRole })
            .Join(roles, x => x.userRole.RoleId, role => role.Id,
                (x, role) => new { x.menu, x.roleMenu, x.userRole, role })
            .Where(x => x.menu.Status == "0")
            .Where(x => x.role.IsPublic)
            .Where(x => x.userRole.RoleId == userId)
            .Where(x => x.menu.Status == "0")
            .Select(x => new String(x.menu.Perms ?? ""))
            .ToList();
        return result;
    }

    /// <summary>
    /// 根据用户ID查询菜单树信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<SysMenu>> GetMenuTreeByUserId(Guid userId)
    {
        var menus = await _menuRepository.WithDetailsAsync();
        var roleMenus = await _roleMenuRepository.WithDetailsAsync();
        var userRoles = await _userRoleRepository.WithDetailsAsync();
        var roles = await _roleRepository.WithDetailsAsync();
        var result = menus.Join(roleMenus, menu => menu.Id, roleMenu => roleMenu.MenuId,
                (menu, roleMenu) => new { menu = menu, roleMenu })
            .Join(userRoles, x => x.roleMenu.RoleId, userRole => userRole.RoleId,
                (x, userRole) => new { x.menu, x.roleMenu, userRole })
            .Join(roles, x => x.userRole.RoleId, role => role.Id,
                (x, role) => new { x.menu, x.roleMenu, x.userRole, role })
            .Where(x => x.userRole.UserId == userId)
            .Select(x => new SysMenu
            {
                MenuName = x.menu.MenuName,
                Path = x.menu.Path,
                Component = x.menu.Component,
                Query = x.menu.Query,
                Visible = x.menu.Visible,
                Status = x.menu.Status,
                Perms = x.menu.Perms ?? "",
                IsFrame = x.menu.IsFrame,
                IsCache = x.menu.IsCache,
                OrderNum = x.menu.OrderNum,
                MenuType = x.menu.MenuType,
            })
            .Distinct()
            .Where(x => new String[] { "M", "C" }.Contains(x.MenuType))
            .ToList();
        result.OrderBy(x => x.ParentId)
            .ThenBy(x => x.OrderNum);
        // var result = menus.Join(roleMenus, menu => menu.Id, roleMenu => roleMenu.MenuId,
        //         (menu, roleMenu) => new { menu = menu, roleMenu })
        //     .Join(userRoles, x => x.roleMenu.RoleId, userRole => userRole.RoleId,
        //         (x, userRole) => new { x.menu, x.roleMenu, userRole })
        //     .Join(roles, x => x.userRole.RoleId, role => role.Id,
        //         (x, role) => new { x.menu, x.roleMenu, x.userRole, role })
        //     .Where(x => x.userRole.UserId == userId)
        //     .Select(x => new SysMenu
        //     {
        //         MenuName = x.menu.MenuName,
        //         Path = x.menu.Path,
        //         Component = x.menu.Component,
        //         Query = x.menu.Query,
        //         Visible = x.menu.Visible,
        //         Status = x.menu.Status,
        //         Perms = x.menu.Perms ?? "",
        //         IsFrame = x.menu.IsFrame,
        //         IsCache = x.menu.IsCache,
        //         OrderNum = x.menu.OrderNum,
        //         MenuType = x.menu.MenuType,
        //     })
        //     .Distinct()
        //     .Where(x => new String[] { "M", "C" }.Contains(x.MenuType))
        //     .OrderBy(x => x.ParentId)
        //     .ThenBy(x => x.OrderNum)
        //     .ToList();

        return GetChildPerms(result, 0);
    }

    /// <summary>
    /// 根据用户ID查询菜单列表信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<int>> GetMenuListByRoleId(Guid roleId)
    {
        IdentityRole usrRole = await _roleRepository.GetAsync(x => x.Id == roleId);

        var menus = await _menuRepository.WithDetailsAsync();
        var roleMenus = await _roleMenuRepository.WithDetailsAsync();
        var result = menus.Join(roleMenus, menu => menu.Id, roleMenu => roleMenu.MenuId,
                (menu, roleMenu) => new { menu = menu, roleMenu })
            .Where(x => x.roleMenu.RoleId == roleId).Select(x => x.menu.Id).ToList();
        return result;
    }

    /// <summary>
    /// 据父节点的ID获取所有子节点
    /// </summary>
    /// <param name="menus"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public List<SysMenu> GetChildPerms(List<SysMenu> menus, int parentId)
    {
        List<SysMenu> returnList = new List<SysMenu>();
        IEnumerator enumerator = returnList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            SysMenu menu = (SysMenu)enumerator.Current;
            // 一、根据传入的某个父节点ID,遍历该父节点的所有子节点
            if (Equals(menu.ParentId, parentId.ToString()))
            {
                RecursionFn(menus, menu);
                returnList.Add(menu);
            }
        }

        return returnList;
    }

    public List<SysMenu> BuildMenuTree(List<SysMenu> menus)
    {
        List<SysMenu> returnList = new List<SysMenu>();
        List<int> tempList = new List<int>();
        foreach (SysMenu menu in menus)
        {
            tempList.Add(menu.Id);
        }

        using (var enumerator = menus.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                SysMenu menu = (SysMenu)enumerator.Current;
                // 如果是顶级节点, 遍历该父节点的所有子节点
                if (!tempList.Contains(menu.ParentId))
                {
                    RecursionFn(menus, menu);
                    returnList.Add(menu);
                }
            }
        }

        if (returnList.Count == 0)
        {
            returnList = menus;
        }

        return returnList;
    }

    /// <summary>
    /// 递归列表
    /// </summary>
    /// <param name="menus"></param>
    /// <param name="menu"></param>
    private void RecursionFn(List<SysMenu> menus, SysMenu menu)
    {
        // 得到子节点列表
        List<SysMenu> childMenus = GetChildList(menus, menu);
        menu.Children = childMenus;
        foreach (SysMenu tChild in childMenus)
        {
            if (HasChild(menus, tChild))
            {
                RecursionFn(menus, tChild);
            }
        }
    }

    /// <summary>
    /// 得到子节点列表
    /// </summary>
    /// <param name="menus"></param>
    /// <param name="menu"></param>
    /// <returns></returns>
    private List<SysMenu> GetChildList(List<SysMenu> menus, SysMenu menu)
    {
        List<SysMenu> resultMenus = new List<SysMenu>();
        IEnumerator it = menus.GetEnumerator();
        while (it.MoveNext())
        {
            SysMenu current = (SysMenu)it.Current;
            if (current != null && current.ParentId == menu.ParentId)
            {
                resultMenus.Add(current);
            }
        }

        return resultMenus;
    }

    /// <summary>
    /// 判断是否有子节点
    /// </summary>
    /// <param name="menus"></param>
    /// <param name="menu"></param>
    /// <returns></returns>
    private bool HasChild(List<SysMenu> menus, SysMenu menu)
    {
        return GetChildList(menus, menu).Count() > 0;
    }
}