namespace AdventOfCode;

public sealed class Day08 : BaseDay
{
    private char[][] Map { get; }

    private Dictionary<char, List<Point>> Antennas { get; } = [];

    public Day08()
    {
        Map = File.ReadAllLines(InputFilePath).Select(l => l.ToArray()).ToArray();
        
    }

    private char GetCharAt(Point point)
    {
        return Map[point.Y][point.X];
    }

    private bool IsOnMap(Point point)
    {
        return point.X >= 0 && point.Y >= 0 && point.Y < Map.Length && point.X < Map[point.Y].Length;
    }

    private void FindAntennas()
    {
        if (Antennas.Count > 0) return;

        for (var y = 0; y < Map.Length; y++)
        {
            for (var x = 0; x < Map[y].Length; x++)
            {
                var point = new Point(x, y);
                var c = GetCharAt(point);
                if (c == '.') continue;
                if (!Antennas.ContainsKey(c))
                    Antennas.Add(c, []);
                Antennas[c].Add(point);
            }
        }
    }
    
    public override ValueTask<string> Solve_1()
    {
        FindAntennas();
        var antinodes = new List<Point>();
        foreach (var (frequency, points) in Antennas)
        {
            foreach (var point in points)
            {
                foreach (var point2 in points)
                {
                    if (point == point2) continue;
                    Point[] new_antinodes =
                    [
                        new Point(2*point.X - point2.X, 2*point.Y - point2.Y),
                        new Point(2*point2.X - point.X, 2*point2.Y - point.Y)
                    ];
                    antinodes.AddRange(new_antinodes.Where(IsOnMap));
                    antinodes = antinodes.Distinct().ToList();
                }
            }
        }
        
        return new ValueTask<string>(antinodes.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        FindAntennas();
        var antinodes = new List<Point>();
        foreach (var (frequency, points) in Antennas)
        {
            foreach (var point in points)
            {
                foreach (var point2 in points)
                {
                    if (point == point2) continue;
                    var newAntinodes = new List<Point>();
                    var slope = new Point(point.X - point2.X, point.Y - point2.Y);
                    var lastPoint = point2;
                    
                    do
                    {
                        var newPoint = lastPoint + slope;
                        if (IsOnMap(newPoint))
                            newAntinodes.Add(newPoint);
                        lastPoint = newPoint;
                    } while (IsOnMap(lastPoint));
                    
                    antinodes.AddRange(newAntinodes.Where(IsOnMap));
                    antinodes = antinodes.Distinct().ToList();
                }
            }
        }
        
        return new ValueTask<string>(antinodes.Count.ToString());
    }
}