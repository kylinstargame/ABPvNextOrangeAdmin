using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Organization.Dto;
using ABPvNextOrangeAdmin.System.User.Dto;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System.Organization;

public interface IOrganizationService
{
    /// <summary>
    /// 获取用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<List<SysDeptTreeSelectOutput>>> GetListAsync(DeptInput input);
    
    
    /// <summary>
    /// 获取用户树结构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<List<SysDeptTreeSelectOutput>>> GetTreeAsync(DeptInput input);
}