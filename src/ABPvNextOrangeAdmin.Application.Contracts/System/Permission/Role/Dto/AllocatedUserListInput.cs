using System;

namespace ABPvNextOrangeAdmin.System.Permission.Role.Dto;

public class AllocatedUserListInput:PagedInput
{
    public long RoleId { get; set; }
    public String UserName{ get; set; }
    public String Phonenumber { get; set; }
}