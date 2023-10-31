using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.Dict;

public sealed class SysDictData : FullAuditedEntity<long>
{
    public SysDictData(string remark)
    {
        Remark = remark;
    }

    public SysDictData(long id, long dictSort, string dictLabel, string dictValue, string dictType, string cssClass, string listClass, string isDefault, string remark, string status="0")
    {
        Id = id;
        DictSort = dictSort;
        DictLabel = dictLabel;
        DictValue = dictValue;
        DictType = dictType;
        CssClass = cssClass;
        ListClass = listClass;
        IsDefault = isDefault;
        Status = status;
        Remark = remark;
    }

    [Display(Description = "字典排序")] public long DictSort { get; set; }

    [Display(Description = "字典标签")] public String DictLabel { get; set; }

    [Display(Description = "字典键值")] public String DictValue { get; set; }

    [Display(Description = "字典标签")] public String DictType { get; set; }

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
    /// 注释 
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 状态（0正常 1停用）
    /// </summary>
    public string Status { get; set; }
}