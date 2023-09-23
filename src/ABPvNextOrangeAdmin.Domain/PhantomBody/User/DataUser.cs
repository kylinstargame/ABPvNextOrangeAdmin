using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ABPvNextOrangeAdmin.PhantomBody.User;

public class DataUser : AuditedAggregateRoot<Guid>
{
    public DataUser(Guid? tenantId)
    {
    }
    
    /// <summary>
    /// 微信OPENID
    /// </summary>
    public String OpenId;

    /// <summary>
    /// 登录名称
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 用户密码
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// 登录邮箱
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
    /// 用户所在省份
    /// </summary>
    public string Province { get; set; }
    
    
}