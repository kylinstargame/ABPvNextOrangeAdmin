namespace ABPvNextOrangeAdmin.Utils;

public class StringUtils
{
    /**
     * 获取参数不为空值
     * 
     * @param value defaultValue 要判断的value
     * @return value 返回值
     */
    public static T Nvl<T>(T value, T defaultValue)
    {
        return value != null ? value : defaultValue;
    } 
}