using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Permission.Dto;
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
    public RoleAppService(IIdentityRoleRepository roleRepository)
    {
        this.roleRepository = roleRepository;
    }

    private IIdentityRoleRepository roleRepository { get; }

    [HttpGet]
    [ActionName("list")]
    public async Task<CommonResult<PagedResultDto<RoleOutput>>> GetListAsync(RoleListInput input)
    {
        var identityRoles = await this.roleRepository.GetListAsync();
        var roleOutputs = ObjectMapper.Map<List<IdentityRole>, List<RoleOutput>>(identityRoles);
        return CommonResult<PagedResultDto<RoleOutput>>.Success(
            new PagedResultDto<RoleOutput>((long) roleOutputs.Count, roleOutputs), "");
    }
    
    
}