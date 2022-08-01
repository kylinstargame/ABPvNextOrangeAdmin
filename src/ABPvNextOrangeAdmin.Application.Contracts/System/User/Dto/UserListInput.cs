using System;
using System.Drawing.Printing;
using Volo.Abp.Application.Dtos;

namespace ABPvNextOrangeAdmin.System.User.Dto;

public class UserListInput : IPagedAndSortedResultRequest
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public Boolean Status { get; set; }
    public string DeptId { get; set; }
    
    public DateTime? BeginTime { get; set; }
    
    public DateTime? EndTime { get; set; }

    public int MaxResultCount { get; set; }
    public int SkipCount { get; set; }
    public string Sorting { get; set; }
}
