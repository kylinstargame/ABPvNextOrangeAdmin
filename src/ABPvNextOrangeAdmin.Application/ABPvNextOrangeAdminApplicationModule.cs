using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace ABPvNextOrangeAdmin;

[DependsOn(
    typeof(ABPvNextOrangeAdminDomainModule),
    typeof(AbpAccountApplicationModule),
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
    }
}
