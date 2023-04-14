using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.User.Exstension;

public class LookupNormalizer: ILookupNormalizer,ITransientDependency
{
    public string NormalizeName(string name)
    {
        return name.ToUpper();
        // throw new NotImplementedException();
    }

    public string NormalizeEmail(string email)
    {
        return email.ToUpper();
        // throw new NotImplementedException();
    }
}