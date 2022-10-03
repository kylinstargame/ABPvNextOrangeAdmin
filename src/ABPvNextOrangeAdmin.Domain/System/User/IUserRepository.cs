using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Roles;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity.Settings;

namespace ABPvNextOrangeAdmin.System.User;

public interface IUserRepository : IBasicRepository<SysUser, long>
{
    Task<SysUser> FindByNormalizedUserNameAsync(
        [NotNull] string normalizedUserName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<List<string>> GetRoleNamesAsync(
        long id,
        CancellationToken cancellationToken = default
    );

    Task<List<string>> GetRoleNamesInOrganizationUnitAsync(
        long id,
        CancellationToken cancellationToken = default);

    Task<SysUser> FindByLoginAsync(
        [NotNull] string loginProvider,
        [NotNull] string providerKey,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<SysUser> FindByNormalizedEmailAsync(
        [NotNull] string normalizedEmail,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<List<SysUser>> GetListByClaimAsync(
        Claim claim,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<List<SysUser>> GetListByNormalizedRoleNameAsync(
        string normalizedRoleName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    Task<List<SysUser>> GetListAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
        string userName = null,
        string phoneNumber = null,
        string emailAddress = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        CancellationToken cancellationToken = default
    );

    Task<List<SysRole>> GetRolesAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    // Task<List<IdentitySettingNames.OrganizationUnit>> GetOrganizationUnitsAsync(
    //     Guid id,
    //     bool includeDetails = false,
    //     CancellationToken cancellationToken = default);

    Task<List<SysUser>> GetUsersInOrganizationUnitAsync(
        Guid organizationUnitId,
        CancellationToken cancellationToken = default
    );

    Task<List<SysUser>> GetUsersInOrganizationsListAsync(
        List<Guid> organizationUnitIds,
        CancellationToken cancellationToken = default
    );

    Task<List<SysUser>> GetUsersInOrganizationUnitWithChildrenAsync(
        string code,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string filter = null,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
        string userName = null,
        string phoneNumber = null,
        string emailAddress = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        CancellationToken cancellationToken = default
    );
}