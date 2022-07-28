using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.Config;

public class ConfigDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<SysConfig> _configRepository;

    public ConfigDataSeedContributor(IRepository<SysConfig> configRepository)
    {
        _configRepository = configRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        List<SysConfig> sysConfigs = new List<SysConfig>();

        sysConfigs.Add(new SysConfig("主框架页-默认皮肤样式名称", "sys.index.skinName", "skin-blue", "Y",
            "蓝色 skin-blue、绿色 skin-green、紫色 skin-purple、红色 skin-red、黄色 skin-yellow"));
        sysConfigs.Add(new SysConfig("用户管理-账号初始密码", "sys.user.initPassword", "123456", "Y", "初始化密码 123456"));
        sysConfigs.Add(new SysConfig("主框架页-侧边栏主题", "sys.index.sideTheme", "theme-dark", "Y",
            "深色主题theme-dark，浅色主题theme-light"));
        sysConfigs.Add(
            new SysConfig("账号自助-验证码开关", "sys.account.captchaOnOff", "true", "Y", "是否开启验证码功能（true开启，false关闭）"));
        sysConfigs.Add(
            new SysConfig("账号自助-是否开启用户注册功能", "sys.account.registerUser", "false", "Y", "是否开启注册用户功能（true开启，false关闭）"));
        await _configRepository.InsertManyAsync(sysConfigs);
    }
}