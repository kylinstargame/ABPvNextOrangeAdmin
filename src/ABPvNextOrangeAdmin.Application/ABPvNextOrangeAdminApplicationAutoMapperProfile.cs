using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Config;
using ABPvNextOrangeAdmin.System.Config.Dto;
using ABPvNextOrangeAdmin.System.Menu;
using ABPvNextOrangeAdmin.System.Organization.Dto;
using ABPvNextOrangeAdmin.System.Permission.Dto;
using ABPvNextOrangeAdmin.System.User.Dto;
using AutoMapper;
using Volo.Abp.Identity;

namespace ABPvNextOrangeAdmin;

public class ABPvNextOrangeAdminApplicationAutoMapperProfile : Profile
{
    public ABPvNextOrangeAdminApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<SysMenu, MenuOutput>();
        CreateMap<SysConfig, ConfigOutput>();
        
        CreateMap<IdentityUser, UserOutput>();
        CreateMap<IdentityRole, RoleOutput>();
        CreateMap<OrganizationUnit, DeptOutput>();
    }
}
