using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;

namespace ABPvNextOrangeAdmin.System.User.Exstension;

public class PasswordHasher : PasswordHasher<SysUser>, ITransientDependency
{
}