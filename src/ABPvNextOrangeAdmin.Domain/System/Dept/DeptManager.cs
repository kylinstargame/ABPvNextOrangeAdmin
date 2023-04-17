using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Organization;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.System.User;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Uow;

namespace ABPvNextOrangeAdmin.System.Dept;

public class DeptManager : IDomainService
{
    public DeptManager(IDeptRepository deptRepository, IRoleRepository roleRepository)
    {
        DeptRepository = deptRepository;
        RoleRepository = roleRepository;
    }

    protected IDeptRepository DeptRepository;

    protected IRoleRepository RoleRepository;

    [UnitOfWork]
    public virtual async Task CreateAsync(SysDept sysDept)
    {
        sysDept.Code = await this.GetNextChildCodeAsync(sysDept.ParentId).ConfigureAwait(false);
        await ValidateDeptAsync(sysDept).ConfigureAwait(false);
        await DeptRepository.InsertAsync(sysDept).ConfigureAwait(false);
    }

    public virtual async Task UpdateAsync(SysDept sysDept)
    {
        await ValidateDeptAsync(sysDept).ConfigureAwait(false);
        await DeptRepository.UpdateAsync(sysDept).ConfigureAwait(false);
    }

    public virtual async Task<string> GetNextChildCodeAsync(long? parentId)
    {
        SysDept sysDept =
            await this.GetLastChildOrNullAsync(parentId).ConfigureAwait(false);
        if (sysDept != null)
        {
            return SysDept.CalculateNextCode(sysDept.Code);
        }

        string parentCode;
        if (parentId.HasValue)
        {
            parentCode = await GetCodeOrDefaultAsync(parentId).ConfigureAwait(false);
        }
        else
        {
            parentCode = (string)null;
        }

        return SysDept.AppendCode(parentCode, SysDept.CreateCode(1));
    }

    public virtual async Task<SysDept> GetLastChildOrNullAsync(long? parentId)
    {
        return (await DeptRepository.GetChildrenAsync(parentId, false).ConfigureAwait(false)).MaxBy(
            (Func<SysDept, string>)(c => c.Code));
    }

    [UnitOfWork]
    public virtual async Task DeleteAsync(long id)
    {
        foreach (SysDept childDept in await FindChildrenAsync(new long?(id), true)
                     .ConfigureAwait(false))
        {
            ConfiguredTaskAwaitable configuredTaskAwaitable =
                DeptRepository.RemoveAllMembersAsync(childDept).ConfigureAwait(false);
            await configuredTaskAwaitable;
            configuredTaskAwaitable = DeptRepository.RemoveAllRolesAsync(childDept).ConfigureAwait(false);
            await configuredTaskAwaitable;
            configuredTaskAwaitable = DeptRepository.DeleteAsync(childDept, false, new CancellationToken())
                .ConfigureAwait(false);
            await configuredTaskAwaitable;
        }

        SysDept sysDept = await DeptRepository
            .GetAsync(id, true, new CancellationToken()).ConfigureAwait(false);
        await DeptRepository.RemoveAllMembersAsync(sysDept).ConfigureAwait(false);
        await DeptRepository.RemoveAllRolesAsync(sysDept).ConfigureAwait(false);
        await DeptRepository.DeleteAsync(id, false, new CancellationToken()).ConfigureAwait(false);
    }

    [UnitOfWork]
    public virtual async Task MoveAsync(long id, long? parentId)
    {
        var dept = await DeptRepository.GetAsync(id);
        if (dept.ParentId == parentId)
        {
            return;
        }

        //Should find children before Code change
        var children = await FindChildrenAsync(id, true);

        //Store old code of OU
        var oldCode = dept.Code;

        //Move OU
        dept.Code = await GetNextChildCodeAsync(parentId);
        dept.ParentId = parentId;

        await ValidateDeptAsync(dept);

        //Update Children Codes
        foreach (var child in children)
        {
            child.Code = SysDept.AppendCode(dept.Code, SysDept.GetRelativeCode(child.Code, oldCode));
            await DeptRepository.UpdateAsync(child);
        }

        await DeptRepository.UpdateAsync(dept);
    }

    public virtual async Task<string> GetCodeOrDefaultAsync(long? id)
    {
        Debug.Assert(id != null, (string)(nameof(id) + " != null"));
        return (await DeptRepository
            .GetAsync(id.Value, true, new CancellationToken()).ConfigureAwait(false))?.Code;
    }

    protected virtual async Task ValidateDeptAsync(SysDept dept)
    {
        var childrenDepts = (await FindChildrenAsync(dept.ParentId))
            .Where(ou => ou.Id != dept.Id)
            .ToList();

        if (childrenDepts.Any(x => x.DeptName == dept.DeptName))
        {
            throw new global::System.Exception("组织名已经存在");
        }
    }

    public async Task<List<SysDept>> FindChildrenAsync(long? parentId,
        bool recursive = false)
    {
        if (!recursive)
        {
            return await DeptRepository.GetChildrenAsync(parentId, true).ConfigureAwait(false);
        }

        if (!parentId.HasValue)
        {
            return await this.DeptRepository
                .GetListAsync((string)null, int.MaxValue, 0, true).ConfigureAwait(false);
        }

        return await this.DeptRepository
            .GetChildrenWithParentCodeAsync(await this.GetCodeOrDefaultAsync(parentId.Value).ConfigureAwait(false),
                parentId, true).ConfigureAwait(false);
    }

    public virtual Task<bool> IsInDeptAsync(SysUser user, SysDept dept) =>
        Task.FromResult<bool>(user.IsInDept(dept.Id));

    public virtual async Task AddRoleToDeptAsync(long roleId, long DeptId)
    {
        SysRole role = await RoleRepository.GetAsync(roleId).ConfigureAwait(false);
        SysDept ou = await DeptRepository
            .GetAsync(DeptId, true, new CancellationToken()).ConfigureAwait(false);
        await AddRoleToOrganizationUnitAsync(role, ou).ConfigureAwait(false);
    }

    public virtual Task AddRoleToOrganizationUnitAsync(SysRole role, SysDept dept)
    {
        if (dept.Roles.Any<SysRoleDept>(
                (Func<SysRoleDept, bool>)(r => r.DeptId == dept.Id && r.RoleId == role.Id)))
            return (Task)Task.FromResult<int>(0);
        dept.AddRole(role.Id);
        return (Task)DeptRepository.UpdateAsync(dept);
    }

    public virtual async Task RemoveRoleFromDeptAsync(long roleId, long deptId)
    {
        SysRole role = await RoleRepository.GetAsync(roleId).ConfigureAwait(false);
        SysDept dept = await DeptRepository
            .GetAsync(deptId, true).ConfigureAwait(false);
        await this.RemoveRoleFromDeptAsync(role, dept).ConfigureAwait(false);
    }

    public virtual Task RemoveRoleFromDeptAsync(
        SysRole role,
        SysDept sysDept)
    {
        sysDept.RemoveRole(role.Id);
        return (Task)this.DeptRepository.UpdateAsync(sysDept);
    }
}