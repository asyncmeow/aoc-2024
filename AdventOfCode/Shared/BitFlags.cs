namespace AdventOfCode.Shared;

public static class BitFlags
{
    public static bool IsBitSet(this int integerValue, int bitNumber)
    {
        return (integerValue & (1 << bitNumber)) != 0;
    } 
}