using System;
using System.Collections.Generic;
using System.Text;
using ABPvNextOrangeAdmin.Localization;
using Volo.Abp.Application.Services;

namespace ABPvNextOrangeAdmin;

/* Inherit your application services from this class.
 */
public abstract class ABPvNextOrangeAdminAppService : ApplicationService
{
    protected ABPvNextOrangeAdminAppService()
    {
        LocalizationResource = typeof(ABPvNextOrangeAdminResource);
    }
}
