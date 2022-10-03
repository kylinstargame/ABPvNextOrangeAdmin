using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Roles;

namespace ABPvNextOrangeAdmin.System.User;

public interface IRoleRepository
{
    Task<SysRole> FindByNormalizedNameAsync(
        string normalizedRoleName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<List<SysRole>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );
    Task<List<SysRole>> GetListAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default
    );

    Task<List<SysRole>> GetDefaultOnesAsync(
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default
    ); 
}