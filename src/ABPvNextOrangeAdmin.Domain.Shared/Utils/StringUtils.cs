using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using ABPvNextOrangeAdmin.Constans;

namespace ABPvNextOrangeAdmin.Utils;

public class StringUtils
{
    /** 空字符串 */
    public static readonly String NULLSTR = "";

    /** 下划线 */
    public static readonly char SEPARATOR = '_';
    
    /** 下划线 */
    public static readonly String EMPTY = "";

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
    
      /**
     * * 判断一个Collection是否为空， 包含List，Set，Queue
     * 
     * @param coll 要判断的Collection
     * @return true：为空 false：非空
     */
    public static Boolean IsEmpty<T>(Collection<T> coll)
    {
        return IsNull(coll) || coll.Count == 0;
    }

    /**
     * * 判断一个Collection是否非空，包含List，Set，Queue
     * 
     * @param coll 要判断的Collection
     * @return true：非空 false：空
     */
    public static Boolean IsNotEmpty<T>(Collection<T> coll)
    {
        return !IsEmpty(coll);
    }

    /**
     * * 判断一个对象数组是否为空
     * 
     * @param objects 要判断的对象数组
     ** @return true：为空 false：非空
     */
    public static Boolean IsEmpty(Object[] objects)
    {
        return IsNull(objects) || (objects.Length == 0);
    }

    /**
     * * 判断一个对象数组是否非空
     * 
     * @param objects 要判断的对象数组
     * @return true：非空 false：空
     */
    public static Boolean IsNotEmpty(Object[] objects)
    {
        return !IsEmpty(objects);
    }

    /**
     * * 判断一个Map是否为空
     * 
     * @param map 要判断的Map
     * @return true：为空 false：非空
     */
    public static Boolean IsEmpty<T1,T2>(Dictionary<T1, T2> map)
    {
        return IsNull(map) || map.Count == 0;
    }

    /**
     * * 判断一个Map是否为空
     * 
     * @param map 要判断的Map
     * @return true：非空 false：空
     */
    public static Boolean IsNotEmpty<T1,T2>(Dictionary<T1, T2> map)
    {
        return !IsEmpty(map);
    }

    /**
     * * 判断一个字符串是否为空串
     * 
     * @param str String
     * @return true：为空 false：非空
     */
    public static Boolean IsEmpty(String str)
    {
        return IsNull(str) || NULLSTR.Equals(str.Trim());
    }

    /**
     * * 判断一个字符串是否为非空串
     * 
     * @param str String
     * @return true：非空串 false：空串
     */
    public static Boolean IsNotEmpty(String str)
    {
        return !IsEmpty(str);
    }

    /**
     * * 判断一个对象是否为空
     * 
     * @param object Object
     * @return true：为空 false：非空
     */
    public static Boolean IsNull(Object @object)
    {
        return @object == null;
    }

    /**
     * * 判断一个对象是否非空
     * 
     * @param object Object
     * @return true：非空 false：空
     */
    public static Boolean IsNotNull(Object @object)
    {
        return !IsNull(@object);
    }

    /**
     * * 判断一个对象是否是数组类型（Java基本型别的数组）
     * 
     * @param object 对象
     * @return true：是数组 false：不是数组
     */
    public static Boolean IsArray(Object @object)
    {
        return IsNotNull(@object) && @object is Array;
    }


    public static string CapitalizeFirstLetter(String input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
    }

    public static String Capitalize(String str)
    {
        int strLen = str.Length;
        if (strLen == 0)
        {
            return str;
        }
        else
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }
    }

    /**
     * 是否为http(s)://开头
     * 
     * @param link 链接
     * @return 结果
     */
    public static Boolean IsHTTP(String link)
    {
        return StringUtils.StartsWithAny(link, CommonConstants.HTTP, CommonConstants.HTTPS);
    }

    private static bool StartsWithAny(string sequence, params string[] searchStrings)
    {
        if (!String.IsNullOrEmpty(sequence) && searchStrings.Length > 0)
        {
            for (int i = 0; i < searchStrings.Length; ++i)
            {
                if (sequence.StartsWith(searchStrings[i]))
                {
                    return true;
                }
            }

            return false;
        }
        else
        {
            return false;
        }
    }

    public static string ReplaceEach(string text, string[] searchList, string[] replacementList)
    {
        return ReplaceEach(text, searchList, replacementList, false);
    }

    private static String ReplaceEach(String text, String[] searchList, String[] replacementList, Boolean repeat)
    {
        if (String.IsNullOrEmpty(text) || searchList.IsEmpty() || replacementList.IsEmpty())
        {
            return text;
        }
        else
        {
            if (searchList.Length != replacementList.Length)
            {
                throw new System.Exception("Search and Replace array lengths don't match: " + searchList.Length +
                                           " vs " + replacementList.Length);
            }

            string newText = text;
            int length = searchList.Length;
            for (int i = 0; i < length; i++)
            {
                newText = newText.Replace(searchList[i], replacementList[i]);
            }

            if (repeat)
            {
                text = newText;
            }

            return newText;
        }
    }
}

static class ArrayUtils
{
    public static Boolean IsEmpty<T>(this T[] array)
    {
        if (array == null || array.Length <= 0)
        {
            return true;
        }

        return false;
    }

    public static Boolean IsEmpty<T>(this List<T> array)
    {
        if (array == null || array.Count <= 0)
        {
            return true;
        }

        return false;
    }
}