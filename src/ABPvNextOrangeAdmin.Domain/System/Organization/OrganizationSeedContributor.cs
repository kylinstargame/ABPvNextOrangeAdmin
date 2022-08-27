using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.Organization;

public class OrganizationSeedContributor : IDataSeedContributor, ITransientDependency
{
    private IGuidGenerator _guidGenerator;
    private IOrganizationUnitRepository OrganizationUnitRepository;
    private IUnitOfWorkManager _unitOfWorkManager;
    OrganizationUnitManager _organizationUnitManager;

    public OrganizationSeedContributor(IGuidGenerator guidGenerator, OrganizationUnitManager organizationUnitManager,
        IRepository<OrganizationUnit> organizationUnitRepository,
        IOrganizationUnitRepository organizationUnitRepository1, IUnitOfWorkManager unitOfWorkManager)
    {
        _guidGenerator = guidGenerator;
        _organizationUnitManager = organizationUnitManager;
        OrganizationUnitRepository = organizationUnitRepository1;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        List<OrganizationUnit> organizationUnits = new List<OrganizationUnit>();

        var rootOgranization = new OrganizationUnit(_guidGenerator.Create(), "橙卡科技");
        organizationUnits.Add(rootOgranization);
        var shenzhenOrganization = new OrganizationUnit(_guidGenerator.Create(), "深圳总公司", rootOgranization.Id);
        organizationUnits.Add(shenzhenOrganization);
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "研发部门", shenzhenOrganization.Id));
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "市场部门", shenzhenOrganization.Id));
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "测试部门", shenzhenOrganization.Id));
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "财务部门", shenzhenOrganization.Id));
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "运维部门", shenzhenOrganization.Id));

        var changshaOrganization = new OrganizationUnit(_guidGenerator.Create(), "长沙总公司", rootOgranization.Id);
        organizationUnits.Add(changshaOrganization);
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "研发部门", changshaOrganization.Id));
        organizationUnits.Add(new OrganizationUnit(_guidGenerator.Create(), "市场部门", changshaOrganization.Id));

        foreach (var organizationUnit in organizationUnits)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                await _organizationUnitManager.CreateAsync(organizationUnit);
                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}