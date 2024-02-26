using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Config;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace ABPvNextOrangeAdmin.System.User;

public class UserDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public UserDataSeedContributor( IUserDataSeeder userDataSeeder)
    {
        UserDataSeeder = userDataSeeder;
    }

    protected IUserDataSeeder UserDataSeeder { get; }

    public const string AdminEmailPropertyName = "AdminEmail";
    public const string AdminEmailDefaultValue = "orangecardgame@163.com";
    public const string AdminEmailDefaultValue1 = "laoyaodie@163.com";
    public const string AdminPasswordPropertyName = "AdminPassword";
    public const string AdminPasswordDefaultValue = "1q2w3E*";

    public async Task SeedAsync(DataSeedContext context)
    {
        List<SysUser> sysUsers = new List<SysUser>();
        await UserDataSeeder.SeedAsync("admin", context?[AdminEmailPropertyName] as string ?? AdminEmailDefaultValue,
            "橙卡软件科技", "15803843236", /*context?[AdminPasswordPropertyName] as string ?? */AdminPasswordDefaultValue);
        
        await UserDataSeeder.SeedAsync("liliang", context?[AdminEmailPropertyName] as string ?? AdminEmailDefaultValue1,
            "橙卡李亮", "15890048221", /*context?[AdminPasswordPropertyName] as string ??*/ AdminPasswordDefaultValue);


    }
}