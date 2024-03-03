using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Permission.Dto;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using NotImplementedException = System.NotImplementedException;
using SysRoleStore = ABPvNextOrangeAdmin.System.Roles.SysRoleStore;

namespace ABPvNextOrangeAdmin.System.Permission.Role;

[Authorize]
[Route("api/sys/role/[action]")]
public class RoleAppService : ApplicationService, IRoleAppService
{
    public RoleAppService(SysRoleStore roleStore, IRoleRepository roleRepository,
        IRepository<SysRoleMenu> roleMenuRepository)
    {
        RoleStore = roleStore;

        RoleRepository = roleRepository;
        RoleMenuRepository = roleMenuRepository;
    }


    public SysRoleStore RoleStore { get; set; }

    private IRepository<SysRoleMenu> RoleMenuRepository { get; set; }

    private IRoleRepository RoleRepository { get; }

    [HttpGet]
    [ActionName("list")]
    public async Task<CommonResult<PagedResultDto<SysRoleOutput>>> GetListAsync(RoleListInput input)
    {
        var sysRoles = await this.RoleRepository.GetListAsync();
        var roleOutputs = ObjectMapper.Map<List<SysRole>, List<SysRoleOutput>>(sysRoles);
        return CommonResult<PagedResultDto<SysRoleOutput>>.Success(
            new PagedResultDto<SysRoleOutput>((long)roleOutputs.Count, roleOutputs), "");
    }

    [HttpPost]
    [ActionName("get")]
    public async Task<CommonResult<SysRoleOutput>> GetAsync(string roleId)
    {
        var role = await RoleStore.FindByIdAsync(roleId);
        var roleOutput = ObjectMapper.Map<SysRole, SysRoleOutput>(role); 

        return CommonResult<SysRoleOutput>.Success(
            roleOutput, "獲取角色信息成功");
    } 
    

    [HttpPost]
    [ActionName("add")]
    public async Task<CommonResult<string>> CreateAsync(SysRoleUpdateInput input)
    {
        var role = ObjectMapper.Map<SysRoleUpdateInput, SysRole>(input);
        var newRole= await RoleRepository.InsertAsync(role,true);
        
        await RoleMenuRepository.InsertManyAsync(SysRoleMenu.CreateInstances(newRole.Id,input.menuIds.ToArray()));
        return CommonResult<string>.Success("", "角色添加完成");
    }
}