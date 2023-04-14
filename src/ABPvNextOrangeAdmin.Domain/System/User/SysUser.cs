using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ABPvNextOrangeAdmin.System.Dept;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.Roles;
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
    
    public SysUser(Guid id, string userName, string email, string password, Guid? tenantId = null)
    {
        Id = id;
        UserName = userName;
        TenantId = tenantId;
        Email = email;
        Password = password;
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

    public SysUser(Guid id, string userName, string email, string password, bool lockoutEnabled, DateTimeOffset? lockoutEnd,
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
    /// 部门ID
    /// </summary>
    public long DeptId { get; set; }

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
    public DateTime? CreationTime { get; set; }

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
    public ICollection<SysUserRole> Roles { get;  set; }

    /// <summary>
    /// 关联部门
    /// </summary>
    public ICollection<SysDept> Depts { get;  set; }


    public bool IsInRole(long roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        return Roles.Any(r => r.RoleId == roleId);
    }


    public void AddRole(long roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (IsInRole(roleId))
        {
            return;
        }

        Roles.Add(new SysUserRole(Id, roleId, TenantId));
    }

    public void RemoveRole(long roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (!IsInRole(roleId))
        {
            return;
        }

        Roles.RemoveAll(r => r.RoleId == roleId);
    }
}