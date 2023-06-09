using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator
{
	public static List<string> Calculate(List<string> first, List<string> second)
	{
		var opt = CreateOptimizationTable(first, second);
        return RestoreAnswer(opt, first, second);

    }

	private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
	{
        var opt = new int[first.Count + 1, second.Count + 1];
        for (var i = first.Count - 1; i >= 0; i--)
        {
            for (var j = second.Count - 1; j >= 0; j--)
            {
                if (first[i] == second[j])
                    opt[i, j] = opt[i + 1, j + 1] + 1;
                else
                    opt[i, j] = Math.Max(opt[i + 1, j], opt[i, j + 1]);
            }
        }
        return opt;
    }

    private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
	{
        var res = new List<string>();
        GetPathFromStart(res, opt, first, second, 0, 0);
        return res;
    }

    private static void GetPathFromStart(List<string> res, int[,] opt, List<string> first, List<string> second, int i, int j)
    {

        if (i == first.Count || j == second.Count)
            return;
        if (first[i] == second[j])
        {
            res.Add(first[i]);
            GetPathFromStart(res, opt, first, second, i + 1, j + 1);
        }
        else if (opt[i, j] == opt[i + 1, j])
            GetPathFromStart(res, opt, first, second, i + 1, j);
        else
            GetPathFromStart(res, opt, first, second, i, j + 1);
    }
}