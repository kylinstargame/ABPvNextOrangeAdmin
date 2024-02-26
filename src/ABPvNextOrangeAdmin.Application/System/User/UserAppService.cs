using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.ObjectMapper;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.Organization.Dto;
using ABPvNextOrangeAdmin.System.Permission.Dto;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;

namespace ABPvNextOrangeAdmin.System.User;

[Authorize]
[Route("api/sys/user/[action]")]
public class UserAppService : ApplicationService, IUserAppService
{
    public UserManager userManager { get; }
    public SysUserStore UserStore { get; }

    public IRepository<SysUser> UserRepository { get; }

    // public IRepository<OrganizationUnit> OrganizationUnitRepository { get; }
    private UserOutObjectMapper userObjectMapper { get; }

    public IRepository<SysPost> PostRepository { get; }

    public IRepository<SysRole> RoleRepository { get; }

    public UserAppService(IRepository<SysUser> userRepository, SysUserStore userStore,
        UserOutObjectMapper userObjectMapper, IRepository<SysPost> postRepository, IRepository<SysRole> roleRepository,
        UserManager userManager
        /*IRepository<OrganizationUnit> organizationUnitRepository*/)
    {
        UserRepository = userRepository;
        UserStore = userStore;
        this.userObjectMapper = userObjectMapper;
        PostRepository = postRepository;
        RoleRepository = roleRepository;
        this.userManager = userManager;
        // OrganizationUnitRepository = organizationUnitRepository;
    }

    [HttpGet]
    [ActionName("get")]
    public async Task<CommonResult<SysUserOutputWithRoleAndPosts>> GetAsync(string userId)
    {
        List<SysUser> users = new List<SysUser>();
        var user = await UserStore.FindByIdAsync(userId);
        List<SysRole> roles = await RoleRepository.GetListAsync();
        List<SysPost> posts = await PostRepository.GetListAsync();
        var rolerOutputs = ObjectMapper.Map<List<SysRole>, List<SysRoleOutput>>(roles);
        var postOutputs = ObjectMapper.Map<List<SysPost>, List<SysPostOutput>>(posts);
        if (user != null)
        {
            var userOutput = ObjectMapper.Map<SysUser, SysUserOutput>(user);


            List<long> roleIds = await UserStore.GetRoleIdsAsync(user);

            List<long> postIds = user.Posts.Select(x => x.Id).ToList();
            // SysUserOutput userOutput = ObjectMapper.Map<SysUser, SysUserOutput>(user);
            return CommonResult<SysUserOutputWithRoleAndPosts>.Success(
                SysUserOutputWithRoleAndPosts.CreateInstance(userOutput, roleIds, postIds, postOutputs, rolerOutputs),
                "获取用户列表成功");
        }
        else
        {
            // SysUserOutput userOutput = ObjectMapper.Map<SysUser, SysUserOutput>(user);
            return CommonResult<SysUserOutputWithRoleAndPosts>.Success(
                SysUserOutputWithRoleAndPosts.CreateInstance(null, null, null, postOutputs, rolerOutputs),
                "获取用成功");
        }
    }

    [HttpGet]
    [ActionName("list")]
    public async Task<CommonResult<PagedResultDto<SysUserOutput>>> GetListAsync(UserListInput input)
    {
        var queryale = await UserRepository.GetQueryableAsync();

        var sysUsers = queryale
            .WhereIf<SysUser, IQueryable<SysUser>>(input.UserId != Guid.Empty, x => x.Id == input.UserId)
            .WhereIf(!String.IsNullOrWhiteSpace(input.UserName), x => x.UserName.Contains(input.UserName))
            .WhereIf(!String.IsNullOrWhiteSpace(input.PhoneNumber), x => x.PhoneNumber == input.PhoneNumber)
            // // .WhereIf(String.IsNullOrWhiteSpace(input.DeptId),
            //     x => OrganizationUnitRepository.GetQueryableAsync().Result.Select(x => x.Id).Any(input.DeptId))
            .Where(x => x.IsDeleted == input.Status)
            .WhereIf(input.BeginTime != null, x => x.CreationTime > input.BeginTime)
            .WhereIf(input.EndTime != null, x => x.CreationTime > input.EndTime)
            .PageBy(input.SkipCount, input.MaxResultCount).ToList();

        var userOutput = ObjectMapper.Map<List<SysUser>, List<SysUserOutput>>(sysUsers);
        return CommonResult<PagedResultDto<SysUserOutput>>.Success(
            new PagedResultDto<SysUserOutput>(sysUsers.Count(), userOutput), "获取用户列表成功");
    }


    [HttpPost]
    [ActionName("add")]
    public async Task<CommonResult<String>> CreateAsync(SysUserUpdateInput input)
    {
        if (!await CheckPhoneUnique(input.phoneNumber))
        {
            return CommonResult<String>.Failed(string.Format("电话{0}已被使用", input.phoneNumber));
        }

        if (!await CheckEmailUnique(input.email))
        {
            return CommonResult<String>.Failed(string.Format("邮箱{0}已被使用", input.email));
        }

        var user = ObjectMapper.Map<SysUserUpdateInput, SysUser>(input);
        await userManager.CreateAsync(user, input.password, false);
        return CommonResult<String>.Success("", "添加用户完成");
        
    }

    [HttpGet]
    [HttpPut]
    [ActionName("edit")]
    public async Task<CommonResult<string>> UpdateAsync(SysUserUpdateInput input)
    {
        if (!await CheckPhoneUnique(input.phoneNumber))
        {
            return CommonResult<String>.Failed(string.Format("电话{0}已被使用", input.phoneNumber));
        }

        if (!await CheckEmailUnique(input.email))
        {
            return CommonResult<String>.Failed(string.Format("邮箱{0}已被使用", input.email));
        }

        var user = await UserStore.FindByIdAsync(CurrentUser.Id.ToString());
        user = ObjectMapper.Map<SysUserUpdateInput, SysUser>(input, user);
        // try
        // {
        await UserStore.UpdateAsync(user);
        // }
        // catch (global::System.Exception e)
        // {
        //
        //     throw;
        // }

        return CommonResult<String>.Success("", "无效更新");
    }

    [HttpPost]
    [ActionName("delete")]
    public async Task<CommonResult<string>> DeleteAsync(string userId)
    {
        // foreach (var userId in userIds)
        {
            SysUser user;
            user = await UserStore.FindByIdAsync(userId.ToString());
            await UserStore.DeleteAsync(user);
        }

        return CommonResult<String>.Success("", "刪除用戶成功");
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut]
    [ActionName("resetPwd")]
    public async Task<CommonResult<string>> ResetPasswordAsync(UseResetPasswordInput input)
    {
        var user = await UserStore.FindByIdAsync(input.UserId.ToString());
        if (user.IsAdmin())
        {
            return CommonResult<String>.Failed("不允许操作超级管理员用户");
        }

        // user.Password = input.Password;
        var IdentityResult = await userManager.ChangePasswordAsync(user, user.Password, input.Password);
        if (IdentityResult.Succeeded)

        {
            user.Password = input.Password;
            await userManager.UpdateAsync(user);
            return CommonResult<string>.Success("", "用戶密碼更新成功");
        }
        else
        {
            return CommonResult<string>.Failed(IdentityResult.ToString());
        }
    }

    private async Task<bool> CheckPhoneUnique(String phoneNumber)
    {
        var users = await UserStore.FindByPhoneNumberAsync(phoneNumber);
        if (users != null && users.Count(u => u.Id != CurrentUser.Id) > 0)
        {
            return false;
        }

        return true;
    }

    private async Task<bool> CheckEmailUnique(String email)
    {
        var users = await UserStore.FindByEmailAsync(email);
        if (users != null && users.Count(u => u.Id != CurrentUser.Id) > 0)
        {
            return false;
        }

        return true;
    }
}