namespace AdventOfCode.Shared;

public static class ArrayUtils
{
    public static string ToCommaSeparatedString<T>(this T[] arr)
    {
        return string.Join(',', arr.Select(o => o.ToString()));
    }

    public static string ToCommaSeparatedString(this bool[] arr)
    {
        return string.Join("", arr.Select(o => o ? "1" : "0"));
    }
}