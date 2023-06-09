using NUnit.Framework;
using System;
using System.Collections.Generic;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
	public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
	{
        var result = new List<ComparisonResult>();
        for(var i = 0; i < documents.Count;i++)
        {
            for(var j=i + 1;j <  documents.Count;j++)
            {
                result.Add(GetLevenshteinLength(documents[i], documents[j]));
            }
        }
        return result;
	}

    public ComparisonResult GetLevenshteinLength(DocumentTokens first, DocumentTokens second)
    {
        var opt = new double[first.Count+1, second.Count+1];
        InitializeOPT(first, second, opt);
        FillOPT(first, second, opt);
        return new ComparisonResult(first, second, opt[first.Count, second.Count]);
    }

    public void InitializeOPT(DocumentTokens first, DocumentTokens second, double[,] opt)
    {
        for (var i = 0; i <= first.Count; ++i)
            opt[i, 0] = i;
        for (var i = 0; i <= second.Count; ++i) 
            opt[0, i] = i;
    }

    public void FillOPT(DocumentTokens first, DocumentTokens second, double[,] opt)
    {
        for (var i = 1; i <= first.Count; ++i)
        {
            for (var j = 1; j <= second.Count; ++j)
            {
                if (first[i - 1] == second[j - 1])
                    opt[i, j] = opt[i - 1, j - 1];
                else
                {
                    var distance = GetDistance(first[i - 1], second[j - 1]);
                    opt[i, j] = Math.Min(Math.Min(distance + opt[i - 1, j - 1], 1 + opt[i - 1, j]), 1 + opt[i, j - 1]);
                }
            }
        }
    }

    public double GetDistance(string first, string second)
    {
        return TokenDistanceCalculator.GetTokenDistance(first, second);
    }
}