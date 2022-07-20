using System;
using ABPvNextOrangeAdmin.Common.Attribute;

namespace ABPvNextOrangeAdmin.Common;

public class ResultCode : IErrorCode
{
    [Description("操作成功")] public static long SUCCESS = 200;

    [Description("操作失败")] public static long FAILED = 500;

    [Description("参数检验失败")] public static long NOTFOUND = 404;

    [Description("暂未登录或token已经过期")] public static long UNAUTHORIZED = 401;

    [Description("没有相关权限")] public static long FORBIDDEN = 403;

    private long code;
    private String message;

    public long getCode()
    {
        return code;
    }

    public string getMessage()
    {
        return message;
    }
}