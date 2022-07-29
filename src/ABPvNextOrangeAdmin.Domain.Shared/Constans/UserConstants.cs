using System;

namespace ABPvNextOrangeAdmin.Constans;

public class UserConstants
{
    /**
     * 平台内系统用户的唯一标志
     */
    public static readonly String SYS_USER = "SYS_USER";

    /** 正常状态 */
    public static readonly String NORMAL = "0";

    /** 异常状态 */
    public static readonly String EXCEPTION = "1";

    /** 用户封禁状态 */
    public static readonly String USER_DISABLE = "1";

    /** 角色封禁状态 */
    public static readonly String ROLE_DISABLE = "1";

    /** 部门正常状态 */
    public static readonly String DEPT_NORMAL = "0";

    /** 部门停用状态 */
    public static readonly String DEPT_DISABLE = "1";

    /** 字典正常状态 */
    public static readonly String DICT_NORMAL = "0";

    /** 是否为系统默认（是） */
    public static readonly String YES = "Y";

    /** 是否菜单外链（是） */
    public static readonly String YES_FRAME = "0";

    /** 是否菜单外链（否） */
    public static readonly String NO_FRAME = "1";

    /** 菜单类型（目录） */
    public static readonly String TYPE_DIR = "M";

    /** 菜单类型（菜单） */
    public static readonly String TYPE_MENU = "C";

    /** 菜单类型（按钮） */
    public static readonly String TYPE_BUTTON = "F";

    /** Layout组件标识 */
    public readonly static String LAYOUT = "Layout";
    
    /** ParentView组件标识 */
    public readonly static String PARENT_VIEW = "ParentView";

    /** InnerLink组件标识 */
    public readonly static String INNER_LINK = "InnerLink";

    /** 校验返回结果码 */
    public readonly static String UNIQUE = "0";
    public readonly static String NOT_UNIQUE = "1";

    /**
     * 用户名长度限制
     */
    public static readonly int USERNAME_MIN_LENGTH = 2;
    public static readonly int USERNAME_MAX_LENGTH = 20;

    /**
     * 密码长度限制
     */
    public static readonly int PASSWORD_MIN_LENGTH = 5;
    public static readonly int PASSWORD_MAX_LENGTH = 20; 
}