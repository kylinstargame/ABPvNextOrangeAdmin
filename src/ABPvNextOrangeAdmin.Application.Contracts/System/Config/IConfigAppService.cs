using System;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Config.Dto;
using ABPvNextOrangeAdmin.System.Dict.Dto;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System.Config;

public interface IConfigAppService
{
    /// <summary>
    /// 根据字典类型查询字典数据信息
    /// </summary>
    /// <param name="dictType"></param>
    /// <returns></returns>
    public  Task<CommonResult<ConfigOutput>>  GetConfigKeyAsync(String configKey);
}