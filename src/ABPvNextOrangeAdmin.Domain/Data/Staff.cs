using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.Data;

public class Staff:FullAuditedAggregateRoot<long>
{
   /// <summary>
   /// 姓名
   /// </summary>
   public String Name { get; set; } 
   
   /// <summary>
   /// 入职权限
   /// </summary>
   public int Years{ get; set; } 
   
   /// <summary>
   /// 部门
   /// </summary>
   public string Dept{ get; set; }

   /// <summary>
   /// 个人风采-照片
   /// </summary>
   public List<StaffPhotos> Photos;
   
   /// <summary>
   /// 个人风采-视频
   /// </summary>
   public String Video{ get; set; } 
   
   /// <summary>
   /// 个人简介
   /// </summary>
   public String Remark{ get; set; }

   /// <summary>
   /// 签名照片
   /// </summary>
   public String signature { get; set; }
}

