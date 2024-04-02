using System;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.Data
;

public class SignatureRecord:EntityDto<long>
{
    /// <summary>
    /// 签名照片
    /// </summary>
    public String signature { get; set; }
    
    
    
}