using System.Collections.Generic;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Config;
using ABPvNextOrangeAdmin.System.Config.Dto;
using ABPvNextOrangeAdmin.System.Dept;
using ABPvNextOrangeAdmin.System.Dict;
using ABPvNextOrangeAdmin.System.Dict.Dto;
using ABPvNextOrangeAdmin.System.Menu;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.Organization.Dto;
using ABPvNextOrangeAdmin.System.Permission.Dto;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
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

        CreateMap<SysUser, SysUserOutput>();
        CreateMap<SysRole, SysRoleOutput>();
        CreateMap<SysDept, SysDeptOutput>();
        CreateMap<SysDept, SysDeptTreeSelectOutput>().ForMember(a => a.Label,
            b => b.MapFrom(a => a.DeptName));
        CreateMap<SysDictData,DictDataOutput>();
    }
}