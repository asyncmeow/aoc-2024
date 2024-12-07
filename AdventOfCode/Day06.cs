namespace AdventOfCode;

public sealed class Day06 : BaseDay
{
    private char[][] Map { get; set; }

    private Point Position { get; set; }
    private Direction Facing { get; set; } = Direction.North;
    private List<Point> OccupiedSpots { get; set; } = [];

    public Day06()
    {
        ResetState();
    }

    public void ResetState()
    {
        Map = File.ReadAllLines(InputFilePath).Select(l => l.ToArray()).ToArray();
        FindStartingPosition();
        OccupiedSpots = [];
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
                MoveTo(point);
                Map[y][x] = '.';
                return point;
            }
        }

        throw new Exception("no starting point");
    }

    private void MoveTo(Point point)
    {
        if (!OccupiedSpots.Contains(point))
            OccupiedSpots.Add(point);
        Position = point;
    }
    
    private bool IsOnMap(Point point)
    {
        return point.X >= 0 && point.Y >= 0 && point.Y < Map.Length && point.X < Map[point.Y].Length;
    }

    private Point GetPointInFront()
    {
        return Position + Directions[Facing];
    }
    
    public void MoveForward()
    {
        MoveTo(GetPointInFront());
    }
    
    private char GetCharAt(Point point)
    {
        return Map[point.Y][point.X];
    }
    
    public override ValueTask<string> Solve_1()
    {
        while (true)
        {
            var point = GetPointInFront();
            if (!IsOnMap(point))
            {
                break;
            }
            var front = GetCharAt(point);
            switch (front)
            {
                case '.':
                    MoveForward();
                    break;
                case '#':
                    Facing = NextDirection(Facing);
                    break;
                default:
                    throw new InvalidOperationException("what the fuck is a " + front);
            }
        }
        return new ValueTask<string>(OccupiedSpots.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        // this is terrible but it's 12:30 am and i'm lazy
        ResetState();
        var startingPosition = Position;
        var loopLocations = new List<Point>();
        for (var y = 0; y < Map.Length; y++)
        {
            for (var x = 0; x < Map[y].Length; x++)
            {
                var obstaclePoint = new Point(x, y);
                var originalChar = GetCharAt(obstaclePoint);
                Map[y][x] = '#';
                
                Position = startingPosition;
                List<(Point point, Direction direction)> occupiedSpotsWithDirection = [];

                var looped = false;
                while (true)
                {
                    var point = GetPointInFront();
                    if (!IsOnMap(point))
                        break;
                    var front = GetCharAt(point);
                    var duplicate = AddSpot(Position, Facing);
                    if (duplicate)
                    {
                        looped = true;
                        break;
                    }
                    
                    switch (front)
                    {
                        case '.':
                            MoveForward();
                            break;
                        case '#':
                            Facing = NextDirection(Facing);
                            break;
                        default:
                            throw new InvalidOperationException("what the fuck is a " + front);
                    }
                }

                Map[y][x] = originalChar;
                if (looped)
                {
                    loopLocations.Add(obstaclePoint);
                }

                continue;

                bool AddSpot(Point point, Direction direction)
                {
                    if (occupiedSpotsWithDirection.Contains((point, direction))) return true;
                    occupiedSpotsWithDirection.Add((point, direction));
                    return false;
                }
            }
        }
        return new ValueTask<string>(loopLocations.Count.ToString());
    }

    
    // TODO - refactor this out to a shared record
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
    
    
    private static Dictionary<Direction, Point> Directions { get; } = new()
    {
        { Direction.North,     new Point( 0, -1) },
        { Direction.East,      new Point( 1,  0) },
        { Direction.South,     new Point( 0,  1) },
        { Direction.West,      new Point(-1,  0) },
    };
    
    private enum Direction
    {
        North,
        East,
        South,
        West
    }
    
    private static Direction NextDirection(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}