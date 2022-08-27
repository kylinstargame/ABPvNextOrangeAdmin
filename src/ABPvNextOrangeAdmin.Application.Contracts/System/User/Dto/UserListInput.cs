using System;
using System.Drawing.Printing;

namespace ABPvNextOrangeAdmin.System.User.Dto;

public class UserListInput : PagedInput
{

    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public Boolean Status { get; set; }
    public string DeptId { get; set; }

    public DateTime? BeginTime { get; set; }

    public DateTime? EndTime { get; set; }
    
}