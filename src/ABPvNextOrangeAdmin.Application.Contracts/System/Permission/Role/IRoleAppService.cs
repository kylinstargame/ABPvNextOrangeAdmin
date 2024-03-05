using System;
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
    public Task<CommonResult<PagedResultDto<SysRoleOutput>>> GetListAsync(RoleListInput input);
    
    /// <summary>
    /// 根據Id獲取角色·
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public Task<CommonResult<SysRoleOutput>> GetAsync(string roleId);

    /// <summary>
    /// 添加角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<String>> CreateAsync(SysRoleUpdateInput input);
    
    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public Task<CommonResult<String>> DeleteAsync(long roleId);
}