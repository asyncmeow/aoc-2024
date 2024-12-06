using System.Data;

namespace AdventOfCode;

public sealed class Day04 : BaseDay
{
    private static Dictionary<Direction, Point> Directions { get; } = new()
    {
        { Direction.North,     new Point( 0, -1) },
        { Direction.NorthEast, new Point( 1, -1) },
        { Direction.East,      new Point( 1,  0) },
        { Direction.SouthEast, new Point( 1,  1) },
        { Direction.South,     new Point( 0,  1) },
        { Direction.SouthWest, new Point(-1,  1) },
        { Direction.West,      new Point(-1,  0) },
        { Direction.NorthWest, new Point(-1, -1) }
    };
    
    public char[][] WordSearch { get; set; }

    public Day04()
    {
        WordSearch = File.ReadAllLines(InputFilePath).Select(l => l.ToArray()).ToArray();
    }
    
    public override ValueTask<string> Solve_1()
    {
        var totalXmas = 0;
        for (var y = 0; y < WordSearch.Length; y++)
        {
            for (var x = 0; x < WordSearch[y].Length; x++)
            {
                var point = new Point(x, y);
                foreach (var direction in Directions.Keys)
                {
                    var hasXmas = CheckXmas(point, direction);
                    if (hasXmas) totalXmas += 1;
                }
            }
        }

        return new ValueTask<string>(totalXmas.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var totalMas = 0;
        for (var y = 0; y < WordSearch.Length; y++)
        {
            for (var x = 0; x < WordSearch[y].Length; x++)
            {
                var point = new Point(x, y);
                var hasMas = CheckMas(point);
                if (hasMas) totalMas += 1;
            }
        }

        return new ValueTask<string>(totalMas.ToString());
    }


    private bool CheckXmas(Point point, Direction direction)
    {
        List<Point> points =
        [
            point + Directions[direction] * 0,
            point + Directions[direction] * 1,
            point + Directions[direction] * 2,
            point + Directions[direction] * 3,
        ];
        if (points.Any(p => !PointInGrid(p)))
            return false;

        var chars = new string(points.Select(CharAtPoint).ToArray());
        return chars == "XMAS";
    }

    private bool CheckMas(Point point)
    {
        List<Point> points =
        [
            point + Directions[Direction.NorthWest],
            point + Directions[Direction.NorthEast],
            point,
            point + Directions[Direction.SouthEast],
            point + Directions[Direction.SouthWest]
        ];
        
        if (points.Any(p => !PointInGrid(p)))
            return false;
        var chars = new string(points.Select(CharAtPoint).ToArray());
        
        // for debugging
        if (chars.Contains('.')) 
            return false;
        
        // 0 . 1
        // . 2 .
        // 4 . 3
        var validStrings = new string[]
        {
            "MMASS",
            "SMAMS",
            "SSAMM",
            "MSASM"
        };
        
        return validStrings.Contains(chars);
    }
    private bool PointInGrid(Point point)
    {
        return point.X >= 0
               && point.Y >= 0
               && point.Y < WordSearch.Length
               && point.X < WordSearch[point.Y].Length;
    }

    private char CharAtPoint(Point point)
    {
        return WordSearch[point.Y][point.X];
    }
    private enum Direction
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

    private record Point(int X, int Y)
    {
        public static Point operator *(Point point, int amount)
        {
            return new Point(point.X * amount, point.Y * amount);
        }

        public static Point operator +(Point point1, Point point2)
        {
            return new Point(point1.X + point2.X, point1.Y + point2.Y);
        }
    }
}