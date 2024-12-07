namespace AdventOfCode.Shared;

public record Point(int X, int Y)
{
    public static Point operator *(Point point, int amount)
    {
        return new Point(point.X * amount, point.Y * amount);
    }

    public static Point operator +(Point point1, Point point2)
    {
        return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }

    public Point GetPointInDirection(FourDirection direction)
    {
        return direction switch
        {
            FourDirection.North => new Point(X, Y - 1),
            FourDirection.East => new Point(X + 1, Y),
            FourDirection.South => new Point(X, Y + 1),
            FourDirection.West => new Point(X - 1, Y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}