using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.ExternalLogin;
using JetBrains.Annotations;
using ExternalLoginProviderInfo = ABPvNextOrangeAdmin.System.ExternalLogin.ExternalLoginProviderInfo;

namespace ABPvNextOrangeAdmin.System;


public class SignInOptions 
{
    public Dictionary<string, ExternalLoginProviderInfo> ExternalLoginProviders { get; }

    public SignInOptions()
    {
        ExternalLoginProviders = new Dictionary<string, ExternalLoginProviderInfo>();
    } 
    
    public void Add<TProvider>([NotNull] string name)
        where TProvider : IExternalLoginProvider
    { 
        ExternalLoginProviders[name] =new ExternalLoginProviderInfo(name, typeof(TProvider));
    }
}