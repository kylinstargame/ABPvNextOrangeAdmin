// using ABPvNextOrangeAdmin.System.Dept;
// using ABPvNextOrangeAdmin.System.Menu;
// using ABPvNextOrangeAdmin.System.Organization.Dto;
// using ABPvNextOrangeAdmin.System.Permission.Dto;
// using ABPvNextOrangeAdmin.System.Roles;
// using Volo.Abp.DependencyInjection;
// using Volo.Abp.ObjectMapping;
// using NotImplementedException = System.NotImplementedException;
//
// namespace ABPvNextOrangeAdmin.ObjectMapper;
//
// public class RoleObjectMapper: IObjectMapper<SysRoleUpdateInput, SysRole>, ITransientDependency
// {
//     public SysRole Map(SysRoleUpdateInput source)
//     {
//         var destination= new SysRole(source.RoleName);
//         destination.Permissions = source.roleKey;
//         destination.Order = source.roleSort;
//         destination.Status = source.Status;
//         destination.Remark= source.Remark;
//         foreach (var sourceMenuId in source.menuIds)
//         {
//             destination.RoleMenus.Add(new SysRoleMenu(sourceMenuId));
//         }
//
//     }
//
//     public SysRole Map(SysRoleUpdateInput source, SysRole destination)
//     {
//         destination.RoleName = source.RoleName;
//         destination.Permissions = source.roleKey;
//         destination.Order = source.roleSort;
//         destination.Status = source.Status;
//         destination.Remark= source.Remark;
//         foreach (var sourceMenuId in source.menuIds)
//         {
//             destination.Menus.Add(new SysMenu(sourceMenuId));
//         }
//         
//         return destination;
//     }
// }