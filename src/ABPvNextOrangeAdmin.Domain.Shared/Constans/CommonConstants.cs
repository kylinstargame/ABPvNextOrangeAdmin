using System;

namespace ABPvNextOrangeAdmin.Constans;

public class CommonConstants
{
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