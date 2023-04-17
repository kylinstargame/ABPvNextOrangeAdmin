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
using Volo.Abp.Identity;

namespace ABPvNextOrangeAdmin.System.Permission.Role;

[Authorize]
[Route("api/sys/role/[action]")]
public class RoleAppService : ApplicationService, IRoleAppService
{
    public RoleAppService(IRoleRepository roleRepository)
    {
        this.roleRepository = roleRepository;
    }

    private IRoleRepository roleRepository { get; }

    [HttpGet]
    [ActionName("list")]
    public async Task<CommonResult<PagedResultDto<SysRoleOutput>>> GetListAsync(RoleListInput input)
    {
        var sysRoles = await this.roleRepository.GetListAsync();
        var roleOutputs = ObjectMapper.Map<List<SysRole>, List<SysRoleOutput>>(sysRoles);
        return CommonResult<PagedResultDto<SysRoleOutput>>.Success(
            new PagedResultDto<SysRoleOutput>((long) roleOutputs.Count, roleOutputs), "");
    }
    
    
}