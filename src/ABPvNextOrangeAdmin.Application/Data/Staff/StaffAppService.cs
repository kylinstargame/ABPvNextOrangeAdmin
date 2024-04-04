using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using NPinyin;

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
    public async Task<CommonResult<PagedResultDto<StaffOutput>>> GetListAsync(StaffListInput input)
    {
        var staffs = (await StaffRepository.WithDetailsAsync(a=>a.Photos))
            .WhereIf(!input.Name.IsNullOrEmpty(),x=>Pinyin.GetPinyin(x.Name).StartsWith(input.Name)||x.Name.Contains(input.Name))
            .WhereIf(input.Years!=0,x=>x.Years==input.Years)
            .WhereIf(!input.Dept.IsNullOrEmpty(),x=>x.Dept.Contains(input.Dept))
            .ToList();
        var staffOutputs = ObjectMapper.Map<List<Staff>, List<StaffOutput>>(staffs);
        return CommonResult<PagedResultDto<StaffOutput>>.Success(
            new PagedResultDto<StaffOutput>(staffOutputs.Count, staffOutputs), "获取员工列表成功");
    }


    [HttpPost]
    [ActionName("get")]
    public async Task<CommonResult<StaffOutput>> GetAsync(long staffId)
    {
        var staffQueryable = await StaffRepository.WithDetailsAsync(x => x.Photos);
       var staff =staffQueryable.ToList().Find(X=>X.Id == staffId);
        var staffOutpus = ObjectMapper.Map<Staff, StaffOutput>(staff);
        return CommonResult<StaffOutput>.Success(staffOutpus, "获取员工信息成功");
    }

    [HttpPost]
    [ActionName("add")]
    public async Task<CommonResult<string>> CreateAsync(StaffUpdateInutput input)
    {
        var staff = ObjectMapper.Map<StaffUpdateInutput, Staff>(input);
        var newStaff = await StaffRepository.InsertAsync(staff);
        if (newStaff != null)
        {
            staff.Photos = StaffPhotos.CreateInstances(newStaff.Id, input.Photos.ToArray());
            return CommonResult<String>.Success(
                "获取员工列表成功", "获取员工列表成功");
        }


        return CommonResult<String>.Failed("获取员工列表失败");
    }

    [HttpPost]
    [ActionName("update")]
    public Task<CommonResult<string>> UpdateAsync(StaffUpdateInutput input)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [ActionName("delete")]
    public async Task<CommonResult<string>> DeleteAsync(long staffId)
    {
        await StaffRepository.DeleteAsync(x => x.Id == staffId);

        return CommonResult<string>.Success("", "员工删除完成");
    }
}