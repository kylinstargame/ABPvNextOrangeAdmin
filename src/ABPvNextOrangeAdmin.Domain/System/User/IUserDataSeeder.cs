using System;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Extensions;
using ABPvNextOrangeAdmin.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace ABPvNextOrangeAdmin.System.User;

public class UserDataSeedResult
{
    public bool CreatedAdminUser { get; set; }

    public bool CreatedAdminRole { get; set; }
}

public interface IUserDataSeeder
{
    Task<UserDataSeedResult> SeedAsync(
        string adminName,
        string adminEmail,
        string nickName,
        string adminPhone,
        string adminPassword,
        Guid? tenantId = null);
}

class UserDataSeeder : IUserDataSeeder, ITransientDependency
{
    public UserDataSeeder(UserManager userManager, IUserRepository userRepository, ICurrentTenant currentTenant,
        IOptions<IdentityOptions> identityOptions, IRoleRepository roleRepository, RoleManager roleManager)
    {
        UserManager = userManager;
        UserRepository = userRepository;
        CurrentTenant = currentTenant;
        IdentityOptions = identityOptions;
        RoleRepository = roleRepository;
        RoleManager = roleManager;
    }

    protected UserManager UserManager { get; }

    protected IUserRepository UserRepository { get; }

    protected RoleManager RoleManager { get; }

    protected IRoleRepository RoleRepository { get; }
    protected ICurrentTenant CurrentTenant { get; }

    protected IOptions<IdentityOptions> IdentityOptions { get; }

    public async Task<UserDataSeedResult> SeedAsync(string adminName, string adminEmail, string nickName,
        string adminPhone, string adminPassword,
        Guid? tenantId = null)
    {
        Check.NotNullOrWhiteSpace(adminEmail, nameof(adminEmail));
        Check.NotNullOrWhiteSpace(adminPassword, nameof(adminPassword));

        using (CurrentTenant.Change(tenantId))
        {
            await IdentityOptions.SetAsync();

            var result = new UserDataSeedResult();

            var adminUser = await UserRepository.FindByNormalizedUserNameAsync(
                adminName // LookupNormalizer.NormalizeName(adminUserName)
            );

            if (adminUser != null)
            {
                return result;
            }

            adminUser = new SysUser(
                adminName,
                adminEmail,
                nickName,
                adminPhone,
                adminPassword,
                tenantId
            );

            (await UserManager.CreateAsync(adminUser, adminPassword, false)).CheckErrors();
            result.CreatedAdminUser = true;

            //"admin" role
            const string adminRoleName = "admin";
            var adminRole =
                await RoleRepository.FindByNameAsync(adminRoleName);
            if (adminRole == null)
            {
                adminRole = new SysRole(
                    adminRoleName,
                    tenantId
                );

                (await RoleManager.CreateAsync(adminRole)).CheckErrors();
                result.CreatedAdminRole = true;
            }

            (await UserManager.AddToRoleAsync(adminUser, adminRoleName)).CheckErrors();

            return result;
        }
    }
}