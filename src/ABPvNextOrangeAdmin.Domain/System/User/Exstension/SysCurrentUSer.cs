using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace ABPvNextOrangeAdmin.System.User.Exstension;

public class SysCurrentUser : CurrentUser, ITransientDependency
{


    public SysCurrentUser(ICurrentPrincipalAccessor principalAccessor) : base(principalAccessor)
    {
    }
}