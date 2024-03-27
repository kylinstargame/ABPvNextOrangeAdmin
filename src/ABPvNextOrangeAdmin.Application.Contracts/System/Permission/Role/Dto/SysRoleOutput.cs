using System;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System.Permission.Dto;

public class SysRoleOutput:EntityDto<long>
{
    /// <summary>
    /// 角色名称aaaaaaaaaa1
    /// </summary>
    public String RoleName { get; set; }

    /// <summary>
    /// 权限字符 
    /// </summary>
    public string RoleKey { get; set; }
    
    /// <summary>
    /// 显示顺序
    /// </summary>
    public int roleSort { get; set; }
    
    /// <summary>
    /// 角色状态（0-正常 1-锁定）
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 菜单树选择项是否关联显示（ 0：父子不互相关联显示 1：父子互相关联显示）
    /// </summary>
    public bool MenuCheckStrictly { get; set; }

    /// <summary>
    /// 部门树选择项是否关联显示（0：父子不互相关联显示 1：父子互相关联显示 ） 
    /// </summary>
    public bool DeptCheckStrictly { get; set; }
    

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreationTime { get; set; }

}   