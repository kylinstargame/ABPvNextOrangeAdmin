using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace ABPvNextOrangeAdmin.System;

public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<SysUser, SysRole>,
    ITransientDependency
{
    public UserClaimsPrincipalFactory(UserManager userManager, SysRoleManager sysRoleManager, IOptions<IdentityOptions> options) 
        : base(userManager, sysRoleManager, options)
    {
    }
}