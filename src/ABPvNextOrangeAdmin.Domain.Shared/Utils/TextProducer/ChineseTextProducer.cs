using System;
using ABPvNextOrangeAdmin.Config;
using ABPvNextOrangeAdmin.Constans;
using ABPvNextOrangeAdmin.Utils.ImageProducer;
using Microsoft.Extensions.Configuration;

namespace ABPvNextOrangeAdmin.Utils.TextProducer;

public class ChineseTextProducer : ITextProducer, IWithConfig
{
    private CaptchaConfig _captchaConfig;
    
    private String[] simplifiedChineseTexts = new String[]
    {
        "包括焦点", "新道消点", "服分目搜", "索姓名電", "子郵件信", "主旨請回", "電子郵件", "給我所有", "討論區明", "發表新文", "章此討論", "區所有文", "章回主題", "樹瀏覽搜"
    };

    public ChineseTextProducer()
    {
    }

    public String getText()
    {
        return this.simplifiedChineseTexts[(new Random()).Next(this.simplifiedChineseTexts.Length)];
    }

    public IProducer SetConfig(CaptchaConfig captchaOption)
    {
        _captchaConfig = captchaOption;
        return this;
    }


}