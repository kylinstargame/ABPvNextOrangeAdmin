using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User.Exstension;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Services;

namespace ABPvNextOrangeAdmin.System.User;

public class SysRoleManager : RoleManager<SysRole>, IDomainService
{
    public SysRoleManager(SysRoleStore store, IEnumerable<IRoleValidator<SysRole>> roleValidators,
        ILookupNormalizer keyNormalizer, SysUserErrorDescriber errors, ILogger<RoleManager<SysRole>> logger)
        : base(store, roleValidators, keyNormalizer, errors, logger)
    {
    }
}