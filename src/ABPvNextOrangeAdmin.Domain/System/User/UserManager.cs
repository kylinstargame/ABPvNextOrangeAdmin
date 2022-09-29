using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace ABPvNextOrangeAdmin.System.User;

public class UserManager : UserManager<SysUser>, IDomainService
{
    public UserManager(IUserStore<SysUser> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<SysUser> passwordHasher, IEnumerable<IUserValidator<SysUser>> userValidators,
        IEnumerable<IPasswordValidator<SysUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<SysUser>> logger) : base(store,
        optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }
    
    public virtual async Task<SysUser> GetByIdAsync(Guid id)
    {
        var user = await Store.FindByIdAsync(id.ToString(), CancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException(typeof(SysUser), id);
        }

        return user;
    }
}