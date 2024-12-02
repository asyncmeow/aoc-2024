namespace AdventOfCode;

public sealed class Day01 : BaseDay
{
    public int[] Left { get; set; }
    public int[] Right { get; set; }

    public Day01()
    {
        var inTuples = File.ReadAllLines(InputFilePath)
            .Select(l => l.Split("   "))
            .Select(l => (int.Parse(l[0]), int.Parse(l[1])))
            .ToList();
        Left = inTuples.Select(t => t.Item1).Order().ToArray();
        Right = inTuples.Select(t => t.Item2).Order().ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        var total = 0;
        for (var i = 0; i < Left.Length; i++)
        {
            total += Math.Abs(Left[i] - Right[i]);
        }

        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var total = 0;
        foreach (var item in Left)
        {
            var count = Right.Count(i => i == item);
            total += item * count;
        }

        return new ValueTask<string>(total.ToString());
    }
}
