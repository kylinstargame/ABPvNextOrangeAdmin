using System;
using Newtonsoft.Json.Converters;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System.Account.Dto;

public class SysUserOutput : EntityDto<Guid>
{
    private SysUserOutput(Guid id, string userName, string Password, string nickName, string avatar, string sex, string email, string phoneNumber, long deptId,string status, string loginIP, string loginTime)
    {
        Id = id;
        UserName = userName;
        userPassword = Password;
        NickName = nickName;
        Avatar = avatar;
        Sex = sex;
        Email = email;
        PhoneNumber = phoneNumber; 
        DeptId = deptId;
        Status = status;
        LoginIP = loginIP;
        LoginTime = loginTime;
    }

    public SysUserOutput()
    {
    }

    public static SysUserOutput CreateInstance(Guid id, string userName, string password, string nickName, string avatar, string sex, string email, string phoneNumber, long deptId, string status, string loginIP, string loginTime)
    {
        return new SysUserOutput(id, userName, password, nickName, avatar, sex, email, phoneNumber, deptId, status, loginIP, loginTime);
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
    public string userPassword { get; set; }

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
    /// 租户Id
    /// </summary>
    public Guid? TenantId { get; } 
}