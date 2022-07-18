using ABPvNextOrangeAdmin.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ABPvNextOrangeAdmin.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ABPvNextOrangeAdminController : AbpControllerBase
{
    protected ABPvNextOrangeAdminController()
    {
        LocalizationResource = typeof(ABPvNextOrangeAdminResource);
    }
}
