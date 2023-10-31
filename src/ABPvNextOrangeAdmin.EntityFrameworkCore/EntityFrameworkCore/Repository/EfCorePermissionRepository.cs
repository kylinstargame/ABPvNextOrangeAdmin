using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Menu;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ABPvNextOrangeAdmin.EntityFrameworkCore.Repository;

public class PermsssionRepository : EfCoreRepository<ABPvNextOrangeAdminDbContext, SysMenu, long>, IPermsssionRepository
{
    public IUserRepository _userRepository;

    public PermsssionRepository(IDbContextProvider<ABPvNextOrangeAdminDbContext> dbContextProvider,
        IUserRepository userRepository) : base(
        dbContextProvider)
    {
        _userRepository = userRepository;
    }

    public async Task<List<string>> GetAllRolePermssionsAsync(long roleId)
    {
        var dbContext = await GetDbContextAsync();
        var Perms = from menu in dbContext.Set<SysMenu>()
            join RoleMenu in dbContext.Set<SysRoleMenu>() on menu.Id equals RoleMenu.MenuId
            where RoleMenu.RoleId == roleId
            select menu.Perms;
        return Perms.ToList();
    }


    public async Task<List<string>> GetAllUserPermssionsAysnc(Guid userId)
    {
        List<String> perms = new List<String>();
        var dbContext = await GetDbContextAsync();
        var users = from u in dbContext.Set<SysUser>() where Equals(u.Id, userId) select u;
        var user = await users.FirstAsync();
        if (user.IsAdmin())
        {
            perms.Add("*:*:*");
        }

        return perms;
    }
}