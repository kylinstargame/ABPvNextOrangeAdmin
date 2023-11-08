using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ABPvNextOrangeAdmin.EntityFrameworkCore.Repository;

public class EfCoreIdentityRoleRepository: EfCoreRepository<ABPvNextOrangeAdminDbContext, SysRole, long>, IRoleRepository
{
    public EfCoreIdentityRoleRepository(IDbContextProvider<ABPvNextOrangeAdminDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<SysRole> FindByNameAsync(string roleName, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            // .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.RoleName == roleName, GetCancellationToken(cancellationToken));

    }



    public async Task<SysRole> FindByIdAsync(long roleId, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            // .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.Id== roleId, GetCancellationToken(cancellationToken));

    }

    public async Task<List<SysRole>> GetListAsync(string sorting = null, int maxResultCount = Int32.MaxValue,
        int skipCount = 0, string filter = null,
        bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            // .IncludeDetails(includeDetails)
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.RoleName.Contains(filter))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(SysRole.RoleName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<List<SysRole>> GetListAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(t => ids.Contains(t.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<List<SysRole>> GetDefaultOnesAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            // .IncludeDetails(includeDetails)
            .Where(r => r.IsDefault)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.RoleName.Contains(filter)) 
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }
}