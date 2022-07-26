using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;

namespace ABPvNextOrangeAdmin.System.Permission;

public class PermissionAppService
{
    protected IPermissionManager PermissionManager { get; set; }

    protected IPermissionDefinitionManager PermissionDefinitionManager { get; set; }

    protected ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; set; }

    public PermissionAppService(IPermissionManager permissionManager,
        IPermissionDefinitionManager permissionDefinitionManager,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager)
    {
        PermissionManager = permissionManager;
        PermissionDefinitionManager = permissionDefinitionManager;
        SimpleStateCheckerManager = simpleStateCheckerManager;
    }


    /// <summary>
    /// 获取用户角色
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private HashSet<String> GetRolePermission(IdentityUser user)
    {
        HashSet<String> roleNames = new HashSet<String>();

        List<IdentityUserRole> identityRoles = user.Roles.ToList();

        // foreach (var role in identityRoles)
        // {
        //     roleNames.Add(role.RoleId)
        // }

        List<IdentityUserOrganizationUnit> organizationUnits = user.OrganizationUnits.ToList();


        // 管理员拥有所有权限

        // roles.addAll(roleService.selectRolePermissionByUserId(user.getUserId()));
        return roleNames;
    }

    /// <summary>
    /// 获取菜单权限
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public HashSet<String> GetMenuPermission(IdentityUser user)
    {
        HashSet<String> perms = new HashSet<String>();
        // 管理员拥有所有权限
        // if (user.isAdmin())
        // {
        //     perms.add("*:*:*");
        // }
        // else
        // {
        //     perms.addAll(menuService.selectMenuPermsByUserId(user.getUserId()));
        // }
        return perms;
    }
}