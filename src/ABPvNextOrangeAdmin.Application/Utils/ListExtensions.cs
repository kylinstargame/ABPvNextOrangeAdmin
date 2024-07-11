using System.Collections.Generic;
using System.Linq;
using Volo.Abp;

namespace ABPvNextOrangeAdmin.System;

public static class ListExtensions
{
    public static IEnumerable<T> PageBy<T>(
        this  IEnumerable<T> query, PagedInput result)
       
    {
        Check.NotNull<IEnumerable<T> >(query, nameof(query));
        return query.Skip(result.SkipCount).Take(result.MaxResultCount);
    }
}