using System;

namespace ABPvNextOrangeAdmin.Utils;

public class ConvertUtils
{
    public static Boolean ToBool(Object value, Boolean defaultValue = false)
    {
        if (value == null)
        {
            return defaultValue;
        }
        if (value is Boolean)
        {
            return (Boolean) value;
        }
        String valueStr = ToString(value, null);
        if (String.IsNullOrEmpty(valueStr))
        {
            return defaultValue;
        }
        valueStr = valueStr.Trim().ToLower();
        switch (valueStr)
        {
            case "true":
            case "yes":
            case "ok":
            case "1":
                return true;
            case "false":
            case "no":
            case "0":
                return false;
            default:
                return defaultValue;
        }
    }
    
    public static String ToString(Object value, String defaultValue = "")
    {
        if (null == value)
        {
            return defaultValue;
        }
        if (value is String)
        {
            return (String) value;
        }
        return value.ToString();
    }
 
}