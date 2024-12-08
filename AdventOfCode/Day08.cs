namespace AdventOfCode;

public sealed class Day08 : BaseDay
{
    private MapGrid<char> Map { get; }

    private Dictionary<char, List<Point>> Antennas { get; } = [];

    public Day08()
    {
        Map = new MapGrid<char>(File.ReadAllLines(InputFilePath).Select(l => l.ToArray()).ToArray());
    }

    private void FindAntennas()
    {
        if (Antennas.Count > 0) return;

        foreach (var (point, c) in Map.EachPoint())
        {
            if (c == '.') continue;
            if (!Antennas.ContainsKey(c))
                Antennas.Add(c, []);
            Antennas[c].Add(point);
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
                    antinodes.AddRange(new_antinodes.Where(Map.IsOnMap));
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
                        if (Map.IsOnMap(newPoint))
                            newAntinodes.Add(newPoint);
                        lastPoint = newPoint;
                    } while (Map.IsOnMap(lastPoint));
                    
                    antinodes.AddRange(newAntinodes.Where(Map.IsOnMap));
                    antinodes = antinodes.Distinct().ToList();
                }
            }
        }
        
        return new ValueTask<string>(antinodes.Count.ToString());
    }
}