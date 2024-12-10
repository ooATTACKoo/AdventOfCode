using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace MyAdventTests

{
    [TestClass]
    public class AdventOfCode10_16
    {
        [TestInitialize]
        public void Setup()
        {

        }

        [TestMethod]
        public void Day10A()
        {
            var matrix = FileReader.LoadFileIntoAMatrix("10A.txt");
            List<(int, int)> ZeroPositions = FindAllZeroPositions(matrix);

            int totals = 0;
            foreach (var item in ZeroPositions)
            {
                List<(int, int)> goals = new List<(int, int)>();
                FindNextPosition(matrix, item, 0, false, ref goals);
                
                    totals += goals.Count;
                
            }

            Assert.AreEqual(754, totals);
        }

        [TestMethod]
        public void Day10B()
        {
            var matrix = FileReader.LoadFileIntoAMatrix("10A.txt");
            List<(int, int)> ZeroPositions = FindAllZeroPositions(matrix);

            int totals = 0;
            foreach (var item in ZeroPositions)
            {
                List<(int, int)> goals = new List<(int, int)>();
                FindNextPosition(matrix, item, 0, true, ref goals);

                totals += goals.Count;

            }

            Assert.AreEqual(1609, totals);
        }

        private List<(int, int)> FindAllZeroPositions(List<List<int>> matrix)
        {
            var zeros = new List<(int, int)>();
            for (int x = 0; x < matrix.Count; x++)
            {
                for (int y = 0; y < matrix[0].Count; y++)
                {
                    if (matrix[x][y] == 0)
                    {
                        zeros.Add((x, y));
                    }
                }
            }
            return zeros;
        }

        private void FindNextPosition(List<List<int>> matrix, (int, int) item, int currentlevel, bool countduplicates,ref List<(int, int)> goals)
        {
            int nextLevel = currentlevel + 1;
            if (currentlevel == 9)
            {

               if (!goals.Contains(item) || countduplicates)
                { goals.Add(item); }

            } else
            {
                foreach (var direction in directions)
                {
                    int newx = item.Item1 + direction.Item1;
                    int newy = item.Item2 + direction.Item2;
                    // check if the direction is valid
                    if (newx >= 0 && newx < matrix.Count &&
                        newy >= 0 && newy < matrix[0].Count)
                    {
                        int checkhigh = matrix[newx][newy];
                        if (checkhigh == nextLevel)
                        {
                             FindNextPosition(matrix, (newx, newy), nextLevel,countduplicates, ref goals);
                        }
                    }
                }
            }
        }

        private readonly List<(int, int)> directions = new List<(int, int)> {
            (-1,0),
            (0,-1),
            (0,1),
            (1,0),
        };
    }
}



