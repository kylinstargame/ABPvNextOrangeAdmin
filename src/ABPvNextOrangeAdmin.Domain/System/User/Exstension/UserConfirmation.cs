using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.User.Exstension;

public class UserConfirmation : IUserConfirmation<SysUser>,ITransientDependency
{
    public Task<bool> IsConfirmedAsync(UserManager<SysUser> manager, SysUser user)
    {
        // throw new NotImplementedException();
        return Task.FromResult(false);
    }
}