using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace ABPvNextOrangeAdmin.System.Account.Dto;

public class AccountOutput
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public IdentityUserDto User { get; set; }
    
    /// <summary>
    /// 角色信息
    /// </summary>
    public IdentityRoleDto Roles { get; set; }
    
   
    /// <summary>
    /// 角色权限
    /// </summary>
    public PermissionGroupDto Permissions { get;}
}