using System;
using System.ComponentModel.DataAnnotations;
using IdentityServer4.Models;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.Config;

public class Config: FullAuditedEntity<Guid>
{
    /// <summary>
    /// 参数名称
    /// </summary>
    [Required]
    public string ConfigName { get; set; }

    /// <summary>
    /// 参数键名
    /// </summary>
    [Required]
    public string ConfigKey { get; set; }

    /// <summary>
    /// 参数键值
    /// </summary>
    [Required]
    public string ConfigValue { get; set; }

    /// <summary>
    /// 系统内置 Y=是,N=否
    /// </summary>
    public string ConfigType { get; set; }

    public Config()
    {
        
    }

    public Config(string configKey)
    {
        this.ConfigKey = configKey;
    }
}