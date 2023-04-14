using System;
using ABPvNextOrangeAdmin.System.Dept;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.User;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ABPvNextOrangeAdmin.EntityFrameworkCore.Repository;

public class EfCoreDeptRepository : EfCoreRepository<ABPvNextOrangeAdminDbContext, SysDept, long>, IDeptRepository
{
    public EfCoreDeptRepository(IDbContextProvider<ABPvNextOrangeAdminDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}