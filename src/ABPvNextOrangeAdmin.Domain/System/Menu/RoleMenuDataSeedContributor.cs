using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using NotImplementedException = System.NotImplementedException;

namespace ABPvNextOrangeAdmin.System.Menu;

public class RoleMenuDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private IGuidGenerator _guidGenerator;
    private IRepository<IdentityRole> _roleRepository;
    private IRepository<SysRoleMenu> _roleMenuRepository;

    public RoleMenuDataSeedContributor(IRepository<SysRoleMenu> roleMenuRepository,
        IRepository<IdentityRole> roleRepository, IGuidGenerator guidGenerator)
    {
        _roleMenuRepository = roleMenuRepository;
        _roleRepository = roleRepository;
        _guidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        IdentityRole role = new IdentityRole(_guidGenerator.Create(), "common");
        await _roleRepository.InsertAsync(role);

        List<SysRoleMenu> sysRoleMenus = new List<SysRoleMenu>();
        sysRoleMenus.AddRange(SysRoleMenu.CreateInstances(role.Id, new[] { 1, 2, 3, 4 }));
        sysRoleMenus.AddRange(SysRoleMenu.CreateInstances(role.Id, new[]
        {
            100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116
        }));
        sysRoleMenus.AddRange(SysRoleMenu.CreateInstances(role.Id, new[]
        {
            500, 501,
        }));
        sysRoleMenus.AddRange(SysRoleMenu.CreateInstances(role.Id, new[]
        {
            1000, 1001,1002, 1003,1004, 1005,1006, 1007,1008, 1009,
            1010, 1011,1012, 1013,1014, 1015,1016, 1017,1018, 1019,
            1020, 1021,1022, 1023,1024, 1025,1026, 1027,1028, 1029,
            1030, 1031,1032, 1033,1034, 1035,1036, 1037,1038, 1039,
            1040, 1041,1042, 1043,1044, 1045,1046, 1047,1048, 1049,
            1050, 1051,1052, 1053,1054, 1055,1056, 1057,1058, 1059,
        }));

        await _roleMenuRepository.InsertManyAsync(sysRoleMenus);
    }
}