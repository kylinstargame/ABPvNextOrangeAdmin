using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPvNextOrangeAdmin.Data;

public class StaffBatch
{
    public long Id { get; set; }
    public String AlterBatch  { get; set; }

    public List<StaffRecord> Children { get; set; }
    public bool Approved { get; set; }
}
public sealed class StaffRecord : FullAuditedAggregateRoot<long>
{

    public StaffRecord(DateTime creationTime)
    {
        CreationTime = creationTime;
    }

    private StaffRecord( string alterBatch, long staffId, String staffName, string fieldName, string operate, string content,  string origincontent="")
    {
        AlterBatch = alterBatch;
        StaffName = staffName;
        FieldName = fieldName;
        Operate = operate;
        Content = content;
        CreationTime = DateTime.Now;
        OriginContent = origincontent;
        StaffId = staffId;
    }
    public static StaffRecord CreateBatchInstance( string alterBatch,long staffId,  String staffName, string content)
    {
        return new StaffRecord( alterBatch,staffId,staffName, "", "", content);
    }

    public static StaffRecord CreateUpdaqteInstance( string alterBatch,long staffId,  String staffName, string fieldName, string content,string Origincontent="")
    {
        return new StaffRecord( alterBatch,staffId,staffName, fieldName, "修改", content,Origincontent);
    }

    public static List<StaffRecord> CreateAddInstances(string alterBatch,long staffId,   String staffName, string fieldName, string[] contents)
    {
        List<StaffRecord> StaffRecords = new List<StaffRecord>(); 
        foreach (var content in contents)
        {
            StaffRecords.Add( CreateAddInstance(alterBatch,staffId, staffName, fieldName, content));
        }

        return StaffRecords;
    }
    public static StaffRecord CreateAddInstance( string alterBatch, long staffId,  String staffName, string fieldName, string content)
    {
        return new StaffRecord( alterBatch,staffId, staffName, fieldName, "新增", content);
    }
    public static List<StaffRecord> CreateDeleteInstances(string alterBatch,long staffId,  String staffName, string fieldName, string[] contents)
    {
        List<StaffRecord> StaffRecords = new List<StaffRecord>(); 
        foreach (var content in contents)
        {
            StaffRecords.Add( CreateDeleteInstance
                 (alterBatch,staffId, staffName, fieldName, content));
        }

        return StaffRecords;
    }
    public static StaffRecord CreateDeleteInstance( string alterBatch,long staffId,   String staffName, string fieldName, string content)
    {
        return new StaffRecord( alterBatch,staffId, staffName, fieldName, "删除", content);
    }

 

    [Comment("变动批次")]
    public String AlterBatch  { get; set; }
    
    [Comment("相关员工")]
    public long StaffId  { get; set; }
    [Comment("相关员工")]
    public String StaffName  { get; set; }
    [Comment("相关字段")] 
    public String FieldName { get; set; }
    [Comment("操作类型")] 
    public String Operate { get; set; }
    public String Content { get; set; }
    [Comment("原始内容")]
    public String OriginContent { get; set; }
    [Comment("审核标记")]
    [DefaultValue(true)]
    public bool Approved { get; set; }
    
   
}

public class StaffRecordOutput
{
    public long Id { get; set; }  
    public String AlterBatch  { get; set; }     
                      
    public long StaffId  { get; set; }          
                       
    public String StaffName  { get; set; }      
                          
    public String FieldName { get; set; }       
                       
    public String Operate { get; set; }         
    
    public String Content { get; set; }         
    
    public String OriginContent { get; set; }    
    
    public bool Approved { get; set; }          
                                                
    public DateTime CreationTime { get; set; }       
}