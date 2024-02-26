using System;

namespace ABPvNextOrangeAdmin.System.Permission.Dto;

public class SysRoleUpdateInput
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public String RoleName { get; set; }

    /// <summary>
    /// 角色权限字符
    /// </summary>
    public String Permissions;
    
    /// <summary>
    /// 角色状态（0正常 1停用）
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 角色排序
    /// </summary>
    public String Order { get; set; }
    
}