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
        // StaffPhotosRepository = staffPhotosRepository;
    }

    public IRepository<Staff> StaffRepository { get; }
    public IRepository<StaffPhotos> StaffPhotosRepository { get; }

    [HttpGet]
    [AllowAnonymous]
    [ActionName("list")]
    public async Task<CommonResult<PagedResultDto<StaffOutput>>> GetListAsync(StaffListInput input)
    { var staffsQueryable = (await StaffRepository.WithDetailsAsync(a => a.Photos))
            .WhereIf(!input.Name.IsNullOrEmpty(),
                x => x.Name.Contains(input.Name)||Pinyin.GetPinyin(input.Name).StartsWith(input.Name))
            .WhereIf(input.Years != 0, x => x.Years == input.Years)
            .WhereIf(!input.Dept.IsNullOrEmpty(), x => x.Dept.Contains(input.Dept));
        var staffs = staffsQueryable.ToList();
        var staffOutputs = ObjectMapper.Map<List<Staff>, List<StaffOutput>>(staffs);
        return CommonResult<PagedResultDto<StaffOutput>>.Success(
            new PagedResultDto<StaffOutput>(staffOutputs.Count, staffOutputs), "获取员工列表成功");
    }


    [HttpPost]
    [ActionName("get")]
    [AllowAnonymous]
    public async Task<CommonResult<StaffOutput>> GetAsync(long staffId)
    {
        var staffQueryable = await StaffRepository.WithDetailsAsync(x => x.Photos);
        var staff = staffQueryable.ToList().Find(X => X.Id == staffId);
        var staffOutpus = ObjectMapper.Map<Staff, StaffOutput>(staff);
        return CommonResult<StaffOutput>.Success(staffOutpus, "获取员工信息成功");
    }

    [HttpPost]
    [ActionName("add")]
    [AllowAnonymous]
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
    [AllowAnonymous]
    public async Task<CommonResult<String>> UpdateAsync(StaffUpdateInutput input)
    {
        var staff = ObjectMapper.Map<StaffUpdateInutput, Staff>(input);
        // var photos = await StaffPhotosRepository.GetListAsync(x => x.StaffId == input.Id);
        // await StaffPhotosRepository.DeleteManyAsync(photos);
        var oldstaff = await StaffRepository.FindAsync(x => x.Id == input.Id);
        oldstaff.Name = staff.Name;
        oldstaff.Years = staff.Years;
        oldstaff.Dept = staff.Dept;
        oldstaff.Photos = staff.Photos;
        oldstaff.Video = staff.Video;
        oldstaff.Remark = staff.Remark;
        oldstaff.signature = staff.signature;


        // oldstaff.LastModificationTime = null;
        await StaffRepository.UpdateAsync(oldstaff);

        return CommonResult<String>.Success(null, "获取员工信息更新成功");
    }

    [HttpPost]
    [ActionName("delete")]
    public async Task<CommonResult<string>> DeleteAsync(long staffId)
    {
        await StaffRepository.DeleteAsync(x => x.Id == staffId);

        return CommonResult<string>.Success("", "员工删除完成");
    }
}