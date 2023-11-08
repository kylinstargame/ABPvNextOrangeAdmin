using System.Collections.Generic;

namespace ABPvNextOrangeAdmin.System.Account.Dto;

public class SysUserUpdateInput
{
    /// <summary>
    /// 
    /// </summary>
    public string deptId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string userName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string email { get; set; }
    /// <summary>
    /// 橙卡软件科技
    /// </summary>
    public string nickName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string phoneNumber { get; set; }
    /// <summary>
    /// 男
    /// </summary>
    public string sex { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string avatar { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string password { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string status { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string loginIP { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string loginTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string creationTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string tenantId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string userPassword { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List <long > postIds { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List <long > roleIds { get; set; }
}