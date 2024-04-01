using System;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.Dto;
using ABPvNextOrangeAdmin.System.Permission.Dto;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin;

public interface IStaffAppService
{
   
    /// <summary>
    /// 获取角色列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<PagedResultDto<StaffOutput>>> GetListAsync(RoleListInput input);
    
    /// <summary>
    /// 根據Id獲取角色·
    /// </summary>
    /// <param name="staffId"></param>
    /// <returns></returns>
    public Task<CommonResult<SysRoleOutput>> GetAsync(string staffId,String Name,int Year);

    /// <summary>
    /// 添加角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<String>> CreateAsync(StaffUpdateInutput input);
    
    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<String>> UpdateAsync(StaffUpdateInutput input);
    
    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="staffId"></param>
    /// <returns></returns>
    public Task<CommonResult<String>> DeleteAsync(long staffId); 
}