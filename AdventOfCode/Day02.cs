namespace AdventOfCode;

public sealed class Day02 : BaseDay
{
    public int[][] Reports { get; set; }

    public Day02()
    {
        Reports = File.ReadAllLines(InputFilePath)
            .Select(l => l.Split(" ").Select(int.Parse).ToArray()).ToArray();
    }
    
    public override ValueTask<string> Solve_1()
    {
        var safe = Reports.Count(IsSafe);
        return new ValueTask<string>(safe.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var safe = Reports.Count(r =>
            // check if there are any versions of the array where if we remove a single element it's safe
            r.Where((v, i) =>
            {
                // make a copy of the array without an individual element
                var record = r.Where((v2, i2) => i2 != i).ToArray();
                return IsSafe(record);
            }).Any()
        );
        
        return new ValueTask<string>(safe.ToString());
    }

    public static bool IsSafe(int[] report)
    {
        var lastLevel = report[0];
        bool? increasing = null;

        for (var i = 1; i < report.Length; i++)
        {
            var level = report[i];
            var difference = level - lastLevel;
            var didIncrease = difference > 0;
            
            // only valid between 1 and 3 inclusive
            if (Math.Abs(difference) is <= 0 or > 3)
            {
                return false;
            }
            
            if (increasing == null)
            {
                increasing = didIncrease;
            }
            else
            {
                if (increasing != didIncrease)
                {
                    return false;
                }
            }
            lastLevel = level;

        }

        return true;
    }
}