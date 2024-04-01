using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;

namespace ABPvNextOrangeAdmin.Common;
[Authorize]
[Route("api/common/[action]")]
public class CommonAppService:ApplicationService
{
    [HttpPost]
    [AllowAnonymous]
    [ActionName("upload")]
    public async Task<CommonResult<String>> UploadAsync(string file)
    {
       return CommonResult<String>.Success(null,"上传成功");
      
    }

}