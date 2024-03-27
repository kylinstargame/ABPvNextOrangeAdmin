using System;
using System.Collections.Generic;

namespace ABPvNextOrangeAdmin.System.Permission.Dto;

public class SysRoleUpdateInput
{
    public long Id { get; set;}

    /// <summary>
    /// 角色名称
    /// </summary>
    public String RoleName { get; set; }

    /// <summary>
    /// 角色权限字符
    /// </summary>
    public String RoleKey{ get; set; }
    
    /// <summary>
    /// 角色状态（0正常 1停用）
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// 角色排序
    /// </summary>
    public int roleSort { get; set; }
    
    /// <summary>
    /// 数据权限
    /// </summary>
    public String DataScope { get; set; }

    /// <summary>
    /// 角色备注
    /// </summary>
    public string Remark { get; set; } 

    /// <summary>
    /// 菜单树选择项是否关联显示（ 0：父子不互相关联显示 1：父子互相关联显示）
    /// </summary>
    public bool MenuCheckStrictly { get; set; }

    /// <summary>
    /// 部门树选择项是否关联显示（0：父子不互相关联显示 1：父子互相关联显示 ） 
    /// </summary>
    public bool DeptCheckStrictly { get; set; }
    
    public List<int> menuIds { get; set; }

    public List<long> deptIds { get; set; }
}