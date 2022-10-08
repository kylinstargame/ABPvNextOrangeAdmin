﻿using ABPvNextOrangeAdmin.Miscrosoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ABPvNextOrangeAdmin.MultiTenancy;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
// using Volo.Abp.PermissionManagement.Identity;
// using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace ABPvNextOrangeAdmin;

[DependsOn(
    typeof(ABPvNextOrangeAdminDomainSharedModule),
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpBackgroundJobsDomainModule),
    typeof(AbpFeatureManagementDomainModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpTenantManagementDomainModule),
    typeof(AbpEmailingModule)
)]
public class ABPvNextOrangeAdminDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpMultiTenancyOptions>(options => { options.IsEnabled = MultiTenancyConsts.IsEnabled; });

#if DEBUG
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif

        // Configure<AbpDistributedEntityEventOptions>(options =>
        // {
        //     options.AutoEventSelectors.Add<SysUser>();
        //     options.AutoEventSelectors.Add<SysRole>();
        // });
        
        //添加自定义Identity依赖
        context.Services.AddIdentityDependency(options =>
        {
            options.User.RequireUniqueEmail = true;
        });
    }
}