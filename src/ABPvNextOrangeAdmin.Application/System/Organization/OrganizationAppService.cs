using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
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
public class OrganizationAppService : ApplicationService, IOrganizationService
{
    private IRepository<OrganizationUnit> OrganzationRepository { get; }

    public OrganizationAppService(IRepository<OrganizationUnit> organzationRepository)
    {
        OrganzationRepository = organzationRepository;
    }

    [HttpGet]
    [ActionName("list")]
    public async Task<CommonResult<PagedResultDto<OrganizationOutput>>> GetListAsync(OrganizationInput input)
    {
       var organizations = await OrganzationRepository.GetListAsync();
       var deptOutputs = ObjectMapper.Map<List<OrganizationUnit>, List<DeptOutput>>(organizations);
       var deptTree = BuildDeptTree(deptOutputs, Guid.Empty);
       return null;
    }

    [HttpGet]
    [ActionName("tree")] 
    public async Task<CommonResult<List<DeptOutput>>> GetTreeAsync(OrganizationInput input)
    {
        var organizations = await OrganzationRepository.GetListAsync();
        var deptOutputs = ObjectMapper.Map<List<OrganizationUnit>, List<DeptOutput>>(organizations);
        var deptTree = BuildDeptTree(deptOutputs, null);
        return CommonResult<List<DeptOutput>>.Success(deptTree, "获取部分树状结构成功");
    }

    private List<DeptOutput> BuildDeptTree(List<DeptOutput> deptOutputs, Guid? parentId)
    {
        List<DeptOutput> returnDeptOutputs = new List<DeptOutput>();
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
    
    private void RecursionFn(List<DeptOutput> depts, DeptOutput parent)
    {
        // 得到子节点列表
        List<DeptOutput> childDepts = GetChildList(depts, parent);
        parent.Children = childDepts;
        foreach (DeptOutput tChild in childDepts)
        {
            if (HasChild(depts, tChild))
            {
                RecursionFn(depts, tChild);
            }
        }
    }

    private bool HasChild(List<DeptOutput> depts, DeptOutput dept)
    {
        return GetChildList(depts, dept).Count > 0;
    }

    private List<DeptOutput> GetChildList(List<DeptOutput> depts, DeptOutput dept)
    {
        List<DeptOutput> resultMenus = new List<DeptOutput>();
        IEnumerator it = depts.GetEnumerator();
        while (it.MoveNext())
        {
            DeptOutput current = (DeptOutput)it.Current;
            if (current != null && current.ParentId == dept.Id)
            {
                resultMenus.Add(current);
            }
        }

        return resultMenus;
    }

}