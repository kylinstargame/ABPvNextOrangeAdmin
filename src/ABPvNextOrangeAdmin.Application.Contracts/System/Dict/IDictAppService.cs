using System;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Dict.Dto;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System;

public interface IDictAppService
{
    /// <summary>
    /// 获取字典数据列表
    /// </summary>
    /// <param name="dataInput"></param>
    /// <returns></returns>
    public Task<CommonResult<PagedResultDto<DictDataOutput>>> GetListAsync(DictDataInput dataInput);
    
    
    /// <summary>
    /// 根据字典类型查询字典数据信息
    /// </summary>
    /// <param name="dictCode"></param>
    /// <returns></returns>
    public Task<CommonResult<DictDataOutput>> GetInfoAsync(long dictCode);

    /// <summary>
    /// 根据字典类型查询字典数据信息
    /// </summary>
    /// <param name="dictType"></param>
    /// <returns></returns>
    public Task<CommonResult<PagedResultDto<DictDataOutput>>> GetDictTypeAsync(String dictType);


}