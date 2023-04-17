using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace ABPvNextOrangeAdmin.System.Dept;

public class DeptSeedContributor : IDataSeedContributor, ITransientDependency
{

    public DeptSeedContributor(IUnitOfWorkManager unitOfWorkManager, DeptManager deptManager)
    {
        UnitOfWorkManager = unitOfWorkManager;
        DeptManager = deptManager;
    }

    public DeptManager DeptManager { get; }

    public IUnitOfWorkManager UnitOfWorkManager { get; }


    public async Task SeedAsync(DataSeedContext context)
    {
        List<SysDept> organizationUnits = new List<SysDept>();
        
        

        var rootOgranization = new SysDept(1, "橙卡科技");
        organizationUnits.Add(rootOgranization);
        var shenzhenOrganization = new SysDept(11, "深圳总公司", rootOgranization.Id);
        organizationUnits.Add(shenzhenOrganization);
        organizationUnits.Add(new SysDept(111, "研发部门", shenzhenOrganization.Id));
        organizationUnits.Add(new SysDept(211, "市场部门", shenzhenOrganization.Id));
        organizationUnits.Add(new SysDept(311, "测试部门", shenzhenOrganization.Id));
        organizationUnits.Add(new SysDept(411, "财务部门", shenzhenOrganization.Id));
        organizationUnits.Add(new SysDept(511, "运维部门", shenzhenOrganization.Id));

        var changshaOrganization = new SysDept(21, "长沙总公司", rootOgranization.Id);
        organizationUnits.Add(changshaOrganization);
        organizationUnits.Add(new SysDept(121, "研发部门", changshaOrganization.Id));
        organizationUnits.Add(new SysDept(221, "市场部门", changshaOrganization.Id));

        foreach (var organizationUnit in organizationUnits)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                await DeptManager.CreateAsync(organizationUnit);
                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}