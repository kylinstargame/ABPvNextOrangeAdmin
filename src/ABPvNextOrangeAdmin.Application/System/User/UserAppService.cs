using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.User.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace ABPvNextOrangeAdmin.System.User;

[Authorize]
[Microsoft.AspNetCore.Components.Route("api/sys/user/[action]")]
public class UserAppService : ApplicationService, IUserAppService
{
    public IRepository<IdentityUser> UserRepository { get; }
    public IRepository<OrganizationUnit> OrganizationUnitRepository { get; }

    public UserAppService(IRepository<IdentityUser> userRepository,
        IRepository<OrganizationUnit> organizationUnitRepository)
    {
        UserRepository = userRepository;
        OrganizationUnitRepository = organizationUnitRepository;
    }

     [HttpGet]
     [ActionName("list")]
     public async Task<CommonResult<PagedResultDto<UserOutput>>> GetListAsync(UserListInput input)
     {
         var queryale = await UserRepository.GetQueryableAsync();

         var identityUsers = queryale
             .WhereIf<IdentityUser, IQueryable<IdentityUser>>(input.UserId != Guid.Empty, x => x.Id == input.UserId)
             .WhereIf(String.IsNullOrWhiteSpace(input.UserName), x => x.UserName.Contains(input.UserName))
             .WhereIf(input.PhoneNumber != null, x => x.PhoneNumber == input.PhoneNumber)
             .WhereIf(String.IsNullOrWhiteSpace(input.DeptId),
                 x => OrganizationUnitRepository.GetQueryableAsync().Result.Select(x => x.Id).Any(input.DeptId))
             .Where(x => x.LockoutEnabled == input.Status)
             .WhereIf(input.BeginTime != null, x => x.CreationTime > input.BeginTime)
             .WhereIf(input.EndTime != null, x => x.CreationTime > input.EndTime)
             .PageBy(input.SkipCount, input.MaxResultCount).ToList();

         var userOutput = ObjectMapper.Map<List<IdentityUser>, List<UserOutput>>(identityUsers);
         return CommonResult<PagedResultDto<UserOutput>>.Success(
             new PagedResultDto<UserOutput>(identityUsers.Count(), userOutput), "获取用户列表成功");
     }

     [HttpGet]
     [ActionName("add")]
     public async Task<CommonResult<String>> CreateAsync(UserListInput input)
     {
         return CommonResult<String>.Success("", "获取用户列表成功");
     }

     [HttpGet]
     [ActionName("update")]
     public async Task<CommonResult<string>> UpdateAsync(UserListInput input)
     {
         return CommonResult<String>.Success("", "获取用户列表成功");
     }

     [HttpGet]
     [ActionName("remove")]
     public async Task<CommonResult<string>> DeleteAsync(long[] userIds)
     {
         return CommonResult<String>.Success("", "获取用户列表成功");
     }
}