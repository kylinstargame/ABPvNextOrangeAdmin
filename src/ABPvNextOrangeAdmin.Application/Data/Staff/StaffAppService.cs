using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.Dto;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Permission.Dto;
using ABPvNextOrangeAdmin.System.Roles;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.Data;

[Authorize]
[Route("api/data/staff/[action]")]
public class StaffAppService : ApplicationService, IStaffAppService
{
    public StaffAppService(IRepository<Staff> staffRepository)
    {
        StaffRepository = staffRepository;
    }

    public IRepository<Staff> StaffRepository { get; }

    [HttpGet]
    [AllowAnonymous]
    [ActionName("list")]
    public async Task<CommonResult<PagedResultDto<StaffOutput>>> GetListAsync(RoleListInput input)
    {
        var staffs = await StaffRepository.GetListAsync();
        var staffOutputs= ObjectMapper.Map<List<Staff>, List<StaffOutput>>(staffs);
        return CommonResult<PagedResultDto<StaffOutput>>.Success(
            new PagedResultDto<StaffOutput>(staffOutputs.Count, staffOutputs), "获取员工列表成功");
    }
    

    [HttpGet]
    [ActionName("get")]
    public Task<CommonResult<SysRoleOutput>> GetAsync(string staffId,String Name,int Year)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [ActionName("add")]
    public async Task<CommonResult<string>> CreateAsync(StaffUpdateInutput input)
    {
        var staff = ObjectMapper.Map<StaffUpdateInutput, Staff>(input);
       await StaffRepository.InsertAsync(staff);
       return CommonResult<String>.Success(
           "获取员工列表成功" ,"获取员工列表成功");
    }

    [HttpGet]
    [ActionName("update")]
    public Task<CommonResult<string>> UpdateAsync(StaffUpdateInutput input)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet]
    [ActionName("delete")]
    public Task<CommonResult<string>> DeleteAsync(long staffId)
    {
        throw new NotImplementedException();
    }
}