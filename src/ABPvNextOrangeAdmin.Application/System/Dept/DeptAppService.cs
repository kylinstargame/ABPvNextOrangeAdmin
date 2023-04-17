using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Dept;
using ABPvNextOrangeAdmin.System.Menu;
using ABPvNextOrangeAdmin.System.Organization.Dto;
using ABPvNextOrangeAdmin.System.User.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.Organization;

[Authorize]
[Route("api/sys/dept/[action]")]
public class DeptAppService : ApplicationService, IOrganizationService
{
    private IDeptRepository DeptRepository { get; }

    public DeptAppService(IDeptRepository deptRepository)
    {
        DeptRepository = deptRepository;
    }

    [HttpGet]
    [ActionName("list")]
    public async Task<CommonResult<List<SysDeptOutput>>> GetListAsync(DeptInput input)
    {
        var organizations = await DeptRepository.GetListAsync();
        var deptOutputs = ObjectMapper.Map<List<SysDept>, List<SysDeptOutput>>(organizations);
        var deptTree = BuildDeptTree(deptOutputs, null);
        return CommonResult<List<SysDeptOutput>>.Success(deptTree, "获取部分树状结构成功");
    }

    [HttpGet]
    [ActionName("tree")]
    public async Task<CommonResult<List<SysDeptOutput>>> GetTreeAsync(DeptInput input)
    {
        long count = await DeptRepository.GetCountAsync();
        var organizations = await DeptRepository.GetListAsync();
        var deptOutputs = ObjectMapper.Map<List<SysDept>, List<SysDeptOutput>>(organizations);
        var deptTree = BuildDeptTree(deptOutputs, 1);
        return CommonResult<List<SysDeptOutput>>.Success(deptTree, "获取部门树状结构成功");
    }

    private List<SysDeptOutput> BuildDeptTree(List<SysDeptOutput> deptOutputs, long? parentId)
    {
        List<SysDeptOutput> returnDeptOutputs = new List<SysDeptOutput>();
        foreach (var deptOutput in deptOutputs)
        {
            if (deptOutput.Id.Equals(parentId))
            {
                RecursionFn(deptOutputs, deptOutput);
                returnDeptOutputs.Add(deptOutput);
            }
        }

        return returnDeptOutputs;
    }

    private void RecursionFn(List<SysDeptOutput> depts, SysDeptOutput parent)
    {
        // 得到子节点列表
        List<SysDeptOutput> childDepts = GetChildList(depts, parent);
        parent.Children = childDepts;
        foreach (SysDeptOutput tChild in childDepts)
        {
            if (HasChild(depts, tChild))
            {
                RecursionFn(depts, tChild);
            }
        }
    }

    private bool HasChild(List<SysDeptOutput> depts, SysDeptOutput sysDept)
    {
        return GetChildList(depts, sysDept).Count > 0;
    }

    private List<SysDeptOutput> GetChildList(List<SysDeptOutput> depts, SysDeptOutput sysDept)
    {
        List<SysDeptOutput> resultMenus = new List<SysDeptOutput>();
        IEnumerator it = depts.GetEnumerator();
        while (it.MoveNext())
        {
            SysDeptOutput current = (SysDeptOutput)it.Current;
            if (current != null && current.ParentId == sysDept.Id)
            {
                resultMenus.Add(current);
            }
        }

        return resultMenus;
    }
}