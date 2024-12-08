using System.Data;
using AdventOfCode.Shared;

namespace AdventOfCode;

public sealed class Day04 : BaseDay
{
    private MapGrid<char> WordSearch { get; }

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
    
    public Day04()
    {
        WordSearch = new MapGrid<char>(File.ReadAllLines(InputFilePath).Select(l => l.ToArray()).ToArray());
    }
    
    public override ValueTask<string> Solve_1()
    {
        var totalXmas = 0;
        foreach (var (point, _) in WordSearch.EachPoint())
        {
            foreach (var direction in Directions.Keys)
            {
                var hasXmas = CheckXmas(point, direction);
                if (hasXmas) totalXmas += 1;
            }
        }
        
        return new ValueTask<string>(totalXmas.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var totalMas = 0;
        
        foreach (var (point, _) in WordSearch.EachPoint())
        {
            var hasMas = CheckMas(point);
            if (hasMas) totalMas += 1;
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
        if (points.Any(p => !WordSearch.IsOnMap(p)))
            return false;

        var chars = new string(points.Select(WordSearch.GetElementAt).ToArray());
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
        
        if (points.Any(p => !WordSearch.IsOnMap(p)))
            return false;
        var chars = new string(points.Select(WordSearch.GetElementAt).ToArray());
        
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
}