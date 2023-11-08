using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ABPvNextOrangeAdmin.System.Dept;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.Roles;
using AutoMapper.Configuration.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ABPvNextOrangeAdmin.System.User;

/// <summary>
/// 系统用户
/// </summary>
public sealed class SysUser : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public SysUser()
    {
    }

    public SysUser(Guid id, string userName, string email, Guid? tenantId = null)
    {
        Id = id;
        UserName = userName;
        TenantId = tenantId;
        Email = email;
        // Password = password;
    }

    public SysUser(Guid id, string userName, string phoneNumber, string email, string password, Guid? tenantId = null)
    {
        Id = id;
        UserName = userName;
        TenantId = tenantId;
        Email = email;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
    }

    public SysUser(Guid id, string userName, string email, string nickName, string phoneNumber, string password,
        Guid? tenantId = null)
    {
        Id = id;
        UserName = userName;
        NickName = nickName;
        Password = password;
        TenantId = tenantId;
        PhoneNumber = phoneNumber;
        Sex = "男";
        Email = email;
        LockoutEnabled = false;
        LockoutEnd = DateTimeOffset.MaxValue;
        Logins = new Collection<SysUserLogin>();
        IsActive = true;
        CreationTime = DateTime.Now;
    }

    public SysUser(Guid id, string userName, string email, string password, bool lockoutEnabled,
        DateTimeOffset? lockoutEnd,
        Guid? tenantId = null)
    {
        Id = id;
        UserName = userName;
        Email = email;
        Password = password;
        LockoutEnabled = lockoutEnabled;
        LockoutEnd = lockoutEnd;
        Logins = new Collection<SysUserLogin>();
        TenantId = tenantId;
        IsActive = true;
        CreationTime = DateTime.Now;
    }


    /// <summary>
    /// 登录名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 登录名称
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// 用户性别
    /// </summary>
    public string Sex { get; set; }

    /// <summary>
    /// 用户头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 用户密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 用户密码哈希值
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// 用户状态 0=正常,1=停用
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    public string LoginIP { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public string LoginTime { get; set; }


    /// <summary>
    /// 最后登录时间
    /// </summary>
    // public DateTime? CreationTime { get; set; }

    /// <summary>
    /// 账户解锁时间
    /// </summary>
    public DateTimeOffset? LockoutEnd { get; set; }

    /// <summary>
    /// 账户是否锁定.
    /// </summary>
    public bool LockoutEnabled { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if the user is active.
    /// </summary>
    public bool IsActive { get; set; }


    /// <summary>
    /// 关联登录记录
    /// </summary>
    public ICollection<SysUserLogin> Logins { get; set; }

    /// <summary>
    /// 关联角色
    /// </summary>
    public ICollection<SysUserRole> Roles { get; set; } = new List<SysUserRole>();

    /// <summary>
    /// 关联部门
    /// </summary>
    public ICollection<SysDept> Depts { get; set; }

    /// <summary>
    /// 关联崗位
    /// </summary>
    public ICollection<SysPost> Posts { get; set; } = new List<SysPost>();


    public bool IsAdmin()
    {
        return UserName.ToLower() == "admin";
    }

    #region 角色相关

    public bool IsInRole(long roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        return Roles.Any(r => r.RoleId == roleId);
    }

    public void ResetRoles(long[] roleIds)
    {
        Roles.Clear();
        Check.NotNull(roleIds, nameof(roleIds));
        // foreach (var roleId in roleIds)
        // {
        //     Roles.Add(new SysUserRole(Id, roleId, TenantId));
        // }
    }

    public void AddRole(long postId)
    {
        Check.NotNull(postId, nameof(postId));

        if (IsInRole(postId))
        {
            return;
        }

        Roles.Add(new SysUserRole(Id, postId, TenantId));
    }

    public void RemoveRole(long postId)
    {
        Check.NotNull(postId, nameof(postId));

        if (!IsInRole(postId))
        {
            return;
        }

        Roles.RemoveAll(r => r.RoleId == postId);
    }

    #endregion

    #region 部门相关

    public bool IsInDept(long deptId)
    {
        return Depts.Any(dept => dept.Id == deptId);
    }


    public void JoinDept(long deptId)
    {
        if (this.IsInDept(deptId))
        {
            return;
        }

        this.Depts.Add(new SysDept(deptId));
    }

    public void QuitDept(long deptId)
    {
        if (this.IsInDept(deptId))
        {
            return;
        }

        Depts.RemoveAll<SysDept>((Func<SysDept, bool>)(userDept => userDept.Id == deptId));
    }

    #endregion
    
    #region 岗位相关

    public bool IsInPost(long postId)
    {
        Check.NotNull(postId, nameof(postId));

        return Posts.Any(up => up.Id== postId);
    }

    public void ResetPosts(long[] postIds)
    {
        Posts.Clear();
        Check.NotNull(postIds, nameof(postIds));
        foreach (var postId in postIds)
        {
            if (postId==1)
            {
                continue;
            }
            
            // Posts.Add(new SysPost(Id, postId));
        }
    }

    public void AddPost(long postId)
    {
        Check.NotNull(postId, nameof(postId));

        if (IsInRole(postId))
        {
            return;
        }

        // Posts.Add(new SysUserPost(Id, postId));
    }

    public void RemovePost(long postId)
    {
        Check.NotNull(postId, nameof(postId));

        if (!IsInRole(postId))
        {
            return;
        }

        Posts.RemoveAll(r => r.Id == postId);
    }

    #endregion

}

public static class UserEfCoreQueryableExtensions
{
    public static IQueryable<SysUser> IncludeDetails(this IQueryable<SysUser> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.Roles)
            .Include(x => x.Logins)
            .Include(x => x.Depts);
    }
}