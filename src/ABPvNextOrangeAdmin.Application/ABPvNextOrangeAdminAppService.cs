using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text;
using ABPvNextOrangeAdmin.Localization;
using Microsoft.AspNetCore.Components;
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
