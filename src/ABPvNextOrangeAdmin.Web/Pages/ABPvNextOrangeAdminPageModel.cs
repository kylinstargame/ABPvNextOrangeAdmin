using ABPvNextOrangeAdmin.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ABPvNextOrangeAdmin.Web.Pages;

public abstract class ABPvNextOrangeAdminPageModel : AbpPageModel
{
    protected ABPvNextOrangeAdminPageModel()
    {
        LocalizationResourceType = typeof(ABPvNextOrangeAdminResource);
    }
}
