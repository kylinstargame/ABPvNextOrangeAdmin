using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace ABPvNextOrangeAdmin.System.Account.Dto;

public class LoginInput
{
    [Required]
    [StringLength(255)]
    public string UserNameOrEmailAddress { get; set; }

    [Required]
    [StringLength(32)]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string Password { get; set; }
    
    /// <summary>
    /// 校验码
    /// </summary>
    public string Code { get; set; }
    
    /// <summary>
    /// 校验码
    /// </summary>
    public string Uuid { get; set; }


    public bool RememberMe { get; set; } 
}