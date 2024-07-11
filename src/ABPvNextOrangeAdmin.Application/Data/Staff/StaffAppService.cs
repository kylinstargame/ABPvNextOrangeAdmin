using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using ABPvNextOrangeAdmin.Common;
using ABPvNextOrangeAdmin.Dto;
using ABPvNextOrangeAdmin.System;
using ABPvNextOrangeAdmin.System.Account.Dto;
using ABPvNextOrangeAdmin.System.Permission.Dto;
using ABPvNextOrangeAdmin.System.Roles;
using ABPvNextOrangeAdmin.Utils;
using AutoMapper;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using NotImplementedException = System.NotImplementedException;
using NPinyin;
using Volo.Abp.Caching;
using Volo.Abp.Uow;


namespace ABPvNextOrangeAdmin.Data;

[Authorize]
[Route("api/data/staff/[action]")]
public class StaffAppService : ApplicationService, IStaffAppService
{
    private int a = 12;
        
    private IHttpContextAccessor _httpContextAccessor;
    private IDistributedCache<String> DistributedCache { get; set; }

    public StaffAppService(IRepository<Staff> staffRepository, IRepository<StaffPhotos> staffPhotosRepository,
        UnitOfWorkManager unitOfWorkManager, IRepository<StaffRecord> staffRecordRepository,
        IHttpContextAccessor httpContextAccessor, IDistributedCache<string> distributedCache)
    {
        StaffRepository = staffRepository;
        StaffPhotosRepository = staffPhotosRepository;
        // UnitOfWorkManager = unitOfWorkManager;
        StaffRecordRepository = staffRecordRepository;
        _httpContextAccessor = httpContextAccessor;


        DistributedCache = distributedCache;
    }

    public IRepository<Staff> StaffRepository { get; }
    public IRepository<StaffPhotos> StaffPhotosRepository { get; }

    public IRepository<StaffRecord> StaffRecordRepository { get; }
    // public UnitOfWorkManager UnitOfWorkManager { get; }

    [HttpGet]
    [AllowAnonymous]
    [ActionName("list")]
    public async Task<CommonResult<PagedResultDto<StaffOutput>>> GetListAsync(StaffListInput input)
    {
        var staffsQueryable = (await StaffRepository.WithDetailsAsync(a => a.Photos))
            // .WhereIf(!input.Name.IsNullOrEmpty(),
            //     x => x.Name.Contains(input.Name) || Pinyin.GetPinyin(x.Name).StartsWith(input.Name))
            .WhereIf(input.Years != 0, x => x.Years == input.Years)
            .WhereIf(!input.Dept.IsNullOrEmpty(), x => x.Dept.Contains(input.Dept))
            .OrderByDescending(x => x.LastModificationTime);
        var staffs = staffsQueryable.ToList();
        staffs = staffs.WhereIf(!input.Name.IsNullOrEmpty(),
            x => x.Name.Contains(input.Name) || Pinyin.GetInitials(x.Name).ToLower().StartsWith(input.Name)).ToList();
        staffs = staffs.PageBy<Staff>(input).ToList();
        var staffOutputs = ObjectMapper.Map<List<Staff>, List<StaffOutput>>(staffs);
        return CommonResult<PagedResultDto<StaffOutput>>.Success(
            new PagedResultDto<StaffOutput>(staffsQueryable.Count(), staffOutputs), "获取员工列表成功");
    }


    [HttpPost]
    [ActionName("get")]
    [AllowAnonymous]
    public async Task<CommonResult<StaffOutput>> GetAsync(String staffId)
    {
        staffId = staffId.Replace(' ', '+');

        string xx = HttpUtility.HtmlDecode(staffId);
        Console.WriteLine(xx);
        staffId = CryptoUtils.DecryptRSA(xx);
        var staffQueryable = await StaffRepository.WithDetailsAsync(x => x.Photos);
        var staff = staffQueryable.ToList().Find(X => X.Id.ToString() == staffId);
        var staffOutpus = ObjectMapper.Map<Staff, StaffOutput>(staff);
        return CommonResult<StaffOutput>.Success(staffOutpus, "获取员工信息成功");
    }

    [HttpPost]
    [ActionName("getFromServer")]
    [AllowAnonymous]
    public async Task<CommonResult<StaffOutput>> GetFromServerAsync(long staffId)
    {
        var staffQueryable = await StaffRepository.WithDetailsAsync(x => x.Photos);
        var staff = staffQueryable.ToList().Find(X => X.Id == staffId);
        var staffOutpus = ObjectMapper.Map<Staff, StaffOutput>(staff);
        return CommonResult<StaffOutput>.Success(staffOutpus, "获取员工信息成功");
    }

    [HttpGet]
    [ActionName("getQRCodeUrl")]
    [AllowAnonymous]
    public Task<CommonResult<String>> GetQrCodeUrlAsync(long staffId)
    {
        String xx = CryptoUtils.EncryptRSA(staffId.ToString());
        Console.WriteLine(xx);
        //String QRCodeUrl = "http://localhost:8080/#/?uid=" + UrlEncoder.Default.Encode(xx);
        String QRCodeUrl = "https://pw.dcfund.com.cn:43342/#/?uid=" + UrlEncoder.Default.Encode(xx);
        Console.WriteLine(QRCodeUrl);

        Console.WriteLine(xx);
        return Task.FromResult(CommonResult<String>.Success(QRCodeUrl, "获取员工二维码链接"));
    }


    [HttpPost]
    [ActionName("add")]
    [AllowAnonymous]
    public async Task<CommonResult<string>> CreateAsync(StaffUpdateInutput input)
    {
        var staff = ObjectMapper.Map<StaffUpdateInutput, Staff>(input);
        staff.Approved = true;
        var newStaff = await StaffRepository.InsertAsync(staff);
        if (newStaff != null)
        {
            staff.Photos = StaffPhotos.CreateInstances(newStaff.Id, input.Photos.ToArray());
            return CommonResult<String>.Success(
                "获取员工列表成功", "获取员工列表成功");
        }


        return CommonResult<String>.Failed("获取员工列表失败");
    }

    [HttpPost]
    [ActionName("addFromMobile")]
    [AllowAnonymous]
    public async Task<CommonResult<string>> CreateFromMobileAsync(StaffUpdateInutput input)
    {
        var staff = ObjectMapper.Map<StaffUpdateInutput, Staff>(input);
        var newStaff = await StaffRepository.InsertAsync(staff);
        if (newStaff != null)
        {
            newStaff.Approved = false;
            staff.Photos = StaffPhotos.CreateInstances(newStaff.Id, input.Photos.ToArray());
            return CommonResult<String>.Success(
                "获取员工列表成功", "获取员工列表成功");
        }


        return CommonResult<String>.Failed("获取员工列表失败");
    }

    /// <summary>
    /// 审核·
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("auditing")]
    [AllowAnonymous]
    public async Task<CommonResult<string>> AuditingStaffAsync(long staffId, bool pass)
    {
        if (pass)
        {
            var staff = await StaffRepository.GetAsync(x => x.Id == staffId);
            staff.Approved = true;
            await StaffRepository.UpdateAsync(staff);

            return CommonResult<String>.Success(null, "员工信息审核通过");
        }
        else
        {
            await StaffRepository.DeleteAsync(x => x.Id == staffId);
            var staffPhotos = await StaffPhotosRepository.GetListAsync(x => x.StaffId == staffId);
            await StaffPhotosRepository.DeleteManyAsync(staffPhotos);
            return CommonResult<String>.Success(null, "删除未通过信息");
        }
    }

    /// <summary>
    /// 撤销审核·
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("cancelAuitingForRecord")]
    [AllowAnonymous]
    public async Task<CommonResult<string>> CancelAuditingStaffAsync(StaffRecordOutput staffRecordOutput)
    {
        using (var unitOfWork = UnitOfWorkManager.Begin())
        {
            var staffRecord = await StaffRecordRepository.FindAsync(x => x.Id == staffRecordOutput.Id);
            if (staffRecord.FieldName.IsNullOrEmpty())
            {
                var staffRecords =
                    await StaffRecordRepository.GetListAsync(x => x.AlterBatch == staffRecord.AlterBatch);
                foreach (var tmpStaffRecord in staffRecords)
                {
                    await CancelAuditStaffRecord(tmpStaffRecord);
                }

                staffRecord.Approved = false;
                await StaffRecordRepository.UpdateAsync(staffRecord);
            }
            else
            {
                await CancelAuditStaffRecord(staffRecord);
            }

            await unitOfWork.SaveChangesAsync();
            var staffRecordCount =
                await StaffRecordRepository.CountAsync(x => x.AlterBatch == staffRecordOutput.AlterBatch);
            if (staffRecordCount == 1)
            {
                var rootStaffRecord =
                    await StaffRecordRepository.FindAsync(x => x.AlterBatch == staffRecordOutput.AlterBatch);
                if (rootStaffRecord != null)
                {
                    await StaffRecordRepository.DeleteAsync(rootStaffRecord);
                }
            }

            return CommonResult<string>.Success(null, "审核完成");
        }
    }

    [HttpPost]
    [ActionName("update")]
    [AllowAnonymous]
    public async Task<CommonResult<String>> UpdateAsync(StaffUpdateInutput input)
    {
        using (var unitOfWork = UnitOfWorkManager.Begin())
        {
            var staff = await StaffRepository.FindAsync(x => x.Id == input.Id);
            staff.Name = input.Name;
            staff.Years = input.Years;
            staff.Dept = input.Dept;
            staff.Video = input.Video;
            staff.Remark = input.Remark;
            staff.signature = input.signature;
            await StaffRepository.UpdateAsync(staff);
            var staffPhotos = await StaffPhotosRepository.GetListAsync(x => x.StaffId == input.Id);
            await StaffPhotosRepository.DeleteManyAsync(staffPhotos);
            await StaffPhotosRepository.InsertManyAsync(StaffPhotos.CreateInstances(staff.Id,
                input.Photos.ToArray()));
        }


        return CommonResult<String>.Success(null, "获取员工信息更新成功");
    }

    [HttpPost]
    [ActionName("updateFromMobile1")]
    [AllowAnonymous]
    public async Task<CommonResult<String>> UpdateFromMobile1Async(StaffUpdateInutput input)
    {
        using (var unitOfWork = UnitOfWorkManager.Begin())
        {
            input.signature = CreateSignature(input.signature);
            String batchName = "Batch" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var newStaff = ObjectMapper.Map<StaffUpdateInutput, Staff>(input);
            var oldStaff = (await StaffRepository.WithDetailsAsync(x => x.Photos)).ToList()
                .Find(x => x.Id == input.Id);
            bool changed = false;
            if (newStaff.Video != oldStaff.Video)
            {
                changed = true;
                var staffRecordForVideo =
                    StaffRecord.CreateUpdaqteInstance(batchName, oldStaff.Id, oldStaff.Name, "视频",
                        newStaff.Video,
                        oldStaff.Video);
                await StaffRecordRepository.InsertAsync(staffRecordForVideo);
            }

            if (newStaff.signature != oldStaff.signature)
            {
                changed = true;
                var staffRecordForSignatrue =
                    StaffRecord.CreateUpdaqteInstance(batchName, oldStaff.Id, oldStaff.Name, "寄语",
                        newStaff.signature, oldStaff.signature);
                await StaffRecordRepository.InsertAsync(staffRecordForSignatrue);
            }

            var oldPhotos = oldStaff.Photos.Select(x => x.PhotoUrl).ToList();
            var newPhotos = input.Photos.Where(x =>
                !oldPhotos.Contains(x)
            ).ToList();
            if (newPhotos.Count > 0)
            {
                changed = true;
                var staffRecordsForAddPhotos =
                    StaffRecord.CreateAddInstances(batchName, oldStaff.Id, oldStaff.Name, "照片",
                        newPhotos.ToArray());
                await StaffRecordRepository.InsertManyAsync(staffRecordsForAddPhotos);
            }

            var delPhotos = oldStaff.Photos.Select(x => x.PhotoUrl).Where(x =>
                !input.Photos.Contains(x)).ToList();
            if (delPhotos.Count > 0)
            {
                var staffRecordsForDelPhotos =
                    StaffRecord.CreateDeleteInstances(batchName, oldStaff.Id, oldStaff.Name, "照片",
                        delPhotos.ToArray());
                foreach (var staffRecordsForDelPhoto in staffRecordsForDelPhotos)
                {
                    if ((await StaffRecordRepository.GetListAsync(x =>
                            x.StaffId == staffRecordsForDelPhoto.StaffId &&
                            x.FieldName == staffRecordsForDelPhoto.FieldName
                            && x.Operate == staffRecordsForDelPhoto.Operate
                            && x.Content == staffRecordsForDelPhoto.Content))
                        .ToList().Count == 0)
                    {
                        await StaffRecordRepository.InsertAsync(staffRecordsForDelPhoto);
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                var staffRecord =
                    StaffRecord.CreateBatchInstance(batchName, oldStaff.Id, oldStaff.Name, "");
                await StaffRecordRepository.InsertAsync(staffRecord);
            }
        }

        return CommonResult<String>.Success(null, input.signature);
    }


    [HttpPost]
    [ActionName("updateFromMobile")]
    [AllowAnonymous]
    public async Task<CommonResult<String>> UpdateFromMobileAsync(StaffUpdateInutput input)
    {
        using (var unitOfWork = UnitOfWorkManager.Begin())
        {
            String batchName = "Batch" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var newStaff = ObjectMapper.Map<StaffUpdateInutput, Staff>(input);
            var oldStaff = (await StaffRepository.WithDetailsAsync(x => x.Photos)).ToList()
                .Find(x => x.Id == input.Id);
            bool changed = false;
            if (newStaff.Video != oldStaff.Video)
            {
                changed = true;
                var staffRecordForVideo =
                    StaffRecord.CreateUpdaqteInstance(batchName, oldStaff.Id, oldStaff.Name, "视频",
                        newStaff.Video,
                        oldStaff.Video);
                await StaffRecordRepository.InsertAsync(staffRecordForVideo);
            }

            if (newStaff.signature != oldStaff.signature)
            {
                changed = true;
                var staffRecordForSignatrue =
                    StaffRecord.CreateUpdaqteInstance(batchName, oldStaff.Id, oldStaff.Name, "寄语",
                        newStaff.signature, oldStaff.signature);
                await StaffRecordRepository.InsertAsync(staffRecordForSignatrue);
            }

            var oldPhotos = oldStaff.Photos.Select(x => x.PhotoUrl).ToList();
            var newPhotos = input.Photos.Where(x =>
                !oldPhotos.Contains(x)
            ).ToList();
            if (newPhotos.Count > 0)
            {
                changed = true;
                var staffRecordsForAddPhotos =
                    StaffRecord.CreateAddInstances(batchName, oldStaff.Id, oldStaff.Name, "照片",
                        newPhotos.ToArray());
                await StaffRecordRepository.InsertManyAsync(staffRecordsForAddPhotos);
            }

            var delPhotos = oldStaff.Photos.Select(x => x.PhotoUrl).Where(x =>
                !input.Photos.Contains(x)).ToList();
            if (delPhotos.Count > 0)
            {
                var staffRecordsForDelPhotos =
                    StaffRecord.CreateDeleteInstances(batchName, oldStaff.Id, oldStaff.Name, "照片",
                        delPhotos.ToArray());
                foreach (var staffRecordsForDelPhoto in staffRecordsForDelPhotos)
                {
                    if ((await StaffRecordRepository.GetListAsync(x =>
                            x.StaffId == staffRecordsForDelPhoto.StaffId &&
                            x.FieldName == staffRecordsForDelPhoto.FieldName
                            && x.Operate == staffRecordsForDelPhoto.Operate
                            && x.Content == staffRecordsForDelPhoto.Content))
                        .ToList().Count == 0)
                    {
                        await StaffRecordRepository.InsertAsync(staffRecordsForDelPhoto);
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                var staffRecord =
                    StaffRecord.CreateBatchInstance(batchName, oldStaff.Id, oldStaff.Name, "");
                await StaffRecordRepository.InsertAsync(staffRecord);
            }
        }

        return CommonResult<String>.Success(null, "获取员工信息更新成功");
    }

    [HttpPost]
    [ActionName("delete")]
    public async Task<CommonResult<string>> DeleteAsync(string staffId)
    {
        var staffIds = staffId.Split(",");

        using (var unitOfWork = UnitOfWorkManager.Begin())
        {
            await StaffRepository.DeleteAsync(x => staffId.Contains(x.Id.ToString()));
        }

        return CommonResult<string>.Success("", "员工删除完成");
    }

    [HttpGet]
    [ActionName("getAlterBatches")]
    public async Task<CommonResult<PagedResultDto<StaffBatch>>> GetBatchesAsync(PagedInput input)
    {
        IQueryable<StaffRecord> staffRecordQueryable = await StaffRecordRepository.GetQueryableAsync();
        var staffRecords =
            staffRecordQueryable.Where(x => x.FieldName == "")
                .ToList().PageBy<StaffRecord>(input).ToList();
        var staffBatches = ObjectMapper.Map<List<StaffRecord>, List<StaffBatch>>(staffRecords);
        foreach (var staffBatch in staffBatches)
        {
            staffRecords =
                await StaffRecordRepository.GetListAsync(
                    x => x.AlterBatch == staffBatch.AlterBatch && x.FieldName != "");
            staffBatch.Children = staffRecords;
        }

        return CommonResult<PagedResultDto<StaffBatch>>.Success(
            new PagedResultDto<StaffBatch>(staffBatches.Count, staffBatches), "获取变动记录成功");
    }

    [HttpPost]
    [ActionName("auditingRejectForRecord")]
    [AllowAnonymous]
    public async Task<CommonResult<string>> AuditingRejectStaffBatchesAsync(StaffRecordOutput staffRecordOutput)
    {
        var staffRecord = ObjectMapper.Map<StaffRecordOutput, StaffRecord>(staffRecordOutput);
        using (var unitOfWork = UnitOfWorkManager.Begin())
        {
            if (staffRecord.FieldName.IsNullOrEmpty())
            {
                var staffRecords =
                    await StaffRecordRepository.GetListAsync(x => x.AlterBatch == staffRecord.AlterBatch);
                await StaffRecordRepository.DeleteManyAsync(staffRecords);
            }
            else
            {
                var oldStaffRecord =
                    await StaffRecordRepository.FindAsync(x => x.Id == staffRecord.Id);
                if (oldStaffRecord != null)
                {
                    await StaffRecordRepository.DeleteAsync(oldStaffRecord);
                }
            }

            await unitOfWork.CompleteAsync();
        }

        var staffRecordCount =
            await StaffRecordRepository.CountAsync(x => x.AlterBatch == staffRecordOutput.AlterBatch);
        if (staffRecordCount == 1)
        {
            var rootStaffRecord =
                await StaffRecordRepository.FindAsync(x => x.AlterBatch == staffRecordOutput.AlterBatch);
            if (rootStaffRecord != null)
            {
                await StaffRecordRepository.DeleteAsync(rootStaffRecord);
            }
        }

        return CommonResult<string>.Success(null, "审核完成");
    }


    [HttpPost]
    [ActionName("auditingForRecord")]
    [AllowAnonymous]
    public async Task<CommonResult<string>> AuditingStaffBatchesAsync(StaffRecordOutput staffRecordOutput)
    {
        using (var unitOfWork = UnitOfWorkManager.Begin())
        {
            var staffRecord = await StaffRecordRepository.FindAsync(x => x.Id == staffRecordOutput.Id);
            if (staffRecord.FieldName.IsNullOrEmpty())
            {
                var staffRecords =
                    await StaffRecordRepository.GetListAsync(x => x.AlterBatch == staffRecord.AlterBatch);
                foreach (var tmpStaffRecord in staffRecords)
                {
                    await AuditStaffRecord(tmpStaffRecord);
                }

                staffRecord.Approved = true;
                await StaffRecordRepository.UpdateAsync(staffRecord);
            }
            else
            {
                await AuditStaffRecord(staffRecord);
            }

            await unitOfWork.SaveChangesAsync();
        }

        var staffRecordCount =
            await StaffRecordRepository.CountAsync(x => x.AlterBatch == staffRecordOutput.AlterBatch);
        if (staffRecordCount == 1)
        {
            var rootStaffRecord =
                await StaffRecordRepository.FindAsync(x => x.AlterBatch == staffRecordOutput.AlterBatch);
            if (rootStaffRecord != null)
            {
                await StaffRecordRepository.DeleteAsync(rootStaffRecord);
            }
        }

        return CommonResult<string>.Success(null, "审核完成");
    }

    private async Task AuditStaffRecord(StaffRecord staffRecord)
    {
        if (staffRecord.StaffId > 0)
        {
            var staff = await StaffRepository.FindAsync(x => x.Id == staffRecord.StaffId);
            if (staff == null)
            {
                throw new ArgumentNullException(nameof(staff));
            }

            staffRecord.Operate = staffRecord.Operate.Replace("\"", "");

            staffRecord.FieldName = staffRecord.FieldName.Replace("\"", "");
            if (staffRecord.Operate == "修改" && staffRecord.FieldName == "视频")
            {
                staff.Video = staffRecord.Content;
            }

            if (staffRecord.Operate == "修改" && staffRecord.FieldName == "寄语")
            {
                staff.signature = staffRecord.Content;
            }

            if (staffRecord.Operate == "新增" && staffRecord.FieldName == "照片")
            {
                await StaffPhotosRepository.InsertAsync(
                    StaffPhotos.CreateInstance(staff.Id, staffRecord.Content));
            }

            if (staffRecord.Operate == "删除" && staffRecord.FieldName == "照片")
            {
                var delstaffRecord = await StaffPhotosRepository.GetListAsync(x =>
                    x.StaffId == staffRecord.StaffId
                    && x.PhotoUrl == staffRecord.Content);

                if (delstaffRecord != null)
                {
                    await StaffPhotosRepository.DeleteManyAsync(delstaffRecord);
                }
            }

            await StaffRepository.UpdateAsync(staff);
            var oldstaffRecord = await StaffRecordRepository.FindAsync(x => x.Id == staffRecord.Id);
            oldstaffRecord.Approved = true;
            await StaffRecordRepository.UpdateAsync(oldstaffRecord);
        }

        add(10, 10);
        add();
        subtraction(30, 10);
        int a = 100;
        int b = 10;
        add(10, 10);
        subtraction(a, b);
        add(a, b);
        multiplication(a,b);
    }

    
    private int add()
    {
        int a=1;
        int b = 2;
        return a + b;
    }
    private int add(int a,int b)
    {
        return a + b;
    }

    private int multiplication(int a,int b)
    {
        return a * b;
    }

    private int subtraction(int a,int b)
    {
        return a - b;
    }
    private async Task CancelAuditStaffRecord(StaffRecord staffRecord)
    {
        if (staffRecord.StaffId > 0)
        {
            var staff = await StaffRepository.FindAsync(x => x.Id == staffRecord.StaffId);
            if (staff == null)
            {
                throw new ArgumentNullException(nameof(staff));
            }

            staffRecord.Operate = staffRecord.Operate.Replace("\"", "");

            staffRecord.FieldName = staffRecord.FieldName.Replace("\"", "");
            if (staffRecord.Operate == "修改" && staffRecord.FieldName == "视频")
            {
                staff.Video = staffRecord.Content;
            }

            if (staffRecord.Operate == "修改" && staffRecord.FieldName == "寄语")
            {
                staff.signature = staffRecord.OriginContent;
            }

            if (staffRecord.Operate == "新增" && staffRecord.FieldName == "照片")
            {
                var delstaffRecord = await StaffPhotosRepository.GetListAsync(x =>
                    x.StaffId == staffRecord.StaffId
                    && x.PhotoUrl == staffRecord.Content);

                if (delstaffRecord != null)
                {
                    await StaffPhotosRepository.DeleteManyAsync(delstaffRecord);
                }
            }

            if (staffRecord.Operate == "删除" && staffRecord.FieldName == "照片")
            {
                await StaffPhotosRepository.InsertAsync(
                    StaffPhotos.CreateInstance(staff.Id, staffRecord.Content));
            }

            await StaffRepository.UpdateAsync(staff);
            var oldstaffRecord = await StaffRecordRepository.FindAsync(x => x.Id == staffRecord.Id);
            oldstaffRecord.Approved = false;
            await StaffRecordRepository.UpdateAsync(oldstaffRecord);
        }
    }


     private string CreateSignature(string text, int canvasWidth = 654,
        int canvasHeight = 371)
    {
        string bgPath = AppDomain.CurrentDomain.BaseDirectory + "寄语文字背景图.jpg";
        Image bgImg = Image.FromFile(bgPath);

        //创建画布 
        var bitmap = new Bitmap(canvasWidth, canvasHeight);
        bitmap.SetResolution(651, 371);
        //创建画笔
        Graphics graphics = Graphics.FromImage(bitmap);
        graphics.DrawImage(bgImg, new PointF(0.0F, 0.0F));
        // graphics.Clear(Color.White);

        //设置字体
        PrivateFontCollection pfcoll = new PrivateFontCollection();
        pfcoll.AddFontFile( AppDomain.CurrentDomain.BaseDirectory +"font.TTF");
        Font font = new Font(pfcoll.Families[0], 5f);
        //设置画刷
        Brush brush = new SolidBrush(Color.White);
        var line = "";
        var lines = new List<String>();
        int maxWidth = 634;
        int lineHeight = 80; // 行高
        int x = 20; // 起始x坐标
        int y = 20; // 起始y坐标

        // 将文本拆分成多行
        for (int i = 0; i < text.Length; i++)
        {
            string word = text[i].ToString();
            if (graphics.MeasureString(line + word, font).Width < maxWidth)
            {
                line += word;
            }
            else
            {
                lines.Add(line);
                line = "";
            }
        }

        lines.Add(line);

        //绘制多行文本
        foreach (var lineText in lines)
        {
            graphics.DrawString(lineText, font, brush, new PointF { X = x, Y = y });

            y += lineHeight;
        }
   
        MemoryStream ms = new MemoryStream();
        bitmap.Save(ms, ImageFormat.Jpeg);
        byte[] bytes = ms.ToArray();

        string base64 = Convert.ToBase64String(bytes);


        var filename = Guid.NewGuid();


        DistributedCache.Set($"filecontentType{filename}", "image/jpeg",
            new DistributedCacheEntryOptions { SlidingExpiration = new TimeSpan(10000, 0, 0) });
        DistributedCache.Set($"filecontent{filename}", base64,
            new DistributedCacheEntryOptions { SlidingExpiration = new TimeSpan(10000, 0, 0) });

        //获取完整的Url地址
        // var imageUrl = _httpContextAccessor.HttpContext.Request.Scheme.ToString() + "://" +
        //                _httpContextAccessor.HttpContext.Request.Host.ToString() +
        //                "/api/common/get?filename=" + filename;
        var imageUrl =     "https://pw.dcfund.com.cn:43342/api/common/get?filename=" + filename;
        return imageUrl;
    }
}