using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace ABPvNextOrangeAdmin;

[Dependency(ReplaceServices = true)]
public class ABPvNextOrangeAdminBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "ABPvNextOrangeAdmin";
}
