namespace AdventOfCode.Shared;

public enum EightDirection
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
}

public enum FourDirection
{
    North,
    East,
    South,
    West,
}

public static class DirectionExtensions {
    public static FourDirection RotateCW(this FourDirection direction)
    {
        return direction switch
        {
            FourDirection.North => FourDirection.East,
            FourDirection.East => FourDirection.South,
            FourDirection.South => FourDirection.West,
            FourDirection.West => FourDirection.North,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}