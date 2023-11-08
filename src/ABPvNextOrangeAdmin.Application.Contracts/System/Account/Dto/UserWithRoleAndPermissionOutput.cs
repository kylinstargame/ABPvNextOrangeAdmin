using System;
using System.Collections.Generic;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace ABPvNextOrangeAdmin.System.Account.Dto;

public class UserWithRoleAndPermissionOutput
{
    public UserWithRoleAndPermissionOutput()
    {
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public SysUserOutput User { get; set; }

    /// <summary>
    /// 角色信息
    /// </summary>
    public long[] Roles { get; set; }


    /// <summary>
    /// 角色权限
    /// </summary>
    public String[] Permissions { get;}

    private UserWithRoleAndPermissionOutput(SysUserOutput user,long[] roles, string[] permissions)
    {
        User = user;
        Roles = roles;
        Permissions = permissions;
    }

    public static UserWithRoleAndPermissionOutput CreateInstance(SysUserOutput user, long[] roles, string[] permissions)
    {
        return new UserWithRoleAndPermissionOutput(user, roles, permissions);
    }
}