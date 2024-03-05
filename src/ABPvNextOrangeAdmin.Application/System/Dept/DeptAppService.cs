using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.ObjectMapper;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.Organization.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;

namespace ABPvNextOrangeAdmin.System.Dept;

[Authorize]
[Route("api/sys/dept/[action]")]
public class DeptAppService : ApplicationService, IOrganizationService
{
    private IDeptRepository DeptRepository { get; }
    private DeptObjectMapper DeptObjectMapper { get; }

    public DeptAppService(IDeptRepository deptRepository, DeptObjectMapper deptObjectMapper)
    {
        DeptRepository = deptRepository;
        DeptObjectMapper = deptObjectMapper;
    }

    [HttpGet]
    [ActionName("list")]
    public async Task<CommonResult<List<SysDeptTreeSelectOutput>>> GetListAsync(DeptInput input)
    {
        var organizations = await DeptRepository.GetListAsync();
        var deptOutputs = ObjectMapper.Map<List<SysDept>, List<SysDeptTreeSelectOutput>>(organizations);
        var deptTree = BuildDeptTree(deptOutputs, 1);
        return CommonResult<List<SysDeptTreeSelectOutput>>.Success(deptTree, "获取部分树状结构成功");
    }

    [HttpGet]
    [ActionName("tree")]
    public async Task<CommonResult<List<SysDeptTreeSelectOutput>>> GetTreeAsync(DeptInput input)
    {
        long count = await DeptRepository.GetCountAsync();
        var organizations = await DeptRepository.GetListAsync();
        //deptOutputsxx=DeptObjectMapper.Map<SysDept, SysDeptTreeSelectOutput>(organizations[0]);
        var deptOutputs = ObjectMapper.Map<List<SysDept>, List<SysDeptTreeSelectOutput>>(organizations);
        // var deptOutputs1 = ObjectMapper.Map<List<SysDept>, List<SysDeptOutput>>(organizations);
        // List<SysDeptTreeSelectOutput> deptOutputs = new List<SysDeptTreeSelectOutput>();
        var deptTree = BuildDeptTree(deptOutputs, 1);

        return CommonResult<List<SysDeptTreeSelectOutput>>.Success(deptTree, "获取部门树状结构成功");
    }

    [HttpGet]
    [ActionName("treeSelectForRole")]
    public async Task<CommonResult<SysDeptTreeSelectForRoleOutput>> GetTreeSelectByRoleIdAsync(long roleId)
    {
        long count = await DeptRepository.GetCountAsync();
        var organizations = await DeptRepository.GetListAsync();
        //deptOutputsxx=DeptObjectMapper.Map<SysDept, SysDeptTreeSelectOutput>(organizations[0]);
        var deptOutputs = ObjectMapper.Map<List<SysDept>, List<SysDeptTreeSelectOutput>>(organizations);
        // var deptOutputs1 = ObjectMapper.Map<List<SysDept>, List<SysDeptOutput>>(organizations);
        // List<SysDeptTreeSelectOutput> deptOutputs = new List<SysDeptTreeSelectOutput>();
        var deptTree = BuildDeptTree(deptOutputs, 1);

        var deptTreeForRole = SysDeptTreeSelectForRoleOutput.CreateInstance(deptTree, new List<long>());
        return CommonResult<SysDeptTreeSelectForRoleOutput>.Success(deptTreeForRole, "获取菜单树成功");
    }

    private List<SysDeptTreeSelectOutput> BuildDeptTree(List<SysDeptTreeSelectOutput> deptOutputs, long? parentId)
    {
        List<SysDeptTreeSelectOutput> returnDeptOutputs = new List<SysDeptTreeSelectOutput>();
        foreach (var deptOutput in deptOutputs)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (deptOutput != null && deptOutput.Id.Equals(parentId))
            {
                RecursionFn(deptOutputs, deptOutput);
                returnDeptOutputs.Add(deptOutput);
            }
        }

        return returnDeptOutputs;
    }

    private void RecursionFn(List<SysDeptTreeSelectOutput> depts, SysDeptTreeSelectOutput parent)
    {
        // 得到子节点列表
        List<SysDeptTreeSelectOutput> childDepts = GetChildList(depts, parent);
        parent.Children = childDepts;
        foreach (SysDeptTreeSelectOutput tChild in childDepts)
        {
            if (HasChild(depts, tChild))
            {
                RecursionFn(depts, tChild);
            }
        }
    }

    private bool HasChild(List<SysDeptTreeSelectOutput> depts, SysDeptOutput sysDept)
    {
        return GetChildList(depts, sysDept).Count > 0;
    }

    private List<SysDeptTreeSelectOutput> GetChildList(List<SysDeptTreeSelectOutput> depts, SysDeptOutput sysDept)
    {
        List<SysDeptTreeSelectOutput> resultMenus = new List<SysDeptTreeSelectOutput>();
        IEnumerator it = depts.GetEnumerator();
        while (it.MoveNext())
        {
            SysDeptTreeSelectOutput current = (SysDeptTreeSelectOutput)it.Current;
            if (current != null && current.ParentId == sysDept.Id)
            {
                resultMenus.Add(current);
            }
        }

        return resultMenus;
    }
}