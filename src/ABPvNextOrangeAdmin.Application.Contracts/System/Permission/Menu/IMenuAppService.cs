using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Organization.Dto;

namespace ABPvNextOrangeAdmin.System;

public interface IMenuAppService
{
    public Task<CommonResult<List<SysMenuTreeSelectOutput>>> GetTreeSelectAsync();
    public Task<CommonResult<List<SysMenuTreeSelectOutput>>> GetListAsync(MenuInput menuInput);
    
    public Task<CommonResult<SysMenuOutput>> GetAsync(int id);

    public Task<CommonResult<string>> CreateAsync(SysMenuOutput input);
    
    public Task<CommonResult<string>> UpdateAsync(SysMenuOutput input);
    
    public Task<CommonResult<string>> DeleteAsync(int id);
}