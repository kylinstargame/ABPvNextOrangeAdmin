namespace ABPvNextOrangeAdmin.Constans;

public static class DeptConsts
{
    public static int MaxNameLength { get; set; } = 128;

    public const int MaxDepth = 16;

    public const int CodeUnitLength = 5;

    public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;
}