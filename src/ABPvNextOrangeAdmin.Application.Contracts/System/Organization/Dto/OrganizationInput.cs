using System;
using System.Collections.Generic;

namespace ABPvNextOrangeAdmin.System.Organization.Dto;

public class OrganizationInput
{
    /** 父部门ID */
    public long ParentId { get; set; }

    /** 祖级列表 */
    public string Ancestors { get; set; }

    /** 部门名称 */
    public string DeptName { get; set; }

    /** 显示顺序 */
    public int OrderNum { get; set; }

    /** 负责人 */
    public String Leader { get; set; }

    /** 联系电话 */
    public String Phone { get; set; }

    /** 邮箱 */
    public string Email { get; set; }

    /** 部门状态:0正常,1停用 */
    public string Status { get; set; }

    /** 删除标志（0代表存在 2代表删除） */
    public string DelFlag { get; set; }

    /** 父部门名称 */
    public string ParentName { get; set; }
}