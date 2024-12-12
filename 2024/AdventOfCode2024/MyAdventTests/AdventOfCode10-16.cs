using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;

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

        [TestMethod]
        public void Day11A()
        {
            var matrix = FileReader.LoadFileIntoAMatrix("11A.txt", ' ');
            var data = new List<long>();
            foreach (var item in matrix[0])
            {
                data.Add(item);
            }

            for (int i = 0; i < 25; i++)
            {
                var newdata = new List<long>();
                foreach (var item in data)
                {
                    if (item == 0)
                    {
                        // replace 0 with 1
                        newdata.Add(1);
                        continue;
                    }
                    var length = item.ToString().Length;
                    if (length % 2 == 0)
                    {
                        // divide the number in two if even
                        var powerOfTen = (int)Math.Pow(10, (double)length / 2);
                        var second = item % powerOfTen;
                        var first = (item - second) / powerOfTen;
                        newdata.Add(first);
                        newdata.Add(second);
                        continue;
                    }
                    long newitem = item * 2024;
                    newdata.Add(newitem);

                }
                data = newdata;
            }

            Assert.AreEqual(189547, data.Count);
        }

        [TestMethod]
        public void Day11B()
        {
            var matrix = FileReader.LoadFileIntoAMatrix("11A.txt", ' ');
            var data = new List<long>();
            int iterations = 75;
            long totals = 0;
            foreach (var item in matrix[0])
            {
                File.AppendAllText(@"07AOut.txt", item + ": " + DateTime.Now.ToString() + "\n");
                totals += ExecuteAStoneBlinck(item, iterations);
            }

            Assert.AreEqual(224577979481346, totals);
        }

        [TestMethod]
        public void Day12A()
        {

            var matrix = FileReader.LoadFileIntoAStringMatrix("12A.txt");
            for (int x = 0; x < matrix.Count; x++)
            {
                for (int y = 0; y < matrix[0].Count; y++)
                {
                    if (visitedPositions.Contains((x, y)))
                    {
                        continue;
                    }
                    var areainfo = (1, 0);
                    var area = (x, y);
                    areaInformations[area] = areainfo;
                    MapArea(matrix, x, y, area);
                }
            }

            int total = 0;
            foreach (var area in areaInformations)
            {
                total += area.Value.Item1 * area.Value.Item2;
            }

            Assert.AreEqual(1415378, total);
        }

        [TestMethod]
        public void Day12B()
        {

            var matrix = FileReader.LoadFileIntoAStringMatrix("12A.txt");
            for (int x = 0; x < matrix.Count; x++)
            {
                for (int y = 0; y < matrix[0].Count; y++)
                {
                    if (visitedPositions.Contains((x, y)))
                    {
                        continue;
                    }
                    var areainfo = (1, 0);
                    var area = (x, y);
                    areaInformations[area] = areainfo;
                    areaInformationsBoarderFields[area] = new HashSet<(int, int)>();
                    areaInformationsFields[area] = new HashSet<(int, int)>() { area };
                    MapArea(matrix, x, y, area);
                }
            }

            int total = 0;
            foreach (var area in areaInformations)
            {
                int boarder = 0;
                var boarderfields = areaInformationsBoarderFields[area.Key];
                var areafields = areaInformationsFields[area.Key];
                foreach (var boarderfield in boarderfields)
                {
                    int field = 0;
                    //check if the boarderfield is in normal direction
                    int found = 0;
                    List<(int, int)> foundfields = new List<(int, int)>();
                    foreach (var direction in directions)
                    {

                        int newx = boarderfield.Item1 + direction.Item1;
                        int newy = boarderfield.Item2 + direction.Item2;

                        if (areafields.Contains((newx, newy)))
                        {
                            foundfields.Add((newx, newy));
                        }
                    }

                    // find innen ecken
                    if (foundfields.Count > 1)
                    {
                        foreach (var foundfield in foundfields)
                        {
                            foreach (var otherfield in foundfields)
                            {
                                if (foundfield != otherfield)
                                {
                                    if (corners.Contains((foundfield.Item1 - otherfield.Item1, foundfield.Item2 - otherfield.Item2)))
                                    {

                                        field++;
                                    }
                                }
                            }
                        }
                    }


                    foreach (var corner in corners)
                    {
                        int newx = boarderfield.Item1 + corner.Item1;
                        int newy = boarderfield.Item2 + corner.Item2;
                        if (boarderfields.Contains((newx, newy)))
                        {
                             
                            var corner1 = (newx - corner.Item1, newy);
                            var corner2 = (newx, newy - corner.Item2);
                            if (areafields.Contains(corner1) && !areafields.Contains(corner2))
                            {
                                field++;
                            } else if (!areafields.Contains(corner1) && areafields.Contains(corner2))
                            { field++; }

                           
                        }
                    }
                    boarder += field;
                }
                total = total + (area.Value.Item1 * (boarder / 2));
            }

            Assert.AreEqual(862714, total);
        }

        private readonly List<(int, int)> corners = new List<(int, int)>
        {
            (-1,-1),

            (-1,1),
            (1,-1),
                        (1,1),
        };

        private void MapArea(List<List<char>> matrix, int x, int y, (int, int) area)
        {
            if (visitedPositions.Contains((x, y)))
            {
                return;
            }

            visitedPositions.Add((x, y));

            foreach (var direction in directions)
            {
                int newx = x + direction.Item1;
                int newy = y + direction.Item2;

                if (newx >= 0 && newx < matrix.Count &&
                                       newy >= 0 && newy < matrix[0].Count)
                {
                    if (matrix[newx][newy] == matrix[x][y])
                    {

                        if (visitedPositions.Contains((newx, newy)))
                        {
                            continue;
                        }
                        HashSet<(int, int)> areafields;
                        if (areaInformationsFields.TryGetValue(area, out areafields))
                        {
                            areafields.Add((newx, newy));
                            areaInformationsFields[area] = areafields;
                        }

                        (int, int) areaInfonew;
                        if (areaInformations.TryGetValue(area, out areaInfonew))
                        {
                            areaInfonew.Item1 += 1;
                            areaInformations[area] = areaInfonew;
                        } else
                        {
                            Assert.Fail("Area not found");
                        }

                        MapArea(matrix, newx, newy, area);
                        continue;
                    }
                }
                if (areaInformations.TryGetValue(area, out (int, int) areaInfo))
                {
                    areaInfo.Item2 += 1;
                    areaInformations[area] = areaInfo;
                }
                HashSet<(int, int)> boarderfields;
                if (areaInformationsBoarderFields.TryGetValue(area, out boarderfields))
                {
                    boarderfields.Add((newx, newy));
                    areaInformationsBoarderFields[area] = boarderfields;
                }
            }
        }

        List<(int, int)> visitedPositions = new List<(int, int)>();
        Dictionary<(int, int), (int, int)> areaInformations = new Dictionary<(int, int), (int, int)>();
        Dictionary<(int, int), HashSet<(int, int)>> areaInformationsBoarderFields = new Dictionary<(int, int), HashSet<(int, int)>>();
        Dictionary<(int, int), HashSet<(int, int)>> areaInformationsFields = new Dictionary<(int, int), HashSet<(int, int)>>();

        Dictionary<(long, int), long> knownresults = new Dictionary<(long, int), long>();

        private long ExecuteAStoneBlinck(long item, int iterations)
        {
            if (knownresults.ContainsKey((item, iterations)))
            {
                return knownresults[(item, iterations)];
            }
            if (iterations == 0)
            {
                return 1;
            }
            if (item == 0)
            {
                // replace 0 with 1
                if (iterations == 1)
                {
                    return 1;
                }

                var result = ExecuteAStoneBlinck(2024, iterations - 2);
                AddToKnownResults(item, iterations, result);
                return result;
            }
            var length = item.ToString().Length;
            if (length % 2 == 0)
            {
                // divide the number in two if even
                var powerOfTen = (int)Math.Pow(10, (double)length / 2);
                var second = item % powerOfTen;
                var first = (item - second) / powerOfTen;
                var a = ExecuteAStoneBlinck(first, iterations - 1);
                var b = ExecuteAStoneBlinck(second, iterations - 1);
                AddToKnownResults(item, iterations, a + b);
                return a + b;
            } else
            {
                var olditem = item;
                int counter = 0;
                while (item.ToString().Length % 2 != 0 && iterations - counter > 0)
                {
                    item = item * 2024;
                    counter++;

                }
                var res = ExecuteAStoneBlinck(item, iterations - counter);
                AddToKnownResults(olditem, iterations, res);
                return res;
            }
        }

        private void AddToKnownResults(long item, int iterations, long res)
        {
            if (!knownresults.ContainsKey((item, iterations)))
            {
                knownresults.Add((item, iterations), res);
            }
        }

        private static List<(int, int)> FindAllZeroPositions(List<List<int>> matrix)
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

        private void FindNextPosition(List<List<int>> matrix, (int, int) item, int currentlevel, bool countduplicates, ref List<(int, int)> goals)
        {
            int nextLevel = currentlevel + 1;
            if (currentlevel == 9)
            {

                if (!goals.Contains(item) || countduplicates)
                { goals.Add(item); }

            } else
            {
                // we are not at level 9 yet, go in all directions
                foreach (var direction in directions)
                {
                    int newx = item.Item1 + direction.Item1;
                    int newy = item.Item2 + direction.Item2;
                    // check if the direction is valid
                    if (newx >= 0 && newx < matrix.Count &&
                        newy >= 0 && newy < matrix[0].Count)
                    {
                        // are we on the right level?
                        int checkhigh = matrix[newx][newy];
                        if (checkhigh == nextLevel)
                        {
                            FindNextPosition(matrix, (newx, newy), nextLevel, countduplicates, ref goals);
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



