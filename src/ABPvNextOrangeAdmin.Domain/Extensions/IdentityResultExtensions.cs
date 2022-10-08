using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Text.Formatting;

namespace ABPvNextOrangeAdmin.Extensions;

public static class IdentityResultExtensions
{
    private static readonly Dictionary<string, string> IdentityStrings = new Dictionary<string, string>();
    
    public static void CheckErrors(this IdentityResult identityResult)
    {
        if (identityResult.Succeeded)
        {
            return;
        }

        if (identityResult.Errors == null)
        {
            throw new ArgumentException("identityResult.Errors should not be null.");
        }

        throw new IdentityResultException(identityResult);
    }
    
    public static string LocalizeErrors(this IdentityResult identityResult, IStringLocalizer localizer)
    {
        if (identityResult.Succeeded)
        {
            throw new ArgumentException("identityResult.Succeeded should be false in order to localize errors.");
        }

        if (identityResult.Errors == null)
        {
            throw new ArgumentException("identityResult.Errors should not be null.");
        }

        return identityResult.Errors.Select(err => LocalizeErrorMessage(err, localizer)).JoinAsString(", ");
    }
    
    public static string LocalizeErrorMessage(this IdentityError error, IStringLocalizer localizer)
    {
        var key = $"Volo.Abp.Identity:{error.Code}";

        var localizedString = localizer[key];

        if (!localizedString.ResourceNotFound)
        {
            var englishString = IdentityStrings.GetOrDefault(error.Code);
            if (englishString != null)
            {
                if (FormattedStringValueExtracter.IsMatch(error.Description, englishString, out var values))
                {
                    return string.Format(localizedString.Value, values.Cast<object>().ToArray());
                }
                
            }
        }

        return localizer["Identity.Default"];
    }
    

    public static string[] GetValuesFromErrorMessage(this IdentityResult identityResult, IStringLocalizer localizer)
    {
        if (identityResult.Succeeded)
        {
            throw new ArgumentException(
                "identityResult.Succeeded should be false in order to get values from error.");
        }

        if (identityResult.Errors == null)
        {
            throw new ArgumentException("identityResult.Errors should not be null.");
        }

        var error = identityResult.Errors.First();
        var englishString = IdentityStrings.GetOrDefault(error.Code);

        if (englishString == null)
        {
            return Array.Empty<string>();
        }

        if (FormattedStringValueExtracter.IsMatch(error.Description, englishString, out var values))
        {
            return values;
        }

        return Array.Empty<string>();
    }
}

[Serializable]
public class IdentityResultException : BusinessException, ILocalizeErrorMessage
{
    public IdentityResult IdentityResult { get; }
    
    
    public IdentityResultException([NotNull] IdentityResult identityResult)
        : base(
            code: $"Volo.Abp.Identity:{identityResult.Errors.First().Code}",
            message: identityResult.Errors.Select(err => err.Description).JoinAsString(", "))
    {
        IdentityResult = Check.NotNull(identityResult, nameof(identityResult));
    }
    
    public string LocalizeMessage(LocalizationContext context)
    {
        var localizer = context.LocalizerFactory.Create<IdentityResource>();

        SetData(localizer);

        return IdentityResult.LocalizeErrors(localizer);
    }

    protected virtual void SetData(IStringLocalizer localizer)
    {
        var values = IdentityResult.GetValuesFromErrorMessage(localizer);

        for (var index = 0; index < values.Length; index++)
        {
            Data[index.ToString()] = values[index];
        }
    }
}