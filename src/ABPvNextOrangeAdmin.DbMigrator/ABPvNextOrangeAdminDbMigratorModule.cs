using ABPvNextOrangeAdmin.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace ABPvNextOrangeAdmin.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ABPvNextOrangeAdminEntityFrameworkCoreModule),
    typeof(ABPvNextOrangeAdminApplicationContractsModule)
    )]
public class ABPvNextOrangeAdminDbMigratorModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AbpCommonDbProperties.DbTablePrefix = "sys_";
        ABPvNextOrangeAdminEfCoreEntityExtensionMappings.Configure();
    }
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
