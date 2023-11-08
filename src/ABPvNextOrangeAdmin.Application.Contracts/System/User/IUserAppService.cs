using System;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.User.Dto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace ABPvNextOrangeAdmin.System.User;

public interface IUserAppService
{
    /// <summary>
    /// 获取用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<SysUserOutputWithRoleAndPosts>> GetAsync(string userId);

    /// <summary>
    /// 获取用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<PagedResultDto<SysUserOutput>>> GetListAsync(UserListInput input);
    
    
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<String>> CreateAsync(UserListInput input);
    
    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<String>> UpdateAsync(SysUserUpdateInput input);

    public Task<CommonResult<string>> ResetPasswordAsync(UseResetPasswordInput password);


    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="input"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    public Task<CommonResult<String>>  DeleteAsync(string userId);
}