namespace ABPvNextOrangeAdmin.CodeGen;

public class GenConfig
{
    /// <summary>
    /// 作者
    /// </summary>
    public static string author;

    /// <summary>
    /// 生成包路径 
    /// </summary>
    public static string packageName;

    /// <summary>
    /// 自动去除表前缀，默认是false
    /// </summary>
    public static bool autoRemovePre;

    /// <summary>
    /// 表前缀(类名不会包含表前缀)
    /// </summary>
    public static string tablePrefix;

    public static string GetAuthor()
    {
        return author;
    }

    public void SetAuthor(string author)
    {
        GenConfig.author = author;
    }

    public static string GetPackageName()
    {
        return packageName;
    }

    public void SetPackageName(string packageName)
    {
        GenConfig.packageName = packageName;
    }

    public static bool GetAutoRemovePre()
    {
        return autoRemovePre;
    }

    public void SetAutoRemovePre(bool autoRemovePre)
    {
        GenConfig.autoRemovePre = autoRemovePre;
    }

    public static string GetTablePrefix()
    {
        return tablePrefix;
    }

    public void SetTablePrefix(string tablePrefix)
    {
        GenConfig.tablePrefix = tablePrefix;
    }
}