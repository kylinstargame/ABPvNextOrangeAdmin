using System;
using System.Drawing;
using ABPvNextOrangeAdmin.Config;
using ABPvNextOrangeAdmin.CustomException;
using ABPvNextOrangeAdmin.Utils.ImageProducer;
using Volo.Abp.DependencyInjection;

namespace ABPvNextOrangeAdmin.Utils;

public class ConfigHelper : ITransientDependency
{
    public Color GetColor(String paramName, String paramValue, Color defaultColor)
    {
        Color color;
        if (!"".Equals(paramValue) && paramValue != null)
        {
            if (paramValue.IndexOf(",", StringComparison.Ordinal) > 0)
            {
                color = this.CreateColorFromCommaSeparatedValues(paramName, paramValue);
            }
            else
            {
                // color = this.createColorFromFieldValue(paramName, paramValue);
            }
        }
        else
        {
            color = defaultColor;
        }

        return color;
    }

    public Color CreateColorFromCommaSeparatedValues(String paramName, String paramValue)
    {
        String[] colorValues = paramValue.Split(",");

        try
        {
            int r = Convert.ToInt32(colorValues[0]);
            int g = Convert.ToInt32(colorValues[1]);
            int b = Convert.ToInt32(colorValues[2]);
            Color color;
            if (colorValues.Length == 4)
            {
                int a = Convert.ToInt32(colorValues[3]);
                color = Color.FromArgb(a, r, g, b);
            }
            else
            {
                if (colorValues.Length != 3)
                {
                    throw new ConfigException(paramName, paramValue,
                        "Color can only have 3 (RGB) or 4 (RGB with Alpha) values.");
                }

                color = Color.FromArgb(r, g, b);
            }

            return color;
        }
        catch (System.Exception ex)
        {
            throw new ConfigException(paramName, paramValue, ex);
        }
      
    }

    // public Color createColorFromFieldValue(String paramName, String paramValue)
    // {
    //     try
    //     {
    //         Field field = Class.forName("java.awt.Color").getField(paramValue);
    //         Color color = (Color)field.get((Object)null);
    //         return color;
    //     }
    //     catch (System.Exception ex)
    //     {
    //         throw new ConfigException(paramName, paramValue, ex);
    //     }
    // }

    public Object GetClassInstance(String paramName, String paramValue, Object defaultInstance, CaptchaConfig config)
    {
        Object instance;
        if (!"".Equals(paramValue) && paramValue != null)
        {
            try
            {
                instance = Activator.CreateInstance( Type.GetType(paramValue) ?? throw new InvalidOperationException());
            }
            catch (System.Exception ex)
            {
                throw new ConfigException(paramName, paramValue, ex);
            }
        }
        else
        {
            instance = defaultInstance;
        }
    
        this.SetConfigurable(instance, config);
        return instance;
    }

    public Font[] GetFonts(String paramName, String paramValue, int fontSize, Font[] defaultFonts)
    {
        Font[] fonts;
        if (!"".Equals(paramValue) && paramValue != null)
        {
            String[] fontNames = paramValue.Split(",");
            fonts = new Font[fontNames.Length];

            for (int i = 0; i < fontNames.Length; ++i)
            {
                fonts[i] = new Font(fontNames[i], fontSize);
            }
        }
        else
        {
            fonts = defaultFonts;
        }

        return fonts;
    }

    public int GetPositiveInt(String paramName, String paramValue, int defaultInt)
    {
        int intValue;
        if (!"".Equals(paramValue) && paramValue != null)
        {
            try
            {
                intValue = Convert.ToInt32(paramValue);
                if (intValue < 1)
                {
                    throw new ConfigException(paramName, paramValue, "Value must be greater than or Equals to 1.");
                }
            }
            catch (System.Exception ex)
            {
                throw new ConfigException(paramName, paramValue, ex);
            }
        }
        else
        {
            intValue = defaultInt;
        }

        return intValue;
    }

    public char[] GetChars(String paramName, String paramValue, char[] defaultChars)
    {
        char[] chars;
        if (!"".Equals(paramValue) && paramValue != null)
        {
            chars = paramValue.ToCharArray();
        }
        else
        {
            chars = defaultChars;
        }

        return chars;
    }

    public bool Getbool(String paramName, String paramValue, bool defaultValue)
    {
        bool boolValue;
        if (!"yes".Equals(paramValue) && !"".Equals(paramValue) && paramValue != null)
        {
            if (!"no".Equals(paramValue))
            {
                throw new ConfigException(paramName, paramValue, "Value must be either yes or no.");
            }

            boolValue = false;
        }
        else
        {
            boolValue = defaultValue;
        }

        return boolValue;
    }

    private void SetConfigurable(Object @object, CaptchaConfig config) {
        if (@object is CaptchaConfig) {
            ((IWithConfig)@object).SetConfig(config);
        }
    }
}