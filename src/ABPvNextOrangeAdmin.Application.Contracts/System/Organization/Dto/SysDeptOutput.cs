using System;
using System.Collections.Generic;
using ABPvNextOrangeAdmin.Common;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System.Organization.Dto;

public class SysDeptOutput : EntityDto<long>
{
    public long? ParentId { get; set; }
    public string Code { get; set; }
    public string DeptName { get; set; }
    public string Leader { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}

public class SysDeptTreeSelectOutput : SysDeptOutput, ITreeSelectNode<SysDeptTreeSelectOutput>
{
    public string Label { get; set; }
    public List<SysDeptTreeSelectOutput> Children { get; set; }
}

public class SysDeptTreeSelectForRoleOutput
{
    public List<SysDeptTreeSelectOutput> deptTree { get; set; }

    private SysDeptTreeSelectForRoleOutput(List<SysDeptTreeSelectOutput> deptTree, List<long> checkedKeys)
    {
        this.deptTree = deptTree;
        this.checkedKeys = checkedKeys;
    }

    public static SysDeptTreeSelectForRoleOutput CreateInstance(List<SysDeptTreeSelectOutput> deptTree, List<long> checkedKeys)
    {
        return new SysDeptTreeSelectForRoleOutput(deptTree, checkedKeys);
    }

    public List<long> checkedKeys { get; set; }
}