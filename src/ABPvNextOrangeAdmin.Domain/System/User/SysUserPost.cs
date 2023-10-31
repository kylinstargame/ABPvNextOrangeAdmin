using System;
using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.Organization;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.User;

public class SysUserPost : FullAuditedEntity
{
    private SysUserPost(Guid userId, long postId)
    {
        UserId = userId;
        PostId = postId;
    }

    public static SysUserPost CreateInstance(Guid userId, long postId)
    {
        return new SysUserPost(userId, postId);
    }

    /// <summary>
    /// 用户ID
    /// </summary>
    public Guid UserId { get; set; }
    

    /// <summary>
    /// 角色ID
    /// </summary>
    public long PostId { get; set; }
    
    /// <summary>
    /// 用户
    /// </summary> 
    public SysUser User { get; set; }
    

    /// <summary>
    /// 
    /// </summary>
    public SysPost Post { get; set; }


    public override object[] GetKeys()
    {
        return new object[] {UserId, PostId};
    }

    private SysUserPost()
    {
    }

    public static List<SysUserPost> CreateInstances(Guid userId, int[] postIds)
    {
        List<SysUserPost> userRoles = new List<SysUserPost>();
        foreach (var postId in postIds)
        {
            userRoles.Add(new SysUserPost()
            {
                UserId = userId,
                PostId = postId
            });
        }

        return userRoles;
    }
}