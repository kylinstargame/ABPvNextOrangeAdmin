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
    public SysUserOutput User { get; set; }

    /// <summary>
    /// 角色信息
    /// </summary>
    public String[] Roles { get; set; }


    /// <summary>
    /// 角色权限
    /// </summary>
    public String[] Permissions { get;}

    private UserWithRoleAndPermissionOutput(SysUserOutput user, string[] roles, string[] permissions)
    {
        User = user;
        Roles = roles;
        Permissions = permissions;
    }

    public static UserWithRoleAndPermissionOutput CreateInstance(SysUserOutput user, string[] roles, string[] permissions)
    {
        return new UserWithRoleAndPermissionOutput(user, roles, permissions);
    }
}