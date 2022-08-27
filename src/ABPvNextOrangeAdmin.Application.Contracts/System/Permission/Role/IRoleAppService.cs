using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Permission.Dto;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System.Permission.Role;

public interface IRoleAppService
{
    /// <summary>
    /// 获取角色列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<PagedResultDto<RoleOutput>>> GetListAsync(RoleListInput input);
}