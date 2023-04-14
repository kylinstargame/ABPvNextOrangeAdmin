using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System.Organization.Dto;

public class SysDeptOutput : EntityDto<Guid>
{
    public Guid? ParentId { get; set; }
    public string Code { get; set; }
    public string DeptName { get; set; }
    public string Leader { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    
    public List<SysDeptOutput> Children { get; set; }
}