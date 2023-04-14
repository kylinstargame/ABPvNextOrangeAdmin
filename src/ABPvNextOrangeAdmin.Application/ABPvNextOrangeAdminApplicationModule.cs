// using Volo.Abp.Account;

using ABPvNextOrangeAdmin.System.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
// using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace ABPvNextOrangeAdmin;

[DependsOn(
    typeof(ABPvNextOrangeAdminDomainModule),
    typeof(ABPvNextOrangeAdminApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class ABPvNextOrangeAdminApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ABPvNextOrangeAdminApplicationModule>();
        });
        context.Services.AddAssemblyOf<PasswordHasher<SysUser>>();
    }
}
