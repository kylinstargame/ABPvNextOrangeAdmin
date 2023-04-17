using System;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ABPvNextOrangeAdmin.Miscrosoft.Extensions.DependencyInjection;

public static class IdentityServiceCollectionExtensions
{
    public static IdentityBuilder AddIdentityDependency(this IServiceCollection services, Action<IdentityOptions> setupAction)
    {
        //AbpRoleManager
        services.TryAddScoped<RoleManager<SysRole>>();
        services.TryAddScoped(typeof(RoleManager<SysRole>),
            provider => provider.GetService(typeof(RoleManager<SysRole>)));

        //AbpUserManager
        services.TryAddScoped<UserManager>();
        services.TryAddScoped(typeof(UserManager<SysUser>),
            provider => provider.GetService(typeof(UserManager)));

        //AbpUserStore
        services.TryAddScoped<SysUserStore>();
        services.TryAddScoped(typeof(IUserStore<SysUser>),
            provider => provider.GetService(typeof(SysUserStore)));

        // //AbpRoleStore
        // services.TryAddScoped<SysRoleStore>();
        // services.TryAddScoped(typeof(IRoleStore<IdentityRole>),
        //     provider => provider.GetService(typeof(IdentityRoleStore)));

        return services
            .AddIdentityCore<SysUser>(setupAction)
            .AddRoles<SysRole>();
            // .AddClaimsPrincipalFactory<AbpUserClaimsPrincipalFactory>();
    }
}