using System.Security.AccessControl;

namespace AdventOfCode;

public sealed class Day06 : BaseDay
{
    public char[][] Map { get; }
    public Point StartingPosition { get; }

    public Day06()
    {
        Map = File.ReadAllLines(InputFilePath).Select(l => l.ToArray()).ToArray();
        StartingPosition = FindStartingPosition();
    }

    private char GetCharAt(Point point)
    {
        return Map[point.Y][point.X];
    }
    
    private Point FindStartingPosition()
    {
        for (var y = 0; y < Map.Length; y++)
        {
            for (var x = 0; x < Map[y].Length; x++)
            {
                var point = new Point(x, y);
                var symbol = GetCharAt(point);
                if (symbol != '^') continue;
                Map[y][x] = '.';
                return point;
            }
        }

        throw new Exception("no starting point");
    }

    private bool IsOnMap(Point point)
    {
        return point.X >= 0 && point.Y >= 0 && point.Y < Map.Length && point.X < Map[point.Y].Length;
    }

    private List<Point>? GetStepsToEnd(Point? extraObstacle = null)
    {
        var position = StartingPosition;
        var facing = FourDirection.North;
        var occupiedSpots = new List<Point>();
        var occupiedSpotsWithFacing = new List<(Point, FourDirection)>();
        
        while (true)
        {
            if (!occupiedSpots.Contains(position))
                occupiedSpots.Add(position);
            
            var facingTuple = (position, facing);
            if (!occupiedSpotsWithFacing.Contains(facingTuple))
            {
                occupiedSpotsWithFacing.Add(facingTuple);
            }
            else
            {
                return null;
            }

            var facingPoint = position.GetPointInDirection(facing);
            if (!IsOnMap(facingPoint))
                break;
            
            if (facingPoint == extraObstacle)
            {
                facing = facing.RotateCW();
                continue;
            }
            
            var front = GetCharAt(facingPoint);
            switch (front)
            {
                case '.':
                    position = facingPoint;
                    break;
                case '#':
                    facing = facing.RotateCW();
                    break;
                default:
                    throw new InvalidOperationException("what the fuck is a " + front);
            }
        }

        return occupiedSpots;
    }
    
    public override ValueTask<string> Solve_1()
    {
        var occupiedSpots = GetStepsToEnd();
        return new ValueTask<string>(occupiedSpots?.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var occupiedSpots = GetStepsToEnd();
        var loops = 0;
        
        if (occupiedSpots == null)
        {
            return new ValueTask<string>("what the fuck");
        }
        
        Parallel.ForEach(occupiedSpots, point =>
        {
            Console.WriteLine($"{loops} - {point}");
            var occupiedSpotsWithLoops = GetStepsToEnd(point);
            if (occupiedSpotsWithLoops == null)
            {
                Interlocked.Increment(ref loops);
            }
        });

        return new ValueTask<string>(loops.ToString());
    }
}