using System;
using Microsoft.Extensions.Configuration;

namespace ABPvNextOrangeAdmin.Config;

public static class CaptchaExtensions
{
    public static IConfigurationBuilder ConfigCaptcha(
         this IConfigurationBuilder builder,
        Action<CaptchaConfigSource> optionsAction)
    {
        return builder.Add(new CaptchaConfigSource());
    } 
}