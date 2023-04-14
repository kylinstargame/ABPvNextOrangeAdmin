using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.User.Exstension;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;


namespace ABPvNextOrangeAdmin.System.User;

public class UserManager : Microsoft.AspNetCore.Identity.UserManager<SysUser>, IDomainService

{
    public UserManager(IUserStore<SysUser> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<SysUser> passwordHasher, IEnumerable<IUserValidator<SysUser>> userValidators,
        IEnumerable<IPasswordValidator<SysUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IOptions<SysUserErrorDescriber> errors, IServiceProvider services, ILogger<UserManager> logger,
        IUserRepository userRepository, IRoleRepository roleRepository) : base(store, optionsAccessor, passwordHasher, userValidators,
        passwordValidators, keyNormalizer, errors.Value, services, logger)
    {
        UserRepository = userRepository;
        RoleRepository = roleRepository;
    }

    
    protected IUserRepository UserRepository { get; }

    protected IRoleRepository RoleRepository { get; }

    
    public virtual async Task<IdentityResult> CreateAsync(SysUser user, string password, bool validatePassword)
    {
        var result = await UpdatePasswordHash(user, password, validatePassword);
        if (!result.Succeeded)
        {
            return result;
        }

        return await CreateAsync(user);
    }
    public virtual async Task<SysUser> GetByIdAsync(Guid id)
    {
        SysUser user =  await Store.FindByIdAsync(id.ToString(), CancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException(typeof(SysUser), id);
        }

        return user;
    }
    
    public virtual async Task<SysUser> GetByNameAsync(String Name)
    {
        SysUser user =  await Store.FindByNameAsync(Name, CancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException(typeof(SysUser), Name);
        }

        return user;
    }

    
    public virtual async Task<IdentityResult> SetRolesAsync([NotNull] SysUser user,
        [NotNull] IEnumerable<string> roleNames)
    {
        Check.NotNull(user, nameof(user));
        Check.NotNull(roleNames, nameof(roleNames));

        var currentRoleNames = await GetRolesAsync(user);

        var result = await RemoveFromRolesAsync(user, currentRoleNames.Except(roleNames).Distinct());
        if (!result.Succeeded)
        {
            return result;
        }

        result = await AddToRolesAsync(user, roleNames.Except(currentRoleNames).Distinct());
        if (!result.Succeeded)
        {
            return result;
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> AddDefaultRolesAsync(SysUser user)
    {
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles, CancellationToken);

        foreach (var role in await RoleRepository.GetDefaultOnesAsync(cancellationToken: CancellationToken))
        {
            if (!user.IsInRole(role.Id))
            {
                user.AddRole(role.Id);
            }
        }
        return await UpdateUserAsync(user);
    }
}