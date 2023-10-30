using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Greta.BO.BusinessLogic.Extensions;

public static class SugestionExtentions
{
    
    /// <summary>
    /// Calculate the levenshtein distance between two strings
    /// i do some change for addapt to mapping work
    /// </summary>
    /// <param name="s"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static int LevenshteinDistance(this string s, string t)
    {
        if (string.IsNullOrEmpty(s))
        {
            //return string.IsNullOrEmpty(t) ? 0 : t.Length;
            return int.MaxValue;
        }

        if (string.IsNullOrEmpty(t))
        {
            // return s.Length;
            return int.MaxValue;
        }
        var distances = new int[s.Length + 1, t.Length + 1];
        for (var i = 0; i <= s.Length; i++)
        {
            distances[i, 0] = i;
        }        for (var j = 0; j <= t.Length; j++)
        {
            distances[0, j] = j;
        }
        for (var i = 1; i <= s.Length; i++)
        {
            for (var j = 1; j <= t.Length; j++)
            {
                var cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                distances[i, j] = Math.Min(Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1), distances[i - 1, j - 1] + cost);
            }
        }

        return distances[s.Length, t.Length];
    }
    
    /// <summary>
    /// Remove all special characters and spaces
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string Normalize(this string input)
    {
        return Regex.Replace(input.ToLower().Trim(), @"[^a-z0-9]", "");
    }

}