using System;
using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.Account.Dto;

namespace ABPvNextOrangeAdmin.System.User.Dto;

public class SysUserOutputWithRoleAndPosts
{
    private SysUserOutputWithRoleAndPosts(SysUserOutput userOutput, List<String> roleIds, List<long> postIds)
    {
        UserOutput = userOutput;
        RoleIds = roleIds;
        PostIds = postIds;
    }

    public static SysUserOutputWithRoleAndPosts CreateInstance(SysUserOutput userOutput, List<String> roleIds, List<long> postIds)
    {
        return new SysUserOutputWithRoleAndPosts(userOutput, roleIds, postIds);
    }


    public SysUserOutput UserOutput{ get; set; }

    /// <summary>
    /// 角色列表
    /// </summary>
    public List<String> RoleIds { get; set; }

    /// <summary>
    /// 岗位列表
    /// </summary>
    public List<long> PostIds { get; set; }
}

/// <summary>
/// 权限字符 }