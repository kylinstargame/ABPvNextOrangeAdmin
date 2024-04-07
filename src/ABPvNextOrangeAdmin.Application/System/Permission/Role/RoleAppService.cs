using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Menu;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.Permission.Dto;
using ABPvNextOrangeAdmin.System.Permission.Role.Dto;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using NotImplementedException = System.NotImplementedException;
using SysRoleStore = ABPvNextOrangeAdmin.System.Roles.SysRoleStore;

namespace ABPvNextOrangeAdmin.System.Permission.Role;

[Authorize]
[Microsoft.AspNetCore.Mvc.Route("api/sys/role/[action]")]
public class RoleAppService : ApplicationService, IRoleAppService
{
    public RoleAppService(SysRoleStore roleStore, IRoleRepository roleRepository,
        IRepository<SysRoleMenu> roleMenuRepository, UnitOfWorkManager unitOfWorkManage,
        IRepository<SysRoleDept> roleDeptRepository, IRepository<SysMenu> menuRepository)
    {
        RoleStore = roleStore;

        RoleRepository = roleRepository;
        RoleMenuRepository = roleMenuRepository;
        UnitOfWorkManage = unitOfWorkManage;
        RoleDeptRepository = roleDeptRepository;
        MenuRepository = menuRepository;
    }

    public UnitOfWorkManager UnitOfWorkManage { get; set; }
    public SysRoleStore RoleStore { get; set; }

    private IRepository<SysMenu> MenuRepository { get; }

    private IRepository<SysRoleMenu> RoleMenuRepository { get; set; }
    private IRepository<SysRoleDept> RoleDeptRepository { get; set; }

    private IRoleRepository RoleRepository { get; }

    [HttpGet]
    [ActionName("list")]
    public async Task<CommonResult<PagedResultDto<SysRoleOutput>>> GetListAsync(RoleListInput input)
    {
        var sysRoles = await this.RoleRepository.GetListAsync();
        var roleOutputs = ObjectMapper.Map<List<SysRole>, List<SysRoleOutput>>(sysRoles);
        return CommonResult<PagedResultDto<SysRoleOutput>>.Success(
            new PagedResultDto<SysRoleOutput>((long)roleOutputs.Count, roleOutputs), "");
    }

    [HttpPost]
    [ActionName("get")]
    public async Task<CommonResult<SysRoleOutput>> GetAsync(string roleId)
    {
        var role = await RoleStore.FindByIdAsync(roleId);
        var roleOutput = ObjectMapper.Map<SysRole, SysRoleOutput>(role);

        return CommonResult<SysRoleOutput>.Success(
            roleOutput, "獲取角色信息成功");
    }


    [HttpPost]
    [ActionName("add")]
    public async Task<CommonResult<string>> CreateAsync(SysRoleUpdateInput input)
    {
        var role = ObjectMapper.Map<SysRoleUpdateInput, SysRole>(input);
        var newRole = await RoleRepository.InsertAsync(role, true);
        await RoleMenuRepository.InsertManyAsync(SysRoleMenu.CreateInstances(newRole.Id, input.menuIds.ToArray()),
            true);
        return CommonResult<string>.Success("", "角色添加完成");
    }

    [HttpPost]
    [ActionName("update")]
    public async Task<CommonResult<string>> UpdateAsync(SysRoleUpdateInput input)
    {
        var roleMenus = await RoleMenuRepository.GetListAsync(rm => rm.RoleId == input.Id);
        await RoleMenuRepository.HardDeleteAsync(roleMenus,false);
        var role = await RoleStore.FindByIdAsync(input.Id.ToString());
        role.RoleName = input.RoleName;
        role.Order = input.roleSort;
        role.Remark = input.Remark;
        if (role.Menus != null)
        {
            role.Menus.Clear();
        }

        var Menus = await MenuRepository.GetListAsync(x => input.menuIds.Contains((int)x.Id));

        // role.Menus = Menus;

        var newRole = await RoleRepository.UpdateAsync(role, false); 
        roleMenus = await RoleMenuRepository.GetListAsync(rm => rm.RoleId == input.Id);
        await RoleMenuRepository.DeleteManyAsync(roleMenus);
         await RoleMenuRepository.InsertManyAsync(SysRoleMenu.CreateInstances(newRole.Id, input.menuIds.ToArray()), true);
        return CommonResult<string>.Success("", "角色添加完成");
    }

    [HttpPost]
    [ActionName("delete")]
    public async Task<CommonResult<string>> DeleteAsync(long roleId)
    {
        using (var unitOfWork = UnitOfWorkManage.Begin())
        {
            var role = await RoleStore.FindByIdAsync(roleId.ToString());
            var result = await RoleStore.DeleteAsync(role);
            if (result == IdentityResult.Success)
            {
                var roleMenus = await RoleMenuRepository.GetListAsync(rm => rm.RoleId == roleId);
                await RoleMenuRepository.DeleteManyAsync(roleMenus);
            }
        }

        return CommonResult<string>.Success("", "角色删除完成");
    }

    [HttpPost]
    [ActionName("dataScope")]
    public async Task<CommonResult<string>> AssignDataScopeAsync(SysRoleUpdateInput input)
    {
        using (var unitOfWork = UnitOfWorkManage.Begin())
        {
            var newRole = await RoleRepository.FindByIdAsync(input.Id);
            newRole.DataScope = input.DataScope;
            newRole.DeptCheckStrictly = input.DeptCheckStrictly;
            newRole = await RoleRepository.UpdateAsync(newRole);
            if (newRole.DataScope == "2")
            {
                await RoleDeptRepository.DeleteAsync(a => a.RoleId == input.Id);
                await RoleDeptRepository.InsertManyAsync(
                    SysRoleDept.CreateInstances(newRole.Id, input.deptIds.ToArray(), CurrentUser.TenantId),
                    true);
            }

            return CommonResult<string>.Success("", String.Format("角色{0}数据权限分配完成", input.Id));
        }
    }

    [HttpGet]
    [ActionName("authUser/allocatedList")]
    public async Task<CommonResult<PagedResultDto<SysUserOutput>>> GetAllocatedUserListFor(AllocatedUserListInput input)
    {
        var users = await RoleStore.GetAllocatedUserList(input.RoleId, input.UserName, input.Phonenumber);
        var userOutputs = ObjectMapper.Map<List<SysUser>, List<SysUserOutput>>(users);
        return CommonResult<PagedResultDto<SysUserOutput>>.Success(
            new PagedResultDto<SysUserOutput>((long)userOutputs.Count, userOutputs), "获取角色授权用户列表成功");
    }

    [HttpGet]
    [ActionName("authUser/unallocatedList")]
    public async Task<CommonResult<PagedResultDto<SysUserOutput>>> GetUnallocatedUserListFor(
        AllocatedUserListInput input)
    {
        var users = await RoleStore.GetUnallocatedUserList(input.RoleId, input.UserName, input.Phonenumber);
        var userOutputs = ObjectMapper.Map<List<SysUser>, List<SysUserOutput>>(users);
        return CommonResult<PagedResultDto<SysUserOutput>>.Success(
            new PagedResultDto<SysUserOutput>((long)userOutputs.Count, userOutputs), "获取角色授权用户列表成功");
    }

    [HttpPost]
    [ActionName("authUser/cancelAuth")]
    public async Task<CommonResult<string>> CancelUserAth([FromBody] string userId, [FromBody] long roleId)
    {
        await RoleStore.CancelUserAth(userId, roleId);
        return CommonResult<string>.Success("", String.Format("用户{0}取消授权角色{1}数完成", userId, roleId));
    }

    [HttpPost]
    [ActionName("authUser/cancelAuthMultiple")]
    public async Task<CommonResult<string>> CancelMultipleUserAth(string[] userIds, long roleId)
    {
        await RoleStore.CancelUserAth(userIds, roleId);
        return CommonResult<string>.Success("", String.Format("取消授权角色{1}数完成", roleId));
    }

    [HttpPost]
    [ActionName("authUser/confirmAuth")]
    public async Task<CommonResult<string>> CancelUserAth(string[] userIds, long roleId)
    {
        await RoleStore.ComfirmUserAth(userIds, roleId);
        return CommonResult<string>.Success("", String.Format("角色{0}授权完成", roleId));
    }
}