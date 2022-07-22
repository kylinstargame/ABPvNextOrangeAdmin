using System;

namespace ABPvNextOrangeAdmin.CustomException;

public class ConfigException : System.Exception
{
    public ConfigException(string paramName, string paramValue, System.Exception exception)
    {
        // throw new NotImplementedException();
    }

    public ConfigException(string paramName, string paramValue, string colorCanOnlyHaveRgbOrRgbWithAlphaValues)
    {
        // throw new NotImplementedException();
    }
}