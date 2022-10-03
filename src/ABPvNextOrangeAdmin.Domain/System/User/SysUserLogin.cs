using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ABPvNextOrangeAdmin.System.User;

public class SysUserLogin :  FullAuditedAggregateRoot<long>, IMultiTenant
{
    /// <summary>
    /// 登录名称
    /// </summary>
    public string UserName { get; set; }
  
    /// <summary>
    /// 登录ID
    /// </summary>
    public string IPAddress { get; set; }
    
    /// <summary>
    /// 登录定位
    /// </summary>
    public string Location { get; set; }
    
    
    /// <summary>
    /// 登录浏览器
    /// </summary>
    public string Brower { get; set; }
    
    /// <summary>
    /// 登录系统
    /// </summary>
    public string LoginOS { get; set; }
    
    /// <summary>
    /// 登录状态
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 登录消息
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// 登录时间
    /// </summary>
    public DateTime LoginTime { get; set; }
    
    /// <summary>
    /// Gets or sets the login provider for the login (e.g. facebook, google)
    /// </summary>
    public virtual string LoginProvider { get; protected set; }

    
    /// <summary>
    /// Gets or sets the unique provider identifier for this login.
    /// </summary>
    public virtual string ProviderKey { get; protected set; }

    /// <summary>
    /// Gets or sets the friendly name used in a UI for this login.
    /// </summary>
    public virtual string ProviderDisplayName { get; protected set; }
    
    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid? TenantId { get; }
}