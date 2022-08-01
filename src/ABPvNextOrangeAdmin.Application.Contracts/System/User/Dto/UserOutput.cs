using System;
using System.Collections.Generic;

namespace ABPvNextOrangeAdmin.System.User.Dto;

public class UserOutput
{
    public Guid UserId {get; set; }
    public String UserName {get; set; }
    public String NickName {get; set; }
    public String Password {get; set; }
    public String PhoneNumber {get; set; }
    public String Email {get; set; }
    public String Sex {get; set; }
    public String Status {get; set; }
    public String Remark {get; set; }
    public List<Guid> PostIds {get; set; }
    public List<Guid> RoleIds {get; set; }

}