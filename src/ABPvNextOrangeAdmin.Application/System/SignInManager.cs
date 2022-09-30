using ABPvNextOrangeAdmin.System.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace ABPvNextOrangeAdmin.System;

public class SignInManager : SignInManager<SysUser>, ITransientDependency
{
    public SignInManager(UserManager userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<SysUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<SysUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<SysUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }
}