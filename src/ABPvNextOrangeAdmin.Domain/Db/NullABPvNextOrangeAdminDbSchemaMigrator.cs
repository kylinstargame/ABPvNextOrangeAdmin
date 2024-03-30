using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ABPvNextOrangeAdmin.Data;

/* This is used if database provider does't define
 * IABPvNextOrangeAdminDbSchemaMigrator implementation.
 */
public class NullABPvNextOrangeAdminDbSchemaMigrator : IABPvNextOrangeAdminDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
