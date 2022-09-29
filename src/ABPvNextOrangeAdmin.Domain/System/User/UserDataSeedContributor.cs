using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Config;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.User;

public class UserDataSeedContributor  : IDataSeedContributor, ITransientDependency
{
    public UserDataSeedContributor(IRepository<SysUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public const string AdminEmailPropertyName = "AdminEmail";
    public const string AdminEmailDefaultValue = "orangecardgame@163.com";
    public const string AdminPasswordPropertyName = "AdminPassword";
    public const string AdminPasswordDefaultValue = "1q2w3E*";

    protected IRepository<SysUser> _userRepository { get; }
    
    public async Task SeedAsync(DataSeedContext context)
    {
        List<SysUser> sysUsers = new List<SysUser>();
        sysUsers.Add(new SysUser("admin",context?[AdminEmailPropertyName] as string ?? AdminEmailDefaultValue,
            context?[AdminPasswordPropertyName] as string ?? AdminPasswordDefaultValue));
        await _userRepository.InsertManyAsync(sysUsers);
    }
}