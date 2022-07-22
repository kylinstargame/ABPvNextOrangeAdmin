using System;
using Microsoft.Extensions.Configuration;

namespace ABPvNextOrangeAdmin.Config;

public static class CaptchaExtensions
{
    public static IConfigurationBuilder ConfigCaptcha(
         this IConfigurationBuilder builder)
    {
        return builder.Add(new CaptchaConfigSource());
    } 
}