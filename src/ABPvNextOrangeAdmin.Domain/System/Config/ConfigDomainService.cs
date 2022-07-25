using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Utils;
using Microsoft.Extensions.Logging;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ABPvNextOrangeAdmin.System.Config;

public class ConfigDomainService : DomainService
{
    private readonly IRepository<SysConfig, Guid> _configRepository;

    private readonly IDistributedCache<String> _distributedCache;

    public ConfigDomainService(IRepository<SysConfig, Guid> configRepository, IDistributedCache<String> distributedCache)
    {
        _configRepository = configRepository;
        _distributedCache = distributedCache;
    }

    //加载所有配置哦
    public async Task LoadingConfigCache(CancellationToken cancellationToken = default)
    {
        List<SysConfig> configs = await _configRepository.GetListAsync();
        foreach (SysConfig config in configs)
        {
            _distributedCache.GetOrAdd(config.ConfigKey, () => { return config.ConfigValue; });
        }
    }

    /// <summary>
    /// 查询参数配置信息(从数据库)
    /// </summary>
    /// <param name="configId"></param>
    /// <returns></returns>
    public async Task<SysConfig> SelectConfigById(Guid configId)
    {
        var config = await _configRepository.GetAsync(x => x.Id == configId);
        return config;
    }

    /// <summary>
    /// 查询参数配置信息(从缓存)
    /// </summary>
    /// <param name="configId"></param>
    /// <returns></returns>
    public async Task<string> SelectConfigByKey(String configKey)
    {
        //从缓存中获取配置
        var configValue = await _distributedCache.GetAsync(configKey);
        if (!String.IsNullOrEmpty(configValue))
        {
            return configValue;
        }
        else
        {
            //从数据库获取配置
            try
            {
                var config = await _configRepository.GetAsync(x => x.ConfigKey == configKey).ConfigureAwait(false);
                if (!String.IsNullOrEmpty(configValue))
                {
                    // 将配置放入缓存
                    await _distributedCache.SetAsync(config.ConfigKey, config.ConfigValue);
                    return config.ConfigValue;
                }
            }
            catch (EntityNotFoundException e)
            {
                Debug.Assert(e.Message != null, (string)"e.Message != null");
                Logger.LogWarning(e.Message);
                return  String.Empty;
            }
            return  String.Empty;
        }
    }

    public async Task<bool> SelectCaptchaOnOff()
    {
        String captchaOnOff = await SelectConfigByKey("sys.account.captchaOnOff");
        if (String.IsNullOrEmpty(captchaOnOff))
        {
            return true;
        }
        return ConvertUtils.ToBool(captchaOnOff);
    }
}