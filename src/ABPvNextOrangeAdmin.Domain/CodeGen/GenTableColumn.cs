namespace ABPvNextOrangeAdmin.CodeGen;

public class GenTableColumn
{
    /// <summary>
    /// 编号 
    /// </summary>
    private long columnId;

    /// <summary>
    /// 归属表编号 
    /// </summary>
    private long tableId;

    /// <summary>
    /// 列名称 
    /// </summary>
    private string columnName;

    /// <summary>
    /// 列描述 
    /// </summary>
    private string columnComment;

    /// <summary>
    /// 列类型 
    /// </summary>
    private string columnType;

    /// <summary>
    /// 类型 
    /// </summary>
    private string javaType;

    /// <summary>
    /// 字段名 
    /// </summary>
    private string javaField;

    /// <summary>
    /// 是否主键
    /// </summary>
    private string isPk;

    /// <summary>
    /// 是否自增
    /// </summary>
    private string isIncrement;

    /// <summary>
    /// 是否必填
    /// </summary>
    private string isRequired;

    /// <summary>
    /// 是否为插入字段
    /// </summary>
    private string isInsert;

    /// <summary>
    /// 是否编辑字段
    /// </summary>
    private string isEdit;

    /// <summary>
    /// 是否列表字段
    /// </summary>
    private string isList;

    /// <summary>
    /// 是否查询字段
    /// </summary>
    private string isQuery;

    /// <summary>
    /// 查询方式（EQ等于、NE不等于、GT大于、LT小于、LIKE模糊、BETWEEN范围）
    /// </summary>
    private string queryType;

    /// <summary>
    /// 显示类型（input文本框、textarea文本域、select下拉框、checkbox复选框、radio单选框、datetime日期控件、image图片上传控件、upload文件上传控件、editor富文本控件）
    /// </summary>
    private string htmlType;

    /// <summary>
    /// 字典类型 
    /// </summary>
    private string dictType;

    /// <summary>
    /// 排序 
    /// </summary>
    private int sort;
 
}