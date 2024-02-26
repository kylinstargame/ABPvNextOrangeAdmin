using System;
using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Organization.Dto;
using SysRoleOutput = ABPvNextOrangeAdmin.System.Permission.Dto.SysRoleOutput;

namespace ABPvNextOrangeAdmin.System.User.Dto;

public class SysUserOutputWithRoleAndPosts
{
    private SysUserOutputWithRoleAndPosts(SysUserOutput userOutput, List<long> roleIds, List<long> postIds, List<SysPostOutput> posts, List<SysRoleOutput> roles)
    {
        UserOutput = userOutput;
        RoleIds = roleIds;
        PostIds = postIds;
        Posts = posts;
        Roles = roles;
    }

    public static SysUserOutputWithRoleAndPosts CreateInstance(SysUserOutput userOutput, List<long> roleIds, List<long> postIds, List<SysPostOutput> posts, List<SysRoleOutput> roles)
    {
        return new SysUserOutputWithRoleAndPosts(userOutput, roleIds, postIds, posts, roles);
    }

    public SysUserOutput UserOutput{ get; set; }

    /// <summary>
    /// 角色列表
    /// </summary>
    public List<long> RoleIds { get; set; }

    /// <summary>
    /// 岗位列表
    /// </summary>
    public List<long> PostIds { get; set; }
    
    public  List<SysPostOutput> Posts{ get; set; }
    
    public  List<SysRoleOutput> Roles{ get; set; }
}

/// <summary>
/// 权限字符 }