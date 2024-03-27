using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ABPvNextOrangeAdmin.Constans;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ABPvNextOrangeAdmin.System.Dept;

/// <summary>
/// 系统部门
/// </summary>
public sealed class SysDept : FullAuditedAggregateRoot<long>, IMultiTenant
{
    public SysDept(long id)
    {
        Id = id;

    }
    public SysDept(long id, string deptName)
    {
        Id = id;
        DeptName = deptName;
    }

    public SysDept(long id,string deptName, Guid? tenantId)
    {
        Id = id;
        DeptName = deptName;
        TenantId = tenantId;
    }

    public SysDept(long id, string deptName, long parentId,Guid? tenantId = null)
    {
        Id = id;
        DeptName = deptName;
        ParentId = parentId;
    }
    

    
    /// <summary>
    /// 父部门ID
    /// </summary>
    public long? ParentId { get; set; } = null;

    /// <summary>
    /// 祖级列表 如 000.001.005
    /// </summary>
    public string Ancestors { get; set; }

    /// <summary>
    /// 部门名称 
    /// </summary>
    public string DeptName { get; set; }

    /// <summary>
    /// 部门代码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 显示顺序  
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 负责人姓名 
    /// </summary>
    public string Leader { get; set; }

    /// <summary>
    /// 联系电话 
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// 邮箱账号
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 部门状态:0正常,1停用 
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 租户ID
    /// </summary>
    public Guid? TenantId { get; }

    /// <summary>
    /// 关联角色
    /// </summary>
    public ICollection<SysRoleDept> Roles { get; set; }
    public ICollection<SysUser> Users { get; set; }

    #region 组织代码

    public static string CreateCode(params int[] numbers)
    {
        if (numbers.IsNullOrEmpty())
        {
            return null;
        }

        return numbers.Select(number => number.ToString(new string('0', DeptConsts.CodeUnitLength))).JoinAsString(".");
    }

    public static string AppendCode(string parentCode, string childCode)
    {
        if (childCode.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
        }

        if (parentCode.IsNullOrEmpty())
        {
            return childCode;
        }

        return parentCode + "." + childCode;
    }

    public static string GetRelativeCode(string code, string parentCode)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        if (parentCode.IsNullOrEmpty())
        {
            return code;
        }

        if (code.Length == parentCode.Length)
        {
            return null;
        }

        return code.Substring(parentCode.Length + 1);
    }

    public static string CalculateNextCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var parentCode = GetParentCode(code);
        var lastUnitCode = GetLastUnitCode(code);

        return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
    }

    public static string GetLastUnitCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var splittedCode = code.Split('.');
        return splittedCode[splittedCode.Length - 1];
    }

    public static string GetParentCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var splittedCode = code.Split('.');
        if (splittedCode.Length == 1)
        {
            return null;
        }

        return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
    }

    #endregion

    #region 角色相关

    public bool HasRole(long roleId)
    {
        Check.NotNull(roleId, nameof(roleId));
        return Roles.Any(role => role.RoleId == roleId);
    }

    public void AddRole(long roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (!HasRole(roleId))
        {
            return;
        }

        Roles.Add(SysRoleDept.CreateInstance(roleId, Id, TenantId));
    }

    public void RemoveRole(long roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (!HasRole(roleId))
        {
            return;
        }

        Roles.RemoveAll(r => r.RoleId == roleId);
    }

    #endregion
}

public static class DeptEfCoreQueryableExtensions
{
    public static IQueryable<SysDept> IncludeDetails(this IQueryable<SysDept> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.Roles);
    }
}