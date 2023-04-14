using System;
using System.Reflection;
using ABPvNextOrangeAdmin.Common.Attribute;
using JetBrains.Annotations;

namespace ABPvNextOrangeAdmin.Common;

public class CommonResult<T>
{
    /**
     * 状态码
     */
    private long code;

    /**
     * 提示信息
     */
    private String message;

    /**
     * 数据封装
     */
    private T? data;

    public CommonResult()
    {
    }

    private CommonResult(long code, string message, T? data)
    {
        this.code = code;
        this.message = message;
        this.data = data;
    }

    public static CommonResult<T> CreateInstance(long code, string message, [CanBeNull] object data)
    {
        return new CommonResult<T>(code, message, (T) data);
    }

    /**
     * 成功返回结果
     *
     * @param data 获取的数据
     */
    public static CommonResult<T> success(T data)
    {
        return CreateInstance(ResultCode.SUCCESS,
            (typeof(ResultCode).GetProperty("SUCCESS"))?.GetCustomAttribute<DescriptionAttribute>()?.value, data);
    }

    /**
     * 成功返回结果
     *
     * @param data 获取的数据
     * @param  message 提示信息
     */
    public static CommonResult<T> Success(T data, String message)
    {
        return CreateInstance(ResultCode.SUCCESS, message, data);
    }
    
    

    /**
     * 失败返回结果
     * @param errorCode 错误码
     */
    public static CommonResult<T> Failed(IErrorCode errorCode)
    {
        return CreateInstance(errorCode.getCode(), errorCode.getMessage(), null);
    }
    
    /**
     * 失败返回结果
     * @param errorCode 错误码
     * @param message 错误信息
     */
    public static CommonResult<T> Failed(long errorCode, String message)
    {
        return CreateInstance(errorCode, message, null);
    }

    /**
     * 失败返回结果
     * @param errorCode 错误码
     * @param message 错误信息
     */
    public static CommonResult<T> Failed(IErrorCode errorCode, String message)
    {
        return CreateInstance(errorCode.getCode(), message, null);
    }

    /**
     * 失败返回结果
     * @param message 提示信息
     */
    public static CommonResult<T> Failed(String message)
    {
        return CreateInstance(ResultCode.FAILED, message, null);
    }

    /**
     * 失败返回结果
     * <param name="errorCode"></param>
     */
    public static CommonResult<T> Failed(long errorCode)
    {
        return Failed(ResultCode.FAILED);
    }

    /**
     * 参数验证失败返回结果
     */
    public static CommonResult<T> validateFailed()
    {
        return Failed(ResultCode.NOTFOUND);
    }

    /**
     * 参数验证失败返回结果
     * @param message 提示信息
     */
    public static CommonResult<T> validateFailed(String message)
    {
        return CreateInstance(ResultCode.NOTFOUND, message, null);
    }

    /**
     * 未登录返回结果
     */
    public static CommonResult<T> unauthorized(T data)
    {
        return CreateInstance(ResultCode.UNAUTHORIZED,
            (typeof(ResultCode).GetProperty("UNAUTHORIZED"))?.GetCustomAttribute<DescriptionAttribute>()?.value, data);
    }

    /**
     * 未授权返回结果
     */
    public static CommonResult<T> forbidden(T data)
    {
        return CreateInstance(ResultCode.FORBIDDEN,
            (typeof(ResultCode).GetProperty("FORBIDDEN"))?.GetCustomAttribute<DescriptionAttribute>()?.value, data);
    }


    /**
     * 状态码
     */
    public long Code
    {
        get => code;
        set => code = value;
    }

    /**
     * 提示信息
     */
    public string Message
    {
        get => message;
        set => message = value;
    }

    /**
     * 数据封装
     */
    public T Data
    {
        get => data;
        set => data = value;
    }
}