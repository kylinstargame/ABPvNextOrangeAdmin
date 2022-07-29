using System;

namespace ABPvNextOrangeAdmin.Constans;

public class CommonConstants
{
    /**
     * http请求
     */
    public static readonly String HTTP = "http://";

    /**
     * https请求
     */
    public static readonly String HTTPS = "https://";
    
    /// <summary>
    /// 验证码 Redis KEY
    /// </summary>
    /// <returns></returns>
    public static readonly String CAPTCHA_CODE_KEY = "captcha_codes:"; 
    
    /// <summary>
    /// 验证码超时时间
    /// </summary>
    public static readonly int CAPTCHA_EXPIRATION = 2;
}