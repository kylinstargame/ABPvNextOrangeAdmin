using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityServer4.Models;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.Config;

[Table("sys_config")]
public class SysConfig: FullAuditedEntity<Guid>
{
    /// <summary>
    /// 参数键名
    /// </summary>
    [Required]
    public string ConfigKey { get; set; }

    /// <summary>
    /// 参数名称
    /// </summary>
    [Required]
    public string ConfigName { get; set; }


    /// <summary>
    /// 参数键值
    /// </summary>
    [Required]
    public string ConfigValue { get; set; }

    /// <summary>
    /// 系统内置 Y=是,N=否
    /// </summary>
    public string ConfigType { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    public string Remark { get; set; }

    public SysConfig()
    {
    }

    public SysConfig(string configKey, string configName)
    {
        ConfigKey = configKey;
        ConfigName = configName;
    }

    public SysConfig(string configKey, string configName, string configValue, string configType,string remark)
    {
        ConfigKey = configKey;
        ConfigName = configName;
        ConfigValue = configValue;
        ConfigType = configType;
        Remark = remark;
    }
}