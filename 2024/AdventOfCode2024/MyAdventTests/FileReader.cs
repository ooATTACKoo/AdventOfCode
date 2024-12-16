using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MyAdventTests
{
    public static class FileReader
    {
        const string relativePath= @"../../../../Inputs/";
        public static List<List<char>> LoadFileIntoAStringMatrix(string file)
        {
            var lines = System.IO.File.ReadAllLines(relativePath+file);

            List<List<char>> allRows = new List<List<char>>();
            foreach (var line in lines)
            {
                List<char> oneRow = new List<char>();
                // Split the string by whitespace
                foreach (var charvar in line)
                {
                    oneRow.Add(charvar);
                }
                allRows.Add(oneRow);
            }
            return allRows;
        }

        public static (List<List<char>>,string) LoadFileIntoAStringMatrixWithMoves(string file)
        {
            var lines = System.IO.File.ReadAllLines(relativePath + file);
            string moves ="";
            List<List<char>> allRows = new List<List<char>>();
            foreach (var line in lines)
            {
                List<char> oneRow = new List<char>();
                if (line.StartsWith("#"))
                {                 // Split the string by whitespace
                    foreach (var charvar in line)
                    {
                        oneRow.Add(charvar);
                    }
                    allRows.Add(oneRow);
                } else
                {
                    if (line.Length > 2)
                    {
                        line.Remove(line.Length-1, 1);
                        moves += line;
                    }
                }

            }
            return (allRows,moves);
        }

        public static List<int> LoadFileIntoAList(string file)
        {
            var lines = System.IO.File.ReadAllLines(relativePath+file);
            List<int> number1 = new List<int>();
            foreach (var line in lines)
            {


                // Parse the parts into integers
                number1.Add(int.Parse(line));
            }
            return number1;
        }


        public static (List<int>, List<int>) LoadFileIntoToLists(string file, bool sort, char c)
        {
            var lines = System.IO.File.ReadAllLines(relativePath+file);
            List<int> number1 = new List<int>();
            List<int> number2 = new List<int>();
            foreach (var line in lines)
            {
                // Split the string by whitespace
                string[] parts = line.Split(new[] { c }, StringSplitOptions.RemoveEmptyEntries);

                // Parse the parts into integers
                number1.Add(int.Parse(parts[0]));
                number2.Add(int.Parse(parts[1]));
            }

            if (sort)
            {
                number1.Sort();
                number2.Sort();
            }

            return (number1, number2);
        }

        public static List<List<int>> LoadFileIntoAMatrix(string file, char c)
        {
            var lines = System.IO.File.ReadAllLines(relativePath +  file);

            List<List<int>> allRows = new List<List<int>>();
            foreach (var line in lines)
            {
                List<int> oneRow = new List<int>();
                // Split the string by whitespace
                string[] parts = line.Split(new[] { c }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    oneRow.Add(int.Parse(part));
                }
                allRows.Add(oneRow);
            }
            return allRows;
        }

        public static List<List<int>> LoadFileIntoAMatrix(string file)
        {
            var lines = System.IO.File.ReadAllLines(relativePath + file);

            List<List<int>> allRows = new List<List<int>>();
            foreach (var line in lines)
            {
                List<int> oneRow = new List<int>();
                // Split the string by whitespace
                foreach (var part in line)
                {
                    oneRow.Add(int.Parse(part.ToString()));
                }
                allRows.Add(oneRow);
            }
            return allRows;
        }

        public static List<(long, List<int>)> LoadFileResultInput(string v)
        {
            var lines = System.IO.File.ReadAllLines(relativePath+v);
            List<(long, List<int>)> data = new List<(long, List<int>)>();
            foreach (var line in lines)
            {
                // Split the string by whitespace
                string[] parts = line.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                long result = long.Parse(parts[0]);
                List<int> numbersList = new List<int>();
                string[] numbers = parts[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var number in numbers)
                {
                    numbersList.Add(int.Parse(number));
                }
                data.Add((result, numbersList));
            }
            return data;

        }

        public static string LoadFileIntoOneString(string v)
        {
            return System.IO.File.ReadAllText(relativePath+v);
        }

        internal static List<((int, int), (int, int), (int,int))> LoadButtonInformation(string v)
        {
            var lines = System.IO.File.ReadAllLines(relativePath + v);
            (int, int) buttonA = (0,0);
            (int, int) buttonB = (0,0);
            (int,int) goal = (0,0);
            List<((int, int), (int, int), (int, int))> data = new List<((int, int), (int, int), (int, int))>();
            foreach (var line in lines)
            {
                // Split the string by whitespace
               if (line.StartsWith("Button A:"))
                {
                    buttonA=CreateAButton(line);
                    continue;
                }
               if (line.StartsWith("Button B:"))
                {
                    buttonB = CreateAButton(line);
                    continue;
                }
               if (line.StartsWith("Prize:"))
                {
                    string pattern = @"X[=+](\d+), Y[=+](\d+)";
                    Match match = Regex.Match(line, pattern);
                    int goalX = int.Parse(match.Groups[1].Value);
                    int goalY = int.Parse(match.Groups[2].Value);

                    goal = (goalX, goalY);
                    continue;
                }
               data.Add((buttonA, buttonB, goal));
            }
            if (!data.Contains((buttonA, buttonB, goal)))
            {
                data.Add((buttonA, buttonB, goal));
            }
            return data;
        }

        private static (int, int) CreateAButton(string line)
        {
            string pattern = @"X\+(\d+), Y\+(\d+)";

            Match match = Regex.Match(line, pattern);
            string buttonx = match.Groups[1].Value;
            string buttony = match.Groups[2].Value;
            return (int.Parse(buttonx), int.Parse(buttony));
        }

        internal static List<((int, int), (int, int))> LoadRobotsFromFile(string v)
        {
            var lines = System.IO.File.ReadAllLines(relativePath + v);
            (int, int) position = (0, 0);
            (int, int) valocity = (0, 0);
            var robots = new List<((int, int), (int, int))>();
            foreach (var line in lines)
            {
                string pattern = @"p=(-?\d+),\s*(-?\d+)\s*v=(-?\d+),\s*(-?\d+)";
                Match match = Regex.Match(line, pattern);
                int x = int.Parse(match.Groups[1].Value);
                int y = int.Parse(match.Groups[2].Value);
                position = (x, y);
                int vx = int.Parse(match.Groups[3].Value);
                int vy = int.Parse(match.Groups[4].Value);
                valocity = (vx, vy);
                robots.Add((position, valocity));
            }
            return robots;
        }
    }
}
