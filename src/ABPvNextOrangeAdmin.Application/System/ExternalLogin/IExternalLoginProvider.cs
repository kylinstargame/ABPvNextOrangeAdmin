using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.User;

namespace ABPvNextOrangeAdmin.System.ExternalLogin;

public interface IExternalLoginProvider
{
    /// <summary>
    ///  用户认证.
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="plainPassword">P密码r</param>
    /// <returns>True, indicates that this used has authenticated by this source</returns>
    Task<bool> TryAuthenticateAsync(string userName, string plainPassword);

    /// <summary>
    /// This method is called when a user is authenticated by this source but the user does not exists yet.
    /// So, the source should create the user and fill the properties.
    /// </summary>
    /// <param name="userName">User name</param>
    /// <param name="providerName">The name of this provider</param>
    /// <returns>Newly created user</returns>
    Task<SysUser> CreateUserAsync(string userName, string providerName);

    /// <summary>
    /// This method is called after an existing user is authenticated by this source.
    /// It can be used to update some properties of the user by the source.
    /// </summary>
    /// <param name="providerName">The name of this provider</param>
    /// <param name="user">The user that can be updated</param>
    Task UpdateUserAsync(SysUser user, string providerName);

    /// <summary>
    /// Return a value indicating whether this source is enabled.
    /// </summary>
    /// <returns></returns>
    Task<bool> IsEnabledAsync(); 
}