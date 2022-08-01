using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System;

public class DictDataInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 字典标签
    /// </summary>
    public String DictLabel { get; set; }
    
    /// <summary>
    /// 字典键值
    /// </summary>
    public String DictValue { get; set; }
    
    /// <summary>
    /// 字典标签
    /// </summary>
    public String DictType { get; set; }

    /// <summary>
    /// 样式属性（其他样式扩展） 
    /// </summary>
    public String CssClass { get; set; }

    /// <summary>
    /// 表格字典样式
    /// </summary>
    public String ListClass { get; set; }

    /// <summary>
    /// 是否默认（Y是 N否）
    /// </summary>
    public string IsDefault { get; set; }

    /// <summary>
    /// 状态（0正常 1停用）
    /// </summary>
    public string Status { get; set; }
}
