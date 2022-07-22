using System;
using System.Text;
using ABPvNextOrangeAdmin.Config;
using ABPvNextOrangeAdmin.Utils.ImageProducer;
using Microsoft.Extensions.Configuration;

namespace ABPvNextOrangeAdmin.Utils.TextProducer;

public class DefaultTextCreator : ITextProducer, IWithConfig
{
    private CaptchaConfig _captchaConfig;
    private ITextProducer _textProducerImplementation;

    public DefaultTextCreator()
    {
    }

    public String getText()
    {
        int length = _captchaConfig.GetTextProducerCharLength();
        char[] chars = _captchaConfig.GetTextProducerCharString();
        Random rand = new Random();
        StringBuilder text = new StringBuilder();

        for (int i = 0; i < length; ++i)
        {
            text.Append(chars[rand.Next(chars.Length)]);
        }
        return text.ToString();
    }

    public IProducer SetConfig(CaptchaConfig config)
    {
        _captchaConfig = config;
        return this;
    }
}