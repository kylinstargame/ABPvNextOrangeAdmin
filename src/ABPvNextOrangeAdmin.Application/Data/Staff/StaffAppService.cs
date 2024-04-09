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
using Volo.Abp.Uow;

namespace ABPvNextOrangeAdmin.Data;

[Authorize]
[Route("api/data/staff/[action]")]
public class StaffAppService : ApplicationService, IStaffAppService
{
    public StaffAppService(IRepository<Staff> staffRepository, IRepository<StaffPhotos> staffPhotosRepository,
        UnitOfWorkManager unitOfWorkManager)
    {
        StaffRepository = staffRepository;
        StaffPhotosRepository = staffPhotosRepository;
        UnitOfWorkManager = unitOfWorkManager;
    }

    public IRepository<Staff> StaffRepository { get; }
    public IRepository<StaffPhotos> StaffPhotosRepository { get; }
    public UnitOfWorkManager UnitOfWorkManager { get; }

    [HttpGet]
    [AllowAnonymous]
    [ActionName("list")]
    public async Task<CommonResult<PagedResultDto<StaffOutput>>> GetListAsync(StaffListInput input)
    {
        var staffsQueryable = (await StaffRepository.WithDetailsAsync(a => a.Photos))
            // .WhereIf(!input.Name.IsNullOrEmpty(),
            //     x => x.Name.Contains(input.Name) || Pinyin.GetPinyin(x.Name).StartsWith(input.Name))
            .WhereIf(input.Years != 0, x => x.Years == input.Years)
            .WhereIf(!input.Dept.IsNullOrEmpty(), x => x.Dept.Contains(input.Dept));
        var staffs = staffsQueryable.PageBy(input).ToList().WhereIf(!input.Name.IsNullOrEmpty(),
            x => x.Name.Contains(input.Name) || Pinyin.GetPinyin(x.Name).StartsWith(input.Name)).ToList();
        var staffOutputs = ObjectMapper.Map<List<Staff>, List<StaffOutput>>(staffs);
        return CommonResult<PagedResultDto<StaffOutput>>.Success(
            new PagedResultDto<StaffOutput>(staffsQueryable.Count(), staffOutputs), "获取员工列表成功");
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
        using (var unitOfWork = UnitOfWorkManager.Begin())
        {
            var staff = await StaffRepository.FindAsync(x => x.Id == input.Id);
            staff.Name = input.Name;
            staff.Years = input.Years;
            staff.Dept = input.Dept;
            staff.Video = input.Video;
            staff.Remark = input.Remark;
            staff.signature = input.signature;
            await StaffRepository.UpdateAsync(staff);
            var staffPhotos = await StaffPhotosRepository.GetListAsync(x => x.StaffId == input.Id);
            await StaffPhotosRepository.DeleteManyAsync(staffPhotos);
            await StaffPhotosRepository.InsertManyAsync(StaffPhotos.CreateInstances(staff.Id,input.Photos.ToArray()));
        }

      
        return CommonResult<String>.Success(null, "获取员工信息更新成功");
    }

    [HttpPost]
    [ActionName("delete")]
    public async Task<CommonResult<string>> DeleteAsync(long staffId)
    {
        using (var unitOfWork = UnitOfWorkManager.Begin())
        {
            await StaffRepository.DeleteAsync(x => x.Id == staffId);
            var staffPhotos = await StaffPhotosRepository.GetListAsync(x => x.StaffId == staffId);
            await StaffPhotosRepository.DeleteManyAsync(staffPhotos);
        }

        return CommonResult<string>.Success("", "员工删除完成");
    }
}