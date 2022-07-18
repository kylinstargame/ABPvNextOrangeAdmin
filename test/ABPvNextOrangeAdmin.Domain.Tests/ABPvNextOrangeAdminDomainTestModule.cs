using ABPvNextOrangeAdmin.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ABPvNextOrangeAdmin;

[DependsOn(
    typeof(ABPvNextOrangeAdminEntityFrameworkCoreTestModule)
    )]
public class ABPvNextOrangeAdminDomainTestModule : AbpModule
{

}
