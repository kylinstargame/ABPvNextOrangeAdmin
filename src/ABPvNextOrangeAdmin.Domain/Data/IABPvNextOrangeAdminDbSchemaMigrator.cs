using System.Threading.Tasks;

namespace ABPvNextOrangeAdmin.Data;

public interface IABPvNextOrangeAdminDbSchemaMigrator
{
    Task MigrateAsync();
}
