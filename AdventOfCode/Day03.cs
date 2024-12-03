using System.Text.RegularExpressions;

namespace AdventOfCode;

public sealed partial class Day03 : BaseDay
{
    public string Program { get; set; }

    public Day03()
    {
        Program = File.ReadAllText(InputFilePath);
    }
    
    public override ValueTask<string> Solve_1()
    {
        var regex = MulRegex();
        var matches = regex.Matches(Program);
        var total = 0;
        foreach (Match match in matches)
        {
            total += int.Parse(match.Groups[1].ToString()) * int.Parse(match.Groups[2].ToString());
        }
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var regex = MulDoDontRegex();
        var matches = regex.Matches(Program);
        var total = 0;
        var mulEnabled = true;
        foreach (Match match in matches)
        {
            switch (match.Groups[0].ToString())
            {
                case "do()":
                    mulEnabled = true;
                    break;
                case "don't()":
                    mulEnabled = false;
                    break;
                default:
                {
                    if (match.Groups[0].ToString().StartsWith("mul(") && mulEnabled)
                    {
                        total += int.Parse(match.Groups[1].ToString()) * int.Parse(match.Groups[2].ToString());
                    }

                    break;
                }
            }
        }
        
        return new ValueTask<string>(total.ToString());
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)", RegexOptions.Compiled)]
    private static partial Regex MulDoDontRegex();
    [GeneratedRegex(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled)]
    private static partial Regex MulRegex();
}