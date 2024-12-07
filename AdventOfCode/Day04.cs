using System.Data;
using AdventOfCode.Shared;

namespace AdventOfCode;

public sealed class Day04 : BaseDay
{
    private static Dictionary<EightDirection, Point> Directions { get; } = new()
    {
        { EightDirection.North,     new Point( 0, -1) },
        { EightDirection.NorthEast, new Point( 1, -1) },
        { EightDirection.East,      new Point( 1,  0) },
        { EightDirection.SouthEast, new Point( 1,  1) },
        { EightDirection.South,     new Point( 0,  1) },
        { EightDirection.SouthWest, new Point(-1,  1) },
        { EightDirection.West,      new Point(-1,  0) },
        { EightDirection.NorthWest, new Point(-1, -1) }
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


    private bool CheckXmas(Point point, EightDirection direction)
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
            point + Directions[EightDirection.NorthWest],
            point + Directions[EightDirection.NorthEast],
            point,
            point + Directions[EightDirection.SouthEast],
            point + Directions[EightDirection.SouthWest]
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
}