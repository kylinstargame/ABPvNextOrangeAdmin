using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Menu;
using AutoMapper;

namespace ABPvNextOrangeAdmin;

public class ABPvNextOrangeAdminApplicationAutoMapperProfile : Profile
{
    public ABPvNextOrangeAdminApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<SysMenu, MenuOutput>();
    }
}
