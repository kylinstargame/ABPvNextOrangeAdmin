using System;
using System.Collections.Generic;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace ABPvNextOrangeAdmin.System.Account.Dto;

public class UserWithRoleAndPermissionOutput
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public IdentityUserDto User { get; set; }

    /// <summary>
    /// 角色信息
    /// </summary>
    public String[] Roles { get; set; }


    /// <summary>
    /// 角色权限
    /// </summary>
    public String[] Permissions { get;}

    private UserWithRoleAndPermissionOutput(IdentityUserDto user, string[] roles, string[] permissions)
    {
        User = user;
        Roles = roles;
        Permissions = permissions;
    }

    public static UserWithRoleAndPermissionOutput CreateInstance(IdentityUserDto user, string[] roles, string[] permissions)
    {
        return new UserWithRoleAndPermissionOutput(user, roles, permissions);
    }
}