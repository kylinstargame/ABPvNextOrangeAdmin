using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Validation;

namespace ABPvNextOrangeAdmin.Common;

[Authorize]
[Route("api/common/[action]")]
public class CommonAppService : ApplicationService
{
    private IHttpContextAccessor HttpContextAccessor;

    public CommonAppService(IDistributedCache<string> distributedCache, IHttpContextAccessor httpContextAccessor)
    {
        DistributedCache = distributedCache;
        HttpContextAccessor = httpContextAccessor;
    }

    private IDistributedCache<String> DistributedCache { get; set; }

    /// <summary>
    /// 上传图像缓存， 返回待上传的图片id，Content-Type：multipart/form-data>
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <exception cref="AbpValidationException"></exception>
    [HttpPost]
    [AllowAnonymous]
    [Consumes("multipart/form-data")]
    [ActionName("upload2redis")]
    public async Task<String> UploadToCacheAsync(IFormFile file)
    {
        var formFileName = file.FileName;
        if (!new[] { ".png", ".jpg", ".bmp","mp4" }.Any((item) => formFileName.EndsWith(item)))
        {
            throw new AbpValidationException("您上传的文件格式必须为png、jpg、bmp中的一种");
        }

        byte[] bytes;
        using (var bodyStream = file.OpenReadStream())
        {
            using (var m = new MemoryStream())
            {
                await bodyStream.CopyToAsync(m);
                bytes = m.ToArray();
            }
        }

        string base64 = Convert.ToBase64String(bytes);


        var filename = Guid.NewGuid();
     

        DistributedCache.Set($"filecontentType{filename}", file.ContentType,
            new DistributedCacheEntryOptions { SlidingExpiration = new TimeSpan(10000, 0, 0) });
        DistributedCache.Set($"filecontent{filename}", base64,
            new DistributedCacheEntryOptions { SlidingExpiration = new TimeSpan(10000, 0, 0) });

        //获取完整的Url地址

        Debug.Assert(HttpContextAccessor.HttpContext != null, (string)"HttpContextAccessor.HttpContext != null");
        var imageUrl = HttpContextAccessor.HttpContext.Request.Scheme.ToString() + "://" +
                       HttpContextAccessor.HttpContext.Request.Host.ToString() +
                       "/api/common/get?filename=" + filename;
            return imageUrl;
        
}

    /// <summary>
    /// 上传图像至数据库， 返回待上传的图片id，Content-Type：multipart/form-data>
    /// TODO:保存至数据库
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <exception cref="AbpValidationException"></exception>
    [HttpGet]
    [AllowAnonymous]
    [ActionName("upload2db")]
    public async Task<CommonResult<String>> UploadToDBAsync(IFormFile file)
    {
        var formFileName = file.FileName;
        if (!new[] { ".png", ".jpg", ".bmp" }.Any((item) => formFileName.EndsWith(item)))
        {
            throw new AbpValidationException("您上传的文件格式必须为png、jpg、bmp中的一种");
        }


        byte[] bytes;
        using (var bodyStream = file.OpenReadStream())
        {
            using (var m = new MemoryStream())
            {
                await bodyStream.CopyToAsync(m);
                bytes = m.ToArray();
            }
        }


        string base64 = Convert.ToBase64String(bytes);
        var fileContentType = "imgype" + Guid.NewGuid();
        var fileContent = "img" + Guid.NewGuid();

        DistributedCache.Set("fileContentType", file.ContentType,
            new DistributedCacheEntryOptions { SlidingExpiration = new TimeSpan(10000, 0, 0) });
        DistributedCache.Set($"{CurrentUser.TenantId}:bg:{fileContent}", base64,
            new DistributedCacheEntryOptions { SlidingExpiration = new TimeSpan(10000, 0, 0) });
        return CommonResult<String>.Success(fileContent, "上传成功");
    }

    [HttpGet]
    [AllowAnonymous]
    [ActionName("get")]
    public async Task<FileContentResult> GetFileAsync(String filename)
    {
        String fileContent = (await DistributedCache.GetAsync($"filecontent{filename}"));
        String fileContentType = (await DistributedCache.GetAsync($"filecontentType{filename}"));
        byte[] imageBytes = Convert.FromBase64String(fileContent);
        return new FileContentResult(imageBytes, fileContentType);
    }
}