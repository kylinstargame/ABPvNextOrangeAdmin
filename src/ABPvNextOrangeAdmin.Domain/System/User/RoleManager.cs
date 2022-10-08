using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;

namespace ABPvNextOrangeAdmin.System.User;

public class RoleManager : RoleManager<SysRole>, IDomainService
{
    public RoleManager(IRoleStore<SysRole> store, IEnumerable<IRoleValidator<SysRole>> roleValidators,
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<SysRole>> logger) : base(
        store, roleValidators, keyNormalizer, errors, logger)
    {
    }
}