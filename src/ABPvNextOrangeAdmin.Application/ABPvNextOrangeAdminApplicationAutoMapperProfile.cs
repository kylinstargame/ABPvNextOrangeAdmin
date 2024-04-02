using System;
using System.Collections.Generic;
using ABPvNextOrangeAdmin.Data;

using ABPvNextOrangeAdmin.Dto;
using ABPvNextOrangeAdmin.System;
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
using Microsoft.AspNetCore.Hosting;
using Volo.Abp.Identity;
using SysRoleStore = ABPvNextOrangeAdmin.System.Roles.SysRoleStore;

namespace ABPvNextOrangeAdmin;

public class ABPvNextOrangeAdminApplicationAutoMapperProfile : Profile
{
    public ABPvNextOrangeAdminApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<SysMenu, SysMenuOutput>();
        CreateMap<SysConfig, ConfigOutput>();

        CreateMap<SysUser, SysUserOutput>();
         
        CreateMap<SysRoleUpdateInput, SysRole>().ForMember(a=>a.Permissions,
            b=>b.MapFrom(a=>a.RoleKey));
        CreateMap<SysRole, SysRoleOutput>().ForMember(a=>a.RoleKey,
                b=>b.MapFrom(a=>a.Permissions))
            .ForMember(a=>a.roleSort,
                b=>b.MapFrom(a=>a.Order));
       
  
        CreateMap<SysDept, SysDeptOutput>();
        CreateMap<SysPost, SysPostOutput>();
        CreateMap<SysDept, SysDeptTreeSelectOutput>().ForMember(a => a.Label,
            b => b.MapFrom(a => a.DeptName));
        CreateMap<SysMenu, SysMenuTreeSelectOutput>().ForMember(a => a.Label,
            b => b.MapFrom(a => a.MenuName));
        
        
        CreateMap<SysDictData,DictDataOutput>();
        CreateMap<StaffUpdateInutput, Staff>();
        CreateMap<Staff, StaffOutput>();
    }
}