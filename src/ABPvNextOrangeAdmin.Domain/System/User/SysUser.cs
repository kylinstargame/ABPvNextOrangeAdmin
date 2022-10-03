using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ABPvNextOrangeAdmin.System.Roles;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ABPvNextOrangeAdmin.System.User;

/// <summary>
/// 系统用户
/// </summary>
public class SysUser : FullAuditedAggregateRoot<long>, IMultiTenant
{
    public SysUser(string userName, string email, Guid? tenantId = null )
    {
        UserName = userName;
        TenantId = tenantId;
        Email = email;
        Logins = new Collection<SysUserLogin>();
        IsActive = true;

    }

    public SysUser(string userName, string email, string password, Guid? tenantId = null)
    {
        UserName = userName;
        Email = email;
        Password = password;
        Logins = new Collection<SysUserLogin>();
        TenantId = tenantId;
        IsActive = true;

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
    /// 租户Id
    /// </summary>
    public Guid? TenantId { get; }
    
    /// <summary>
    /// Gets or sets a flag indicating if the user is active.
    /// </summary>
    public bool IsActive { get; protected internal set; }
    
    
    /// <summary>
    /// Navigation property for this users login accounts.
    /// </summary>
    public ICollection<SysUserLogin> Logins { get; protected set; }
    
    /// <summary>
    /// Navigation property for the roles this user belongs to.
    /// </summary>
    public ICollection<SysUserRole> Roles { get; protected set; }
    
    
    public virtual bool IsInRole(long roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        return Roles.Any(r => r.RoleId == roleId);
    }

    
    public virtual void AddRole(long roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (IsInRole(roleId))
        {
            return;
        }

        Roles.Add(new SysUserRole(Id, roleId, TenantId));
    }

    public virtual void RemoveRole(long roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (!IsInRole(roleId))
        {
            return;
        }

        Roles.RemoveAll(r => r.RoleId == roleId);
    }
}