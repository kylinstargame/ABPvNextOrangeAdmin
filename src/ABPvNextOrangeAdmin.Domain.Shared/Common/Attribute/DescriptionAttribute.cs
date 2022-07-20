using System;

namespace ABPvNextOrangeAdmin.Common.Attribute;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class DescriptionAttribute : System.Attribute
{
    public string value;

    public DescriptionAttribute(string value)
    {
        this.value = value;
    }
}