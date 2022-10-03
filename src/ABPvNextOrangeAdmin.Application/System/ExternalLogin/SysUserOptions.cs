using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.ExternalLogin;
using JetBrains.Annotations;
using ExternalLoginProviderInfo = ABPvNextOrangeAdmin.System.ExternalLogin.ExternalLoginProviderInfo;

namespace ABPvNextOrangeAdmin.System;

public class SysUserOptions 
{
    public Dictionary<string, ExternalLoginProviderInfo> ExternalLoginProviders { get; }

    public SysUserOptions()
    {
        ExternalLoginProviders = new Dictionary<string, ExternalLoginProviderInfo>();
    } 
    
    public void Add<TProvider>([NotNull] string name)
        where TProvider : IExternalLoginProvider
    { 
        ExternalLoginProviders[name] =new ExternalLoginProviderInfo(name, typeof(TProvider));
    }
}