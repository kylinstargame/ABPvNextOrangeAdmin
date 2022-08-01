using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.Organization;

public class OrganizationSeedContributor : IDataSeedContributor, ITransientDependency
{
    private IGuidGenerator _guidGenerator;
    private IRepository<OrganizationUnit> _organizationUnitRepository;

    public OrganizationSeedContributor(IGuidGenerator guidGenerator, IRepository<OrganizationUnit> organizationUnitRepository)
    {
        this._guidGenerator = guidGenerator;
        this._organizationUnitRepository = organizationUnitRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        List<OrganizationUnit> organizationUnits = new List<OrganizationUnit>();
        var rootOgranization = new OrganizationUnit(_guidGenerator.Create(), "橙卡科技");
        organizationUnits.Add(rootOgranization);
        var shenzhenOgranization = new OrganizationUnit(_guidGenerator.Create(), "深圳总公司", rootOgranization.Id);
        organizationUnits.Add(shenzhenOgranization);
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "研发部门", shenzhenOgranization.Id));
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "市场部门", shenzhenOgranization.Id));
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "测试部门", shenzhenOgranization.Id));
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "财务部门", shenzhenOgranization.Id));
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "运维部门", shenzhenOgranization.Id));
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "财务部门", shenzhenOgranization.Id));
        
        var changshaOgranization = new OrganizationUnit(_guidGenerator.Create(), "长沙总公司", rootOgranization.Id);
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "研发部门", changshaOgranization.Id));
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "市场部门", changshaOgranization.Id));
        await _organizationUnitRepository.InsertManyAsync(organizationUnits);
    }
}