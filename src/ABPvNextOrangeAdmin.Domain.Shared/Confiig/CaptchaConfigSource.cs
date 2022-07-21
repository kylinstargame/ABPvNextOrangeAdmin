using System;
using System.Collections.Generic;
using ABPvNextOrangeAdmin.Constans;
using ABPvNextOrangeAdmin.Utils.ImageProducer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ABPvNextOrangeAdmin.Confiig;

public class CaptchaConfigSource : IConfigurationSource
{
    // private readonly Action<DbContextOptionsBuilder> _optionsAction;

    // public EFConfigurationSource(Action<DbContextOptionsBuilder> optionsAction) => _optionsAction = optionsAction;
    
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new CaptchaProvider();
    }
}

public class CaptchaProvider : ConfigurationProvider
{
    public void Load()
    {
        Data = getCaptchaProperties();
    }
    
    public Dictionary<String, String> getCaptchaProperties()
    {
        Dictionary<String, String> properties = new Dictionary<string, string>();
        // 是否有边框 默认为true 我们可以自己设置yes，no 
        properties.Add(CaptchaConstants.CAPTCHA_BORDER, "yes");
        // 验证码文本字符颜色 默认为Color.BLACK
        properties.Add(CaptchaConstants.CAPTCHA_TEXTPRODUCER_FONT_COLOR, "black");
        // 验证码图片宽度 默认为200
        properties.Add(CaptchaConstants.CAPTCHA_IMAGE_WIDTH, "160");
        // 验证码图片高度 默认为50
        properties.Add(CaptchaConstants.CAPTCHA_IMAGE_HEIGHT, "60");
        // 验证码文本字符大小 默认为40
        properties.Add(CaptchaConstants.CAPTCHA_TEXTPRODUCER_FONT_SIZE, "38");
        // CaptchaConstants.CAPTCHA_SESSION_KEY
        properties.Add(CaptchaConstants.CAPTCHA_SESSION_CONFIG_KEY, "captchaCode");
        // 验证码文本字符长度 默认为5
        properties.Add(CaptchaConstants.CAPTCHA_TEXTPRODUCER_CHAR_LENGTH, "4");
        // 验证码文本字体样式 默认为new Font("Arial", 1, fontSize), new Font("Courier", 1, fontSize)
        properties.Add(CaptchaConstants.CAPTCHA_TEXTPRODUCER_FONT_NAMES, "Arial,Courier");
        // 图片样式 水纹com.google.code.captcha.impl.WaterRipple 鱼眼com.google.code.captcha.impl.FishEyeGimpy 阴影com.google.code.captcha.impl.ShadowGimpy
        properties.Add(CaptchaConstants.CAPTCHA_OBSCURIFICATOR_IMPL, "com.google.code.captcha.impl.ShadowGimpy");
        return properties;
    }
    
    public Dictionary<String, String> getCaptchaPropertiesMath()
    {
        Dictionary<String, String> properties = new Dictionary<string, string>();
        // 是否有边框 默认为true 我们可以自己设置yes，no
        properties.Add(CaptchaConstants.CAPTCHA_BORDER, "yes");
        // 边框颜色 默认为Color.BLACK
        properties.Add(CaptchaConstants.CAPTCHA_BORDER_COLOR, "105,179,90");
        // 验证码文本字符颜色 默认为Color.BLACK
        properties.Add(CaptchaConstants.CAPTCHA_TEXTPRODUCER_FONT_COLOR, "blue");
        // 验证码图片宽度 默认为200
        properties.Add(CaptchaConstants.CAPTCHA_IMAGE_WIDTH, "160");
        // 验证码图片高度 默认为50
        properties.Add(CaptchaConstants.CAPTCHA_IMAGE_HEIGHT, "60");
        // 验证码文本字符大小 默认为40
        properties.Add(CaptchaConstants.CAPTCHA_TEXTPRODUCER_FONT_SIZE, "35");
        // CaptchaConstants.CAPTCHA_SESSION_KEY
        properties.Add(CaptchaConstants.CAPTCHA_SESSION_CONFIG_KEY, "captchaCodeMath");
        // 验证码文本生成器
        properties.Add(CaptchaConstants.CAPTCHA_TEXTPRODUCER_IMPL, "com.ruoyi.framework.config.captchaTextCreator");
        // 验证码文本字符间距 默认为2
        properties.Add(CaptchaConstants.CAPTCHA_TEXTPRODUCER_CHAR_SPACE, "3");
        // 验证码文本字符长度 默认为5
        properties.Add(CaptchaConstants.CAPTCHA_TEXTPRODUCER_CHAR_LENGTH, "6");
        // 验证码文本字体样式 默认为new Font("Arial", 1, fontSize), new Font("Courier", 1, fontSize)
        properties.Add(CaptchaConstants.CAPTCHA_TEXTPRODUCER_FONT_NAMES, "Arial,Courier");
        // 验证码噪点颜色 默认为Color.BLACK
        properties.Add(CaptchaConstants.CAPTCHA_NOISE_COLOR, "white");
        // 干扰实现类
        properties.Add(CaptchaConstants.CAPTCHA_NOISE_IMPL, "com.google.code.captcha.impl.NoNoise");
        // 图片样式 水纹com.google.code.captcha.impl.WaterRipple 鱼眼com.google.code.captcha.impl.FishEyeGimpy 阴影com.google.code.captcha.impl.ShadowGimpy
        properties.Add(CaptchaConstants.CAPTCHA_OBSCURIFICATOR_IMPL, "com.google.code.captcha.impl.ShadowGimpy");
        return properties;
    }
}