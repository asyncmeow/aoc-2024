using System.Diagnostics;

namespace AdventOfCode;

public sealed class Day07 : BaseDay
{
    public List<(long Result, int[] Operands)> Problems { get; } = [];

    public Day07()
    {
        foreach (var line in File.ReadAllLines(InputFilePath))
        {
            var lineSplit = line.Split(": ");
            var result = long.Parse(lineSplit[0]);
            var operands = lineSplit[1].Split(' ').Select(int.Parse).ToArray();
            Problems.Add((Result: result, Operands: operands));
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var total = 0L;
        Parallel.ForEach(Problems, (problem) =>
        {
            if (CheckIsSolvable(problem.Operands[0], problem.Operands[1..], problem.Result))
                Interlocked.Add(ref total, problem.Result);
        });
        return new ValueTask<string>(total.ToString());
    }
    
    public override ValueTask<string> Solve_2()
    {
        var total = 0L;
        Parallel.ForEach(Problems, (problem) =>
        {
            if (CheckIsSolvable(problem.Operands[0], problem.Operands[1..], problem.Result, true))
                Interlocked.Add(ref total, problem.Result);
        });
        return new ValueTask<string>(total.ToString());
        
    }
    
    private bool CheckIsSolvable(long first, int[] operands, long result, bool withConcat = false)
    {
        if (operands.Length == 0) return first == result;
        
        var digits = Math.Floor(Math.Log10(operands[0])) + 1;
        var concat = (long)(first * Math.Pow(10, digits) + operands[0]);
        
        return CheckIsSolvable(first + operands[0], operands[1..], result, withConcat) || 
               CheckIsSolvable(first * operands[0], operands[1..], result, withConcat) ||
               (withConcat && CheckIsSolvable(concat, operands[1..], result, true));
    }
    
    private IEnumerable<bool[]> GetCombinations(int size)
    {
        var max = Math.Pow(2, size - 1);
        for (var i = 0; i < max; i++)
        {
            var ops = new bool[size - 1];
            for (var i2 = 0; i2 < size - 1; i2++)
            {
                ops[i2] = i.IsBitSet(i2);
            }

            yield return ops;
        }
    }
    
}