using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;

namespace ABPvNextOrangeAdmin.System.User.Exstension;

public class PasswordHasher : PasswordHasher<SysUser>, ITransientDependency
{
    public override PasswordVerificationResult VerifyHashedPassword(SysUser user, string hashedPassword, string providedPassword)
    { 
        var result=base.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result;
    }
}