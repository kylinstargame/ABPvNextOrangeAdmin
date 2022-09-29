using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.User;

public class SysUserPost : FullAuditedEntity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    public long PostId { get; set; }


    public override object[] GetKeys()
    {
        return new object[] {UserId, PostId};
    }

    private SysUserPost()
    {
    }

    public static List<SysUserPost> CreateInstances(long userId, int[] postIds)
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