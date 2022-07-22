using System;
using System.Collections.Generic;
using System.Drawing;
using ABPvNextOrangeAdmin.Constans;
using ABPvNextOrangeAdmin.Utils;
using ABPvNextOrangeAdmin.Utils.TextProducer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;

namespace ABPvNextOrangeAdmin.Config;

public class CaptchaConfig :  ITransientDependency
{
   private readonly IConfiguration _configuration;
   private ConfigHelper _configHelper;

   public CaptchaConfig(ConfigHelper configHelper, IConfiguration configuration) : base()
   {
      // _configurationRoot = configurationRoot;
      _configHelper = configHelper;
      _configuration = configuration;
      // _configurationRoot = configurationRoot;
   }
   
   
   public ITextProducer GetTextProducerImpl()
   {
      String paramName = CaptchaConstants.CAPTCHA_TEXTPRODUCER_IMPL;
      String paramValue = _configuration.GetValue<string>(paramName);
      ITextProducer textProducer = (ITextProducer)_configHelper.GetClassInstance(paramName, paramValue, new DefaultTextCreator(), this);
      return textProducer;
   }

   public char[] GetTextProducerCharString()
   {
      String paramName = CaptchaConstants.CAPTCHA_TEXTPRODUCER_CHAR_STRING;
      String paramValue = _configuration.GetValue<String>(paramName);
      return _configHelper.GetChars(paramName, paramValue, "abcde2345678gfynmnpwx".ToCharArray());
   }

   public int GetTextProducerCharLength()
   {
      String paramName = CaptchaConstants.CAPTCHA_TEXTPRODUCER_CHAR_LENGTH;
      String paramValue = _configuration.GetValue<String>(paramName);
      return _configHelper.GetPositiveInt(paramName, paramValue, 5);
   }

   public Font[] GetTextProducerFonts(int fontSize) {
      String paramName = CaptchaConstants.CAPTCHA_TEXTPRODUCER_FONT_NAMES;
      String paramValue = _configuration.GetValue<String>(paramName);;
      return _configHelper.GetFonts(paramName, paramValue, fontSize, new Font[]{new Font("Arial",  fontSize), new Font("Courier",  fontSize)});
   }
}