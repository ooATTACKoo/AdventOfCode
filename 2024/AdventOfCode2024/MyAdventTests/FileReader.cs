using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
