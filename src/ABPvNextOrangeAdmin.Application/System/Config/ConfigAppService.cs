using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Config.Dto;
using ABPvNextOrangeAdmin.System.Dict.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.Config;

[Authorize]
[Route("api/sys/config/[action]")]
public class ConfigAppService : ApplicationService, IConfigAppService
{
    public ConfigAppService(IRepository<SysConfig> configReposity)
    {
        ConfigReposity = configReposity;
    }

    private IRepository<SysConfig> ConfigReposity { get; }

    [HttpGet]
    [ActionName("configKey/{configKey}")]
    public async Task<CommonResult<ConfigOutput>> GetConfigKeyAsync([FromRoute] string configKey)
    {
       var sycConfig = await ConfigReposity.GetAsync(x=>x.ConfigKey == configKey);
       var config = ObjectMapper.Map<SysConfig, ConfigOutput>(sycConfig);
       return CommonResult<ConfigOutput>.Success(config,"获取配置成功");
    }
}