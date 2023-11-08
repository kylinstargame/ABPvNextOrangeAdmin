using System;
using Volo.Abp.Identity.Settings;

namespace ABPvNextOrangeAdmin.System.User;

public class UseResetPasswordInput
{
    public UseResetPasswordInput(string password, Guid userId)
    {
        Password = password;
        UserId = userId;
    }

    public Guid UserId { get; set; }

    public string Password { get; set; }
}