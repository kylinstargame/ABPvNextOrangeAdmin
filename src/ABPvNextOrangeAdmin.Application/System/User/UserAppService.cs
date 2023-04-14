using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.User.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace ABPvNextOrangeAdmin.System.User;

[Authorize]
[Route("api/sys/user/[action]")]
public class UserAppService : ApplicationService, IUserAppService
{
    public IRepository<SysUser> UserRepository { get; }
    // public IRepository<OrganizationUnit> OrganizationUnitRepository { get; }

    public UserAppService(IRepository<SysUser> userRepository
        /*IRepository<OrganizationUnit> organizationUnitRepository*/)
    {
        UserRepository = userRepository;
        // OrganizationUnitRepository = organizationUnitRepository;
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


    [HttpGet]
    [ActionName("add")]
    public Task<CommonResult<String>> CreateAsync(UserListInput input)
    {
        return Task.FromResult(CommonResult<String>.Success("", "��ȡ�û��б�ɹ�"));
    }

    [HttpGet]
    [ActionName("update")]
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