namespace AdventOfCode.Shared;

public class MapGrid<T>
{
    public T[][] Map { get; set; } = [];
    
    public MapGrid()
    {
    }

    public MapGrid(T[][] map)
    {
        Map = map;
    }

    public T GetElementAt(Point point)
    {
        return Map[point.Y][point.X];
    }

    public bool IsOnMap(Point point)
    {
        return point.X >= 0 && point.Y >= 0 && point.Y < Map.Length && point.X < Map[point.Y].Length;
    }

    public IEnumerable<(Point point, T element)> EachPoint()
    {
        for (var y = 0; y < Map.Length; y++)
        {
            for (var x = 0; x < Map[y].Length; x++)
            {
                var point = new Point(x, y);
                var element = GetElementAt(point);
                yield return (point, element);
            }
        }
    }
}