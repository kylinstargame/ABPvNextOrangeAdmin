using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.ObjectMapper;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.User.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace ABPvNextOrangeAdmin.System.User;
[Authorize]
[Route("api/sys/user/[action]")]
public class UserAppService : ApplicationService, IUserAppService
{
    public SysUserStore UserStore { get; }

    public IRepository<SysUser> UserRepository { get; }
    // public IRepository<OrganizationUnit> OrganizationUnitRepository { get; }
    private UserOutObjectMapper userObjectMapper { get; }
    public UserAppService(IRepository<SysUser> userRepository, SysUserStore userStore, UserOutObjectMapper userObjectMapper
        /*IRepository<OrganizationUnit> organizationUnitRepository*/)
    {
        UserRepository = userRepository;
        UserStore = userStore;
        this.userObjectMapper = userObjectMapper;
        // OrganizationUnitRepository = organizationUnitRepository;
    }
    
    [HttpGet]
    [ActionName("get")]
    public async Task<CommonResult<SysUserOutputWithRoleAndPosts>> GetAsync(string userId)
    {
        List<SysUser> users=new List<SysUser>();
            var user=await UserStore.FindByIdAsync(userId);
        users.Add(user);
        var userOutputs = ObjectMapper.Map<List<SysUser>, List<SysUserOutput>>(users);
        SysUserOutput userOutput = userObjectMapper.Map(user);
        var sysUserOutput = ObjectMapper.Map<SysUser, SysUserOutput>(user); 
        List<String> roleNames = await UserStore.GetRolesAsync(user) as List<string>;
        List<long> postIds = await UserStore.GetPostsByUserIdAsync(Guid.Parse(userId)); 
        // SysUserOutput userOutput = ObjectMapper.Map<SysUser, SysUserOutput>(user);
        return CommonResult<SysUserOutputWithRoleAndPosts>.Success(
             SysUserOutputWithRoleAndPosts.CreateInstance(userOutput,roleNames,postIds), "获取用户列表成功");
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
    // [HttpGet]
    // [ActionName("add")]
    // public Task<CommonResult<String>> CreateAsync(string userId)
    // {
    //     return Task.FromResult(CommonResult<String>.Success("", ""));
    // }



    [HttpGet]
    [ActionName("add")]
    public Task<CommonResult<String>> CreateAsync(UserListInput input)
    {
        return Task.FromResult(CommonResult<String>.Success("", "��ȡ�û��б�ɹ�"));
    }

    [HttpGet]
    [ActionName("edit")]
    public Task<CommonResult<string>> UpdateAsync(UserListInput input)
    {
        return Task.FromResult(CommonResult<String>.Success("", "��ȡ�û��б�ɹ�"));
    }

    [HttpGet]
    [ActionName("remove")]
    public Task<CommonResult<string>> DeleteAsync(long[] userIds)
    {
        return Task.FromResult(CommonResult<String>.Success("", "��ȡ�û��б�ɹ�"));
    }
}