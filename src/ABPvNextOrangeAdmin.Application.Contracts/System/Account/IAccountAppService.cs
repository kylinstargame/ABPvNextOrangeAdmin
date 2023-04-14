using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Account.Dto;
// using Volo.Abp.Account;
using Volo.Abp.Identity;

namespace ABPvNextOrangeAdmin.System.Account;

public interface IAccountAppService
{
    /// <summary>
    /// 账号注册
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<SysUserOutput>> RegisterAsync(RegisterInput input);

    /// <summary>
    /// 账号登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<string>> LoginAsync(LoginInput input);

    /// <summary>
    /// 账号登出
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<CommonResult<string>> LogoutAsync();


    /// <summary>
    /// 获取图片验证码
    /// </summary>
    /// <returns></returns>
    public Task<CommonResult<CaptchaCodeOutput>> GetCaptchaImageAsync();
    
    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    public Task<CommonResult<UserWithRoleAndPermissionOutput>> GetUserInfoAsync();


    /// <summary>
    /// 获取路由信息
    /// </summary>
    /// <returns></returns>
    public Task<CommonResult<List<RouteOutput>>> GetRouters();

    // /// <summary>

    // /// 发送重置密码验证码

    // /// </summary>

    // /// <param name="input"></param>

    // /// <returns></returns>

    // /// <exception cref="NotImplementedException"></exception>

    // public Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input);


    // /// <summary>

    // /// 重置密码

    // /// </summary>

    // /// <param name="input"></param>

    // /// <returns></returns>

    // public Task ResetPasswordAsync(ResetPasswordDto input);
}