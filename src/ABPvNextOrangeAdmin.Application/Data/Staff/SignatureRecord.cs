using System;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.Data
;

public class SignatureRecord:EntityDto<long>
{
    /// <summary>
    /// 寄语照片
    /// </summary>
    public String signature { get; set; }
    
    
    
}