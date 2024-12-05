namespace AdventOfCode;

public sealed class Day05 : BaseDay
{
    private List<PageRule> Rules { get; set; } = [];
    private ILookup<int, PageRule> RulesLookup { get; set; }
    private List<int[]> Manuals { get; set; } = [];

    public Day05()
    {
        var handlingPageRules = true;
        foreach (var line in File.ReadAllLines(InputFilePath))
        {
            if (handlingPageRules)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    handlingPageRules = false;
                    continue;
                }

                var split = line.Split('|');
                Rules.Add(new PageRule(int.Parse(split[0]), int.Parse(split[1])));
            }
            else
            {
                Manuals.Add(line.Split(',').Select(int.Parse).ToArray());
            }
        }

        RulesLookup = Rules.ToLookup(r => r.Page);
    }
    
    public override ValueTask<string> Solve_1()
    {
        var middleSums = 0;
        foreach (var manual in Manuals)
        {
            if (!ManualValid(manual)) continue;
            // assumes that all lists are odd-numbered in length
            var middleIndex = (int)Math.Floor((decimal)manual.Length / 2);
            middleSums += manual[middleIndex];
        }

        return new ValueTask<string>(middleSums.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var middleSums = 0;
        foreach (var manual in Manuals)
        {
            if (ManualValid(manual)) continue;

            var newManual = ResortManual(manual);
            
            // assumes that all lists are odd-numbered in length
            var middleIndex = (int)Math.Floor((decimal)newManual.Length / 2);
            middleSums += newManual[middleIndex];
        }

        return new ValueTask<string>(middleSums.ToString());    }

    public record PageRule(int Required, int Page);

    public bool ManualValid(int[] manual)
    {
        var encounteredPages = new List<int>(manual.Length);
        foreach (var page in manual)
        {
            foreach (var rule in RulesLookup[page])
            {
                // if the manual has the required page, but we didn't see it yet, it's not valid
                if (manual.Contains(rule.Required) && !encounteredPages.Contains(rule.Required))
                    return false;
            }

            encounteredPages.Add(page);
        }

        return true;
    }

    public int[] ResortManual(int[] manual)
    {
        var newList = new List<int>(manual.Length);
        
        foreach (var page in manual)
        {
            foreach (var rule in RulesLookup[page])
            {
                if (manual.Contains(rule.Required) && !newList.Contains(rule.Required))
                {
                    newList.Add(rule.Required);
                }
            }

            if (!newList.Contains(page)) newList.Add(page);
        }

        if (!ManualValid(newList.ToArray()))
        {
            return ResortManual(newList.ToArray());
        }
        return newList.ToArray();
    }
}