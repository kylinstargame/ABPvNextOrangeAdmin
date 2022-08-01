using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.System.Dict;

public sealed class SysDictType : FullAuditedEntity<long>
{
    public SysDictType()
    {
        
    }
    public SysDictType(long id, string dictName, string dictType, string status, string remark)
    {
        Id = id;
        DictName = dictName;
        DictType = dictType;
        Status = status;
        Remark = remark;
    }

    [Display(Description = "字典名称")] public string DictName { get; set; }

    /**  */
    [Display(Description = "字典类型")]
    public string DictType { get; set; }

    /** 状态（0正常 1停用） */
    [Display(Description = "状态 0=正常,1=停用")]
    public string Status { get; set; }

    public string Remark { get; set; }
}