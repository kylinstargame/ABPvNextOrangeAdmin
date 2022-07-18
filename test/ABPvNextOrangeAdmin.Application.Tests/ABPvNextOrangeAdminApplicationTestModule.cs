using Volo.Abp.Modularity;

namespace ABPvNextOrangeAdmin;

[DependsOn(
    typeof(ABPvNextOrangeAdminApplicationModule),
    typeof(ABPvNextOrangeAdminDomainTestModule)
    )]
public class ABPvNextOrangeAdminApplicationTestModule : AbpModule
{

}
