using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ABPvNextOrangeAdmin.System.Menu;

public class MenuDomainService : DomainService
{
    private IRepository<SysMenu> _menuRepository;

    public MenuDomainService(IRepository<SysMenu> menuRepository)
    {
        _menuRepository = menuRepository;
    }

    // public Task<List<String>> GetMenuPermissions( Guid id)
    // {
    //     
    // }
}