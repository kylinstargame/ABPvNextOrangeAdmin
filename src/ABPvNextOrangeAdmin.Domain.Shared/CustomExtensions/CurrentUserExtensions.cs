using Volo.Abp.Users;

namespace ABPvNextOrangeAdmin.CustomExtensions;

public static class CurrentUserExtensions
{
    public static bool IsAdmin(this ICurrentUser user)
    {
        return user.UserName.ToLower() == "admin";
    }
}