using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Menu;
using ABPvNextOrangeAdmin.System.Roles;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace ABPvNextOrangeAdmin.System.User;

public  interface IPermsssionRepository :  ITransientDependency 
{
    public  Task<List<string>> GetAllRolePermssionsAsync(long roleId);
    
    
    public Task<List<string>> GetAllUserPermssionsAysnc(Guid userId);
}
//
// public class PermsssionRepository : IPermsssionRepository
// {
// }