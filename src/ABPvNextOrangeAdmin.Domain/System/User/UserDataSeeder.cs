using System;
using System.Threading;
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
    public UserDataSeeder(UserManager userStore, SysRoleManager sysRoleManager, IUserRepository userRepository,
        IRoleRepository roleRepository, ICurrentTenant currentTenant)
    {
        UserStore = userStore;
        UserRepository = userRepository;
        CurrentTenant = currentTenant;
        RoleRepository = roleRepository;
        SysRoleManager = sysRoleManager;
    }

    protected UserManager UserStore { get; }

    protected IUserRepository UserRepository { get; }

    protected SysRoleManager SysRoleManager { get; }

    protected IRoleRepository RoleRepository { get; }
    protected ICurrentTenant CurrentTenant { get; }


    public async Task<UserDataSeedResult> SeedAsync(string adminName, string adminEmail, string nickName,
        string adminPhone, string adminPassword,
        Guid? tenantId = null)
    {
        //清空所有配置
        
        // await UserRepository.HardDeleteAsync(await UserRepository.GetListAsync());
        
        Check.NotNullOrWhiteSpace(adminEmail, nameof(adminEmail));
        Check.NotNullOrWhiteSpace(adminPassword, nameof(adminPassword));

        using (CurrentTenant.Change(tenantId))
        {
            var result = new UserDataSeedResult();

            var adminUser = await UserRepository.FindByNormalizedUserNameAsync(
                adminName // LookupNormalizer.NormalizeName(adminUserName)
            );

            if (adminUser != null)
            {
                return result;
            }

            adminUser = new SysUser(
                Guid.Empty, adminName,
                adminEmail,
                nickName,
                adminPhone,
                adminPassword,
                tenantId
            );
            (await UserStore.CreateAsync(adminUser, adminPassword, false)).CheckErrors();
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

                (await SysRoleManager.CreateAsync(adminRole)).CheckErrors();
                result.CreatedAdminRole = true;
            }

            (await UserStore.AddToRoleAsync(adminUser, adminRoleName)).CheckErrors();

            return result;
        }
    }
}