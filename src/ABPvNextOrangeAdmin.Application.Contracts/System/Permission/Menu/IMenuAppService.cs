using System.Collections.Generic;
using System.Threading.Tasks;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.System.Organization.Dto;

namespace ABPvNextOrangeAdmin.System;

public interface IMenuAppService
{
    public Task<CommonResult<List<SysMenuTreeSelectOutput>>> GetTreeSelectAsync();
}