using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System.Organization.Dto;

public class SysPostOutput:EntityDto<long>
{
    public string PostName { get; set; }
    public string Code { get; set; }
}