using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var stoneCounts = new Dictionary<string, long>();

        // Read input from file and populate stoneCounts
        foreach (var stone in File.ReadAllText("C:\\Repos\\GitHub\\AdventOfCode\\2024\\Inputs\\11Test.txt").Trim().Split())
        {
            if (stoneCounts.ContainsKey(stone))
                stoneCounts[stone]++;
            else
                stoneCounts[stone] = 1;
        }

        // Transform stones 75 times
        for (int i = 0; i < 75; i++)
        {
            stoneCounts = TransformStones(stoneCounts);
        }

        // Print the sum of all stone counts
        Console.WriteLine(stoneCounts.Values.Sum());
    }

    static Dictionary<string, long> TransformStones(Dictionary<string, long> stoneCounts)
    {
        var newCounts = new Dictionary<string, long>();

        foreach (var kvp in stoneCounts)
        {
            string stone = kvp.Key;
            long count = kvp.Value;

            if (stone == "0")
            {
                if (newCounts.ContainsKey("1"))
                    newCounts["1"] += count;
                else
                    newCounts["1"] = count;
            } else if (stone.Length % 2 == 0)
            {
                int mid = stone.Length / 2;
                string left = stone.Substring(0, mid).TrimStart('0');
                string right = stone.Substring(mid).TrimStart('0');

                left = string.IsNullOrEmpty(left) ? "0" : left;
                right = string.IsNullOrEmpty(right) ? "0" : right;

                if (newCounts.ContainsKey(left))
                    newCounts[left] += count;
                else
                    newCounts[left] = count;

                if (newCounts.ContainsKey(right))
                    newCounts[right] += count;
                else
                    newCounts[right] = count;
            } else
            {
                string newStone = (int.Parse(stone) * 2024).ToString();
                if (newCounts.ContainsKey(newStone))
                    newCounts[newStone] += count;
                else
                    newCounts[newStone] = count;
            }
        }

        return newCounts;
    }
}