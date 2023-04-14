using System.Collections.Generic;

namespace ABPvNextOrangeAdmin.CodeGen;

public class GenTable
{
    /// <summary>
    /// 编号 
    /// </summary>
    private long tableId;

    /// <summary>
    /// 表名称 
    /// </summary>
    private string tableName;

    /// <summary>
    /// 表描述 
    /// </summary>
    private string tableComment;

    /// <summary>
    /// 关联父表的表名 
    /// </summary>
    private string subTableName;

    /// <summary>
    /// 本表关联父表的外键名 
    /// </summary>
    private string subTableFkName;

    /// <summary>
    /// 实体类名称
    /// </summary>
    private string className;

    /// <summary>
    /// 使用的模板（crud单表操作 tree树表操作 sub主子表操作）
    /// </summary>
    private string tplCategory;

    /// <summary>
    /// 生成包路径 
    /// </summary>
    private string packageName;

    /// <summary>
    /// 生成模块名 
    /// </summary>
    private string moduleName;

    /// <summary>
    /// 生成业务名 
    /// </summary>
    private string businessName;

    /// <summary>
    /// 生成功能名 
    /// </summary>
    private string functionName;

    /// <summary>
    /// 生成作者 
    /// </summary>
    private string functionAuthor;

    /// <summary>
    /// 生成代码方式（0zip压缩包 1自定义路径）
    /// </summary>
    private string genType;

    /// <summary>
    /// 生成路径（不填默认项目路径）
    /// </summary>
    private string genPath;

    /// <summary>
    /// 主键信息 
    /// </summary>
    private GenTableColumn pkColumn;

    /// <summary>
    /// 子表信息 
    /// </summary>
    private GenTable subTable;

    /// <summary>
    /// 表列信息 
    /// </summary>
    private List<GenTableColumn> columns;

    /// <summary>
    /// 其它生成选项 
    /// </summary>
    private string options;

    /// <summary>
    /// 树编码字段 
    /// </summary>
    private string treeCode;

    /// <summary>
    /// 树父编码字段 
    /// </summary>
    private string treeParentCode;

    /// <summary>
    /// 树名称字段 
    /// </summary>
    private string treeName;

    /// <summary>
    /// 上级菜单ID字段
    /// </summary>
    private string parentMenuId;

    /// <summary>
    /// 上级菜单名称字段 
    /// </summary>
    private string parentMenuName;
}