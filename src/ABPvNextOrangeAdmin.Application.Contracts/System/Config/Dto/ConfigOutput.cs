using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System.Config.Dto;

public class ConfigOutput : EntityDto
{
    /// <summary>
    /// 参数键名
    /// </summary>
    public string ConfigKey { get; set; }

    /// <summary>
    /// 参数名称
    /// </summary>
    public string ConfigName { get; set; }


    /// <summary>
    /// 参数键值
    /// </summary>
    public string ConfigValue { get; set; }

    /// <summary>
    /// 系统内置 Y=是,N=否
    /// </summary>
    public string ConfigType { get; set; }

    /// <summary>
    /// 描述信息
    /// </summary>
    public string Remark { get; set; }
 
}