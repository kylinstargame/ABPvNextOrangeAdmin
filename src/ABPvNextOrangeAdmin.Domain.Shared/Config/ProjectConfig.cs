using System;
using Volo.Abp.DependencyInjection;

namespace ABPvNextOrangeAdmin.Config;

/// <summary>
/// 项目设置
/// </summary>
public class ProjectConfig
{
    /** 项目名称 */
    public static string Name { get; set; }

    /** 版本 */
    public static string Version { get; set; }

    /** 版权年份 */
    public static string CopyrightYear { get; set; }

    /** 实例演示开关 */
    public static bool DemoEnabled { get; set; }

    /** 上传路径 */
    public static string Profile { get; set; }

    public static string CaptchaType { get; set; }
}