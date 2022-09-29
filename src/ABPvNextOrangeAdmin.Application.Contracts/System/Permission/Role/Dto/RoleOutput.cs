using System;

namespace ABPvNextOrangeAdmin.System.Permission.Dto;

public class RoleOutput
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public String RoleName { get; set; }

    /// <summary>
    /// 权限字符 
    /// </summary>
    public String RoleKey { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    public int Order { get; set; }
    
    /// <summary>
    /// 角色状态（0-正常 1-锁定）
    /// </summary>
    public string Status { get; set; }
    

    /// <summary>
    /// 创建事件
    /// </summary>
    public DateTime? CreateTime { get; set; }

}   