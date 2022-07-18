using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace ABPvNextOrangeAdmin.Web.Pages;

public class IndexModel : ABPvNextOrangeAdminPageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
