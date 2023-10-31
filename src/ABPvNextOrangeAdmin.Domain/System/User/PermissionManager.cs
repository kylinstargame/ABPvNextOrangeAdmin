using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.System.Dept;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace ABPvNextOrangeAdmin.System.User;

public interface IPermissionManager : ITransientDependency
{
    public Task<List<string>> GetAllRolePermssionsAysnc(long roleId);
    public Task<List<string>> GetAllUserPermssionsAysnc(Guid userId);
}

public class PermissionManager : IPermissionManager
{
    
     
    protected IPermsssionRepository PermissionRepository { get;}

    public PermissionManager(IPermsssionRepository permissionRepository)
    {
        PermissionRepository = permissionRepository;
    }

    public async Task<List<string>> GetAllRolePermssionsAysnc(long roleId)
    {
        return await PermissionRepository.GetAllRolePermssionsAsync(roleId);
    }

    public async Task<List<string>> GetAllUserPermssionsAysnc(Guid userId)
    {
        return await PermissionRepository.GetAllUserPermssionsAysnc(userId);
    }
}
