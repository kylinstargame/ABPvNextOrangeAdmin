using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Dict.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ABPvNextOrangeAdmin.System.Dict;

[Authorize]
[Route("api/sys/dict/data/[action]")]
public class DictAppService : ApplicationService, IDictAppService
{
    public DictAppService(IRepository<SysDictData> dictRepository)
    {
        DictRepository = dictRepository;
    }

    public IRepository<SysDictData> DictRepository { get; private set; }

    [HttpGet]
    [ActionName("list")]
    public async Task<CommonResult<PagedResultDto<DictDataOutput>>> GetListAsync(DictDataInput dataInput)
    {
       var queryale = await DictRepository.GetQueryableAsync().ConfigureAwait(true);
       var dictDatas  = queryale.WhereIf(!String.IsNullOrEmpty(dataInput.DictType), x => x.DictType == dataInput.DictType)
           .WhereIf(!String.IsNullOrEmpty(dataInput.DictLabel), x => x.DictLabel.Contains(dataInput.DictLabel))
           .WhereIf(!String.IsNullOrEmpty(dataInput.Status), x=>x.Status == dataInput.Status).OrderBy(dataInput.Sorting)
           .PageBy(dataInput.SkipCount,dataInput.MaxResultCount).ToList();
       var dictOutputs = ObjectMapper.Map<List<SysDictData>, List<DictDataOutput>>(dictDatas);
       return CommonResult<PagedResultDto<DictDataOutput>>.Success(
           new PagedResultDto<DictDataOutput>(dictOutputs.Count(), dictOutputs), "获取用户列表成功");
       
    }
    
    [HttpGet]
    [ActionName("{dictCode}")]
    public async Task<CommonResult<DictDataOutput>> GetInfoAsync([FromRoute] long id)
    {
        var queryale = await DictRepository.GetQueryableAsync().ConfigureAwait(true);
        SysDictData dictData = await DictRepository.GetAsync(x => x.Id == id); 
        
        var dictOutput = ObjectMapper.Map<SysDictData, DictDataOutput>(dictData);
        return CommonResult<DictDataOutput>.Success(
            dictOutput, "获取用户列表成功");
    }

    [HttpGet]
    [ActionName("type/{dictType}")]
    public async Task<CommonResult<PagedResultDto<DictDataOutput>>> GetDictTypeAsync([FromRoute] String dictType)
    {
        var queryale = await DictRepository.GetQueryableAsync().ConfigureAwait(true);
        var dictDatas = queryale.Where(x => x.DictType == dictType && x.Status == "0").ToList(); 
        
        var dictOutputs = ObjectMapper.Map<List<SysDictData>, List<DictDataOutput>>(dictDatas);
        return CommonResult<PagedResultDto<DictDataOutput>>.Success(
            new PagedResultDto<DictDataOutput>(dictOutputs.Count(), dictOutputs), "获取用户列表成功");
    }
}