using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MyAdventTests

{
    [TestClass]
    public class AdventOfCode
    {

        [TestInitialize]
        public void Setup()
        {

        }

        [TestCategory("L0")]
        [TestMethod]
        public void Day1A()
        {
            var (list1, list2) = FileReader.LoadFileIntoToLists(@"01A.txt", true, ' ');
            var total = 0;
            for (int i = 0; i < list1.Count; i++)
            {
                var diff = list1[i] - list2[i];
                total += Math.Abs(diff);
            }


            Assert.AreEqual(1834060, total);
        }

        [TestCategory("L0")]
        [TestMethod]
        public void Day1B()
        {
            var (list1, list2) = FileReader.LoadFileIntoToLists(@"01A.txt", true, ' ');
            var total = 0;
            for (int i = 0; i < list1.Count; i++)
            {
                var key = list1[i];
                int counter = 0;
                foreach (var item in list2)
                {
                    if (key == item)
                    {
                        counter++;
                        continue;
                    }
                    if (key < item)
                    {
                        break;
                    }
                }
                total += counter * key;
            }


            Assert.AreEqual(21607792, total);
        }

        [TestCategory("L0")]
        [TestMethod]
        public void Day2A()
        {
            var list1 = FileReader.LoadFileIntoAMatrix(@"02A.txt", ' ');
            var total = 0;
            foreach (var row in list1)
            {
                bool first = true;
                bool up = true;
                bool down = true;
                //bool ok = true;
                int before = 0;
                foreach (var item in row)
                {
                    if (first)
                    {
                        before = item;
                        first = false;
                        continue;
                    }

                    if (item < before && down && before - item < 4)
                    {
                        up = false;
                        before = item;
                        continue;
                    }

                    if (item > before && up && item - before < 4)
                    {
                        down = false;
                        before = item;
                        continue;
                    }

                    down = false;
                    up = false;
                    break;
                }
                if (up || down)
                {
                    total++;
                }
            }

            Assert.AreEqual(306, total);
        }

        [TestCategory("L0")]
        [TestMethod]
        public void Day2B()
        {
            var list1 = FileReader.LoadFileIntoAMatrix(@"02A.txt", ' ');
            var total = 0;
            foreach (var row in list1)
            {
                bool ok = IsRowOk(row);
                if (ok)
                {
                    total++;
                    continue;
                }

                bool shortrowOK = false;
                for (int i = 0; i < row.Count; i++)
                {
                    var checkrow = new List<int>(row);
                    checkrow.RemoveAt(i);
                    if (IsRowOk(checkrow))
                    {
                        shortrowOK = true;
                        break;
                    }
                }

                if (shortrowOK)
                {
                    total++;
                }
            }


            Assert.AreEqual(366, total);
        }

        [TestCategory("L0")]
        [TestMethod]
        public void Day3A()
        {
            string list1 = FileReader.LoadFileIntoOneString(@"03A.txt");
            var total = 0;
            while (list1.Length > 0)
            {
                var firstChar = list1[0];
                list1 = list1.Substring(1);
                if (firstChar == 'm')
                {
                    var stringUL = list1.Substring(0, 3);
                    if (stringUL == "ul(")
                    {
                        list1 = list1.Substring(3);
                        int n1 = FindNumber(ref list1);
                        if (list1[0] == ',')
                        {
                            list1 = list1.Substring(1);
                            int n2 = FindNumber(ref list1);
                            if (list1[0] == ')')
                            {
                                list1 = list1.Substring(1);
                                total = total + n1 * n2;
                            }
                        }
                    }
                }
            }

            Assert.AreEqual(163931492, total);
        }

        [TestCategory("L0")]
        [TestMethod]
        public void Day3B()
        {
            string list1 = FileReader.LoadFileIntoOneString(@"03A.txt");
            var total = 0;
            bool docalc = true;
            while (list1.Length > 0)
            {
                var firstChar = list1[0];
                list1 = list1.Substring(1);
                if (docalc && firstChar == 'm')
                {
                    var stringUL = list1.Substring(0, 3);
                    if (stringUL == "ul(")
                    {
                        list1 = list1.Substring(3);
                        int n1 = FindNumber(ref list1);
                        if (list1[0] == ',')
                        {
                            list1 = list1.Substring(1);
                            int n2 = FindNumber(ref list1);
                            if (list1[0] == ')')
                            {
                                list1 = list1.Substring(1);
                                total = total + n1 * n2;
                            }
                        }
                    }
                }

                if (firstChar == 'd')
                {
                    if (docalc)
                    {
                        docalc = CheckForDontCalc(ref list1);
                    } else
                    {
                        docalc = CheckForDoCalc(ref list1);
                    }
                }
            }

            Assert.AreEqual(76911921, total);
        }

        [TestCategory("L0")]
        [TestMethod]
        public void Day4A()
        {
            var matrix = FileReader.LoadFileIntoAStringMatrix(@"04A.txt");
            var total = 0;
            for (int x = 0; x < matrix.Count; x++)
            {
                var row = matrix[x];
                for (int y = 0; y < row.Count; y++)
                {
                    var item = matrix[x][y];
                    if (item == 'X')
                    {
                        foreach (var (ix, iy) in directions)
                        {
                            if (CheckInDirection(ix, iy, x + ix, y + iy, matrix, 'M'))
                            {
                                total++;
                            }
                        }
                    }
                }
            }

            Assert.AreEqual(2390, total);
        }

        [TestCategory("L0")]
        [TestMethod]
        public void Day4B()
        {
            var matrix = FileReader.LoadFileIntoAStringMatrix(@"04A.txt");
            var total = 0;
            for (int x = 1; x < matrix.Count - 1; x++)
            {
                var row = matrix[x];
                for (int y = 1; y < row.Count - 1; y++)
                {
                    var item = matrix[x][y];
                    if (item == 'A')
                    {
                        List<char> xmas = new List<char>();
                        foreach (var (ix, iy) in Xdirections)
                        {
                            xmas.Add(matrix[x + ix][y + iy]);
                        }
                        if (succesXmas.Contains(new string(xmas.ToArray())))
                        {
                            total++;
                        }
                    }
                }
            }

            Assert.AreEqual(1809, total);
        }

        [TestCategory("L0")]
        [TestMethod]
        public void Day5A()
        {
            List<List<int>> data = FileReader.LoadFileIntoAMatrix(@"05BTest.txt", ',');
            var (list1, list2) = FileReader.LoadFileIntoToLists(@"05BOrder.txt", false, '|');
            var total = 0;
            foreach (var row in data)
            {
                bool valid = true;
                for (int y = 0; y < row.Count - 1; y++)
                {
                    var item = row[y];
                    List<int> validBeforeItems = GetValidBeforeItems(list1, list2, item);
                    List<int> validAfterItems = GetValidAfterItems(list1, list2, item);

                    for (int i = 0; i < y; i++)
                    {
                        if (!validBeforeItems.Contains(row[i]))
                        {
                            valid = false;
                            break;
                        }
                    }

                    for (int i = y + 1; i < row.Count; i++)
                    {
                        if (!validAfterItems.Contains(row[i]))
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (!valid)
                    {
                        break;
                    }
                }

                if (valid)
                {
                    total += row[row.Count / 2];
                }
            }

            Assert.AreEqual(5651, total);
        }

        [TestCategory("L0")]
        [TestMethod]
        public void Day5B()
        {
            List<List<int>> data = FileReader.LoadFileIntoAMatrix(@"05BTest.txt", ',');
            var (list1, list2) = FileReader.LoadFileIntoToLists(@"05BOrder.txt", false, '|');
            var total = 0;
            foreach (var row in data)
            {
                bool valid = true;
                for (int y = 0; y < row.Count - 1; y++)
                {
                    var item = row[y];
                    List<int> validBeforeItems = GetValidBeforeItems(list1, list2, item);
                    List<int> validAfterItems = GetValidAfterItems(list1, list2, item);

                    for (int i = 0; i < y; i++)
                    {
                        if (!validBeforeItems.Contains(row[i]))
                        {
                            valid = false;
                            break;
                        }
                    }

                    for (int i = y + 1; i < row.Count; i++)
                    {
                        if (!validAfterItems.Contains(row[i]))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (!valid)
                    {
                        break;
                    }
                }

                if (!valid)
                {
                    List<int> sortrow = GetRowSorted(row, list1, list2);
                    total += sortrow[sortrow.Count / 2];
                }
            }

            Assert.AreEqual(4743, total);
        }

        [TestCategory("L0")]
        [TestMethod]
        public void Day6A()
        {
            var data = FileReader.LoadFileIntoAStringMatrix("06A.txt");
            (int, int, string) position = FindCursorInMatrix(data);
            // x pos , y pos, direction
            var total = 0;
            //string checkMatrix = "";
            while (position.Item1 >= 0 && position.Item1 < data[0].Count && position.Item2 >= 0 && position.Item2 < data.Count)
            {
                data[position.Item2][position.Item1] = 'X';
                //checkMatrix = "";
                //foreach (var row in data) {
                //    checkMatrix += new string(row.ToArray());
                //    checkMatrix += "\n";
                //}
                if (position.Item3 == "N")
                {
                    position.Item2--;
                    if (position.Item2 - 1 >= 0 && data[position.Item2 - 1][position.Item1] == '#')
                    {
                        position.Item3 = "E";
                    }
                    continue;
                }
                if (position.Item3 == "S")
                {
                    position.Item2++;
                    if (position.Item2 + 1 < data.Count && data[position.Item2 + 1][position.Item1] == '#')
                    {
                        position.Item3 = "W";
                    }
                    continue;
                }
                if (position.Item3 == "E")
                {
                    position.Item1++;
                    if (position.Item1 + 1 < data[0].Count && data[position.Item2][position.Item1 + 1] == '#')
                    {
                        position.Item3 = "S";
                    }
                    continue;
                }
                if (position.Item3 == "W")
                {
                    position.Item1--;
                    if (position.Item1 - 1 >= 0 && data[position.Item2][position.Item1 - 1] == '#')
                    {
                        position.Item3 = "N";
                    }
                }


            }

            foreach (var row in data)
            {
                foreach (var item in row)
                {
                    if (item == 'X')
                    {
                        total++;
                    }
                }
            }

            Assert.AreEqual(4988, total);
        }



        [TestCategory("L0")]
        [TestMethod]
        public void Day6B()
        {
            var data = FileReader.LoadFileIntoAStringMatrix(@"06A.txt");
            (int, int, string) position = FindCursorInMatrix(data);
            // x pos , y pos, direction
            var total = 0;
            for (int x = 0; x < data[0].Count; x++)
            {
                for (int y = 0; y < data.Count; y++)
                {
                    var test = data.Select(innerList => new List<char>(innerList)).ToList();
                    test[y][x] = '#';
                    bool endless = RunInGrid(test, position);
                    if (endless)
                    {
                        total++;
                    }
                }
            }
            Assert.AreEqual(1697, total);
        }

        [TestMethod]
        public void Day7A()
        {
            List<(long, List<int>)> data = FileReader.LoadFileResultInput(@"07A.txt");
            long total = 0;
            foreach (var row in data)
            {
                var numbers = row.Item2;
                var result = row.Item1;
                List<List<bool>> permutations = GetPermutations(numbers.Count - 1);
                foreach (var permutation in permutations)
                {
                    long sum = numbers[0];
                    for (int i = 0; i < permutation.Count; i++)
                    {
                        if (permutation[i])
                        {
                            sum += numbers[i + 1];
                        } else
                        {
                            sum = sum * numbers[i + 1];
                        }
                    }
                    if (sum == result)
                    {
                        total += sum;

                        //File.AppendAllText(@"07AOut.txt", sum + "\n");
                        break;
                    }
                }
            }
            Assert.AreEqual(12553187650171, total);
        }

        [TestMethod]
        public void Day7B()
        {
            List<(long, List<int>)> data = FileReader.LoadFileResultInput(@"07A.txt");
            long total = 0;
            File.Delete(@"C:\temp\07BOut.txt");
            foreach (var row in data)
            {
                var numbers = row.Item2;
                var result = row.Item1;
                List<List<int>> permutations = GetThreePermutations(numbers.Count - 1);
                foreach (var permutation in permutations)
                {
                    long sum = numbers[0];
                    for (int i = 0; i < permutation.Count; i++)
                    {

                        if (permutation[i] == 0)
                        {
                            sum += numbers[i + 1];
                        } else
                        {
                            if (permutation[i] == 1)
                            {
                                sum = sum * numbers[i + 1];
                            } else
                            {
                                sum = sum * (long)Math.Pow(10, numbers[i + 1].ToString().Length) + numbers[i + 1];
                            }
                        }

                    }
                    if (sum == result)
                    {
                        total += sum;
                        // File.AppendAllText(@"C:\temp\07BOut.txt", sum + " : " + string.Join(" ", numbers) +" : " + string.Join(" ", permutation) + "\n");
                        break;
                    }
                }
            }
            Assert.AreEqual(96779702119491, total);
        }

        [TestMethod]
        public void Day8A()
        {
            List<List<char>> map = FileReader.LoadFileIntoAStringMatrix(@"08A.txt");
            List<(char, int, int)> locations = GetLocationsFromMap(map);
            var total = 0;
            List<char> keys = locations.Select(x => x.Item1).Distinct().ToList();
            var goallocations = new List<(int, int)>();
            foreach (var key in keys)
            {
                var keylocations = locations.Where(x => x.Item1 == key).ToList();
                for (int i = 0; i < keylocations.Count; i++)
                {
                    var location = keylocations[i];
                    for (int j = i + 1; j < keylocations.Count; j++)
                    {
                        var location2 = keylocations[j];
                        (int, int) anntenna1 = (2 * location.Item3 - location2.Item3, 2 * location.Item2 - location2.Item2); // wegen a+(a-b) = 2a-b
                        (int, int) anntenna2 = (2 * location2.Item3 - location.Item3, 2 * location2.Item2 - location.Item2);
                        if (anntenna1.Item1 >= 0 && anntenna1.Item2 >= 0 && anntenna1.Item1 < map.Count && anntenna1.Item2 < map.Count)
                        {
                            if (!goallocations.Contains(anntenna1))
                            {
                                goallocations.Add(anntenna1);
                            }
                        }
                        if (anntenna2.Item1 >= 0 && anntenna2.Item2 >= 0 && anntenna2.Item1 < map.Count && anntenna2.Item2 < map.Count)
                        {
                            if (!goallocations.Contains(anntenna2))
                            {
                                goallocations.Add(anntenna2);
                            }
                        }
                    }
                }

            }
            total = goallocations.Count;
            Assert.AreEqual(313, total);
        }

        [TestMethod]
        public void Day8B()
        {
            List<List<char>> map = FileReader.LoadFileIntoAStringMatrix(@"08A.txt");
            List<(char, int, int)> locations = GetLocationsFromMap(map);
            var total = 0;
            List<char> keys = locations.Select(x => x.Item1).Distinct().ToList();
            var goallocations = new List<(int, int)>();
            foreach (var key in keys)
            {
                var keylocations = locations.Where(x => x.Item1 == key).ToList();
                for (int i = 0; i < keylocations.Count; i++)
                {
                    var location = keylocations[i];
                    if (!goallocations.Contains((location.Item2, location.Item3)))
                    {
                        goallocations.Add((location.Item2, location.Item3));
                    }
                    for (int j = i + 1; j < keylocations.Count; j++)
                    {
                        var location2 = keylocations[j];
                        if (!goallocations.Contains((location2.Item2, location2.Item3)))
                        {
                            goallocations.Add((location2.Item2, location2.Item3));
                        }
                        (int, int) distance = (location.Item2 - location2.Item2, location.Item3 - location2.Item3);
                        bool inbound = true;
                        var counter = 1;
                        while (inbound)
                        {
                            var newlocation = (location.Item2 + (distance.Item1 * counter), location.Item3 + distance.Item2 * counter);
                            if (newlocation.Item1 >= 0 && newlocation.Item2 >= 0 && newlocation.Item1 < map.Count && newlocation.Item2 < map.Count)
                            {
                                if (!goallocations.Contains(newlocation))
                                {
                                    goallocations.Add(newlocation);
                                }
                                counter++;
                            } else
                            {
                                inbound = false;
                            }
                        }

                        inbound = true;
                        counter = -1;
                        while (inbound)
                        {
                            var newlocation = (location.Item2 + (distance.Item1 * counter), location.Item3 + distance.Item2 * counter);
                            if (newlocation.Item1 >= 0 && newlocation.Item2 >= 0 && newlocation.Item1 < map.Count && newlocation.Item2 < map.Count)
                            {
                                if (!goallocations.Contains(newlocation))
                                {
                                    goallocations.Add(newlocation);
                                }
                                counter--;
                            } else
                            {
                                inbound = false;
                            }
                        }

                    }
                }

            }
            total = goallocations.Count;
            Assert.AreEqual(1064, total);
        }

        [TestMethod]
        public void Day9A()
        {
            string input = FileReader.LoadFileIntoOneString(@"09A.txt");
            var idsWithSpaces = GenerateIdList(input);
            int right = idsWithSpaces.Count - 1;
            for (int left = 0; left < idsWithSpaces.Count; left++)
            {
                if (idsWithSpaces[left] != null)
                {
                    continue;
                }

                while (idsWithSpaces[right] == null)
                {
                    right--;
                }
                if (right <= left)
                {
                    break;
                }
                idsWithSpaces[left] = idsWithSpaces[right];
                idsWithSpaces[right] = null;
                right--;
            }
            long checksum = GetChecksum(idsWithSpaces);
            Assert.AreEqual(6349606724455, checksum); //6349606724455
        }

        [TestMethod]
        public void Day9B()
        {
            string input = FileReader.LoadFileIntoOneString(@"09A.txt");
            var idsWithSpaces = GenerateIdList(input);
            (int, int) location = (0, 0);
            int last = idsWithSpaces.Last().Value;
            int maxsize = input.Length;
            for (int id = last; id >= 0; id--)
            {
                int right = idsWithSpaces.Count - 1;
                bool found = false;

                // find id that should be moved to the left
                while (idsWithSpaces[right] == id || !found)
                {

                    if (idsWithSpaces[right] == id && !found)
                    {
                        found = true;
                        location = (right, right);
                    }
                    location.Item1 = right;
                    right--;
                    if (right < 0)
                    {
                        break;
                    }
                }
                int size = location.Item2 - location.Item1; // size of the id (correct size is size + 1)
                if (size >= maxsize)
                {
                    // there is no location for ids with size bigger than maxsize
                    continue;
                }


                (int, int) putlocation = (0, 0);
                // find new location to put the id
                for (int i = 0; i < right; i++)
                {
                    bool locationfound = false;
                    if (idsWithSpaces[i] == null)
                    {
                        locationfound = true;
                        for (int j = 0; j <= size; j++)
                        {
                            if (idsWithSpaces[i + j] != null)
                            {
                                locationfound = false;
                                break;
                            }
                        }
                    }
                    if (locationfound)
                    {
                        putlocation = (i, i + size);
                        break;
                    }
                }

                if (putlocation.Item1 == 0 && putlocation.Item2 == 0)
                {
                    // no new location found go to the next id
                    // todo remember max size
                    maxsize = size;
                    if (maxsize == 0)
                    { break; }
                    continue;
                }
                // move id to the left
                for (int i = location.Item1; i <= location.Item2; i++)
                {
                    idsWithSpaces[putlocation.Item1] = idsWithSpaces[i];
                    idsWithSpaces[i] = null;
                    putlocation.Item1++;
                }

            }
            long checksum = GetChecksum(idsWithSpaces);
            Assert.AreEqual(6376648986651, checksum); //6349606724455
        }

        private long GetChecksum(List<int?> idsWithSpaces)
        {
            long checksum = 0;
            for (int i = 0; i < idsWithSpaces.Count; i++)
            {
                if (idsWithSpaces[i] == null)
                {
                    continue;
                }
                checksum += i * idsWithSpaces[i].Value;
            }
            return checksum;
        }



        private static List<int?> GenerateIdList(string input)
        {
            List<int?> list = new List<int?>();
            bool addId = true;
            int counter = 0;
            int? insertvar;
            foreach (char c in input)
            {
                if (addId)
                {
                    insertvar = counter;
                    counter++;
                } else
                {
                    insertvar = null;
                }
                int x = int.Parse(c.ToString());
                if (x == 0 && addId)
                {
                    counter--;
                }
                for (int i = 0; i < x; i++)
                {
                    list.Add(insertvar);
                }
                addId = !addId;
            }
            return list;
        }

        private List<(char, int, int)> GetLocationsFromMap(List<List<char>> map)
        {
            List<(char, int, int)> locations = new List<(char, int, int)>();
            for (int x = 0; x < map.Count; x++)
            {
                for (int y = 0; y < map[x].Count; y++)
                {
                    if (map[x][y] != '.')
                    {
                        locations.Add((map[x][y], x, y));
                    }
                }
            }

            return locations;
        }

        private List<List<bool>> GetPermutations(int v)
        {
            var permutations = new List<List<bool>>();
            for (int i = 0; i < Math.Pow(2, v); i++)
            {

                string binaryRepresentation = Convert.ToString(i, 2);
                int length = binaryRepresentation.Length;
                for (int j = 0; j < v - length; j++)
                {
                    binaryRepresentation = "0" + binaryRepresentation;
                }

                List<bool> permutation = new List<bool>();
                for (int j = 0; j < binaryRepresentation.Length; j++)
                {
                    permutation.Add(binaryRepresentation[j] == '1');
                }
                permutations.Add(permutation);
            }
            return permutations;

        }

        private List<List<int>> GetThreePermutations(int v)
        {
            var permutations = new List<List<int>>();
            for (int i = 0; i < Math.Pow(3, v); i++)
            {

                string base3Representation = ConvertToBaseThree(i);
                int length = base3Representation.Length;
                for (int j = 0; j < v - length; j++)
                {
                    base3Representation = "0" + base3Representation;
                }

                List<int> permutation = new List<int>();
                for (int j = 0; j < base3Representation.Length; j++)
                {
                    permutation.Add(int.Parse(base3Representation[j].ToString()));
                }
                permutations.Add(permutation);
            }
            return permutations;

        }

        private string ConvertToBaseThree(int i)
        {
            if (i == 0)
                return "0";
            const int baseNumber = 3;
            string base3Representation = "";
            int rest = i;
            while (rest > 0)
            {
                int result = rest % baseNumber;
                base3Representation = result + base3Representation;
                rest = rest / baseNumber;
            }
            return base3Representation;
        }

        private static bool RunInGrid(List<List<char>> data, (int, int, string) position)
        {
            int total = 0;
            int total2 = 0;
            foreach (var row in data)
            {
                foreach (var item in row)
                {
                    if (item == '#')
                    {
                        total++;
                    }
                }
            }

            string checkMatrixbefore = "";
            foreach (var row in data)
            {
                checkMatrixbefore += new string(row.ToArray());
                checkMatrixbefore += "\n";
            }


            string checkMatrix = "";
            int counter = 0;
            while (position.Item1 >= 0 && position.Item1 < data[0].Count && position.Item2 >= 0 && position.Item2 < data.Count)
            {
                counter++;
                //data[position.Item2][position.Item1] = 'X';
                //checkMatrix = "";
                //foreach (var row in data) {
                //    checkMatrix += new string(row.ToArray());
                //    checkMatrix += "\n";
                //}
                if (position.Item3 == "N")
                {
                    position.Item2--;
                    if (position.Item2 - 1 >= 0 && data[position.Item2 - 1][position.Item1] == '#')
                    {
                        position.Item3 = "E";
                    }
                    continue;
                }
                if (position.Item3 == "S")
                {
                    position.Item2++;
                    if (position.Item2 + 1 < data.Count && data[position.Item2 + 1][position.Item1] == '#')
                    {
                        position.Item3 = "W";
                    }
                    continue;
                }
                if (position.Item3 == "E")
                {
                    position.Item1++;
                    if (position.Item1 + 1 < data[0].Count && data[position.Item2][position.Item1 + 1] == '#')
                    {
                        position.Item3 = "S";
                    }
                    continue;
                }
                if (position.Item3 == "W")
                {
                    position.Item1--;
                    if (position.Item1 - 1 >= 0 && data[position.Item2][position.Item1 - 1] == '#')
                    {
                        position.Item3 = "N";
                    }
                }
                if (data.Count * data[0].Count < counter)
                {
                    foreach (var row in data)
                    {
                        foreach (var item in row)
                        {
                            if (item == '#')
                            {
                                total2++;
                            }
                        }
                    }

                    checkMatrix = "";
                    foreach (var row in data)
                    {
                        checkMatrix += new string(row.ToArray());
                        checkMatrix += "\n";
                    }

                    for (int i = 0; i < checkMatrix.Length; i++)
                    {
                        if (checkMatrix[i] != checkMatrixbefore[i])
                        {
                            break;
                        }
                    }

                    Assert.AreEqual(total, total2);
                    return true;
                }
            }
            foreach (var row in data)
            {
                foreach (var item in row)
                {
                    if (item == '#')
                    {
                        total2++;
                    }
                }
            }

            Assert.AreEqual(total, total2);
            return false;
        }

        private (int, int, string) FindCursorInMatrix(List<List<char>> data)
        {
            for (int x = 0; x < data[0].Count; x++)
            {
                for (int y = 0; y < data.Count; y++)
                {
                    if (data[y][x] == '^')
                    {
                        return (x, y, "N");
                    }
                    if (data[y][x] == 'v')
                    {
                        return (x, y, "S");
                    }
                    if (data[y][x] == '>')
                    {
                        return (x, y, "E");
                    }
                    if (data[y][x] == '<')
                    {
                        return (x, y, "W");
                    }
                }
            }
            Assert.Fail("No cursor found");
            return (0, 0, "");
        }

        private List<int> GetRowSorted(List<int> row, List<int> list1, List<int> list2)
        {
            bool sorted = false;
            int counter = 0;
            while (!sorted)
            {
                counter++;
                sorted = true;
                for (int i = 0; i < row.Count - 1; i++)
                {
                    int a = row[i];
                    int b = row[i + 1];
                    List<int> validBeforeItems = GetValidBeforeItems(list1, list2, b);
                    List<int> validAfterItems = GetValidAfterItems(list1, list2, a);
                    if (!(validAfterItems.Contains(b) && validBeforeItems.Contains(a)))
                    {
                        sorted = false;
                        // swap
                        var temp = row[i];
                        row[i] = row[i + 1];
                        row[i + 1] = temp;
                    }
                }

                if (counter > row.Count * row.Count)
                {
                    Assert.Fail("To many iterations");
                }
            }
            return row;
        }

        private static List<int> GetValidAfterItems(List<int> list1, List<int> list2, int item)
        {
            List<int> validAfterItems = new List<int>();
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] == item)
                {
                    validAfterItems.Add(list2[i]);
                }
            }

            return validAfterItems;
        }

        private static List<int> GetValidBeforeItems(List<int> list1, List<int> list2, int item)
        {

            List<int> validBeforeItems = new List<int>();
            for (int i = 0; i < list2.Count; i++)
            {
                if (list2[i] == item)
                {
                    validBeforeItems.Add(list1[i]);
                }
            }

            return validBeforeItems;
        }

        private List<(int, int)> LoadFileIntoTuples(string v)
        {
            var lines = System.IO.File.ReadAllLines(v);
            List<(int, int)> numbers = new List<(int, int)>();
            foreach (var line in lines)
            {
                // Split the string by whitespace
                string[] parts = line.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                // Parse the parts into integers
                var a = int.Parse(parts[0]);
                var b = int.Parse(parts[1]);
                numbers.Add((a, b));
            }
            return numbers;
        }

        private bool CheckInDirection(int ix, int iy, int posx, int posy, List<List<char>> matrix, char v)
        {
            if (posx < 0 || posx >= matrix.Count)
                return false;
            if (posy < 0 || posy >= matrix[posx].Count)
                return false;
            char before = matrix[posx - ix][posy - iy];
            if (matrix[posx][posy] == 'S' && before == 'A')
            {
                return true;
            }
            if (matrix[posx][posy] == 'S' && before != 'A')
            {
                return false;
            }

            if (matrix[posx][posy] == v)
            {
                return CheckInDirection(ix, iy, posx + ix, posy + iy, matrix, keyValuePairs[v]);
            }
            return false;
        }

        private readonly List<(int, int)> directions = new List<(int, int)> {
            (-1,-1),
            (-1,0),
            (-1,1),
            (0,-1),
            (0,1),
            (1,-1),
            (1,0),
            (1,1),
        };

        private readonly List<(int, int)> Xdirections = new List<(int, int)> {
            (-1,-1),
            (-1,1),
            (1,-1),
            (1,1),
        };

        List<string> succesXmas = new List<string> {
            "MSMS",
            "SMSM",
            "SSMM",
            "MMSS",
        };

        private readonly Dictionary<char, char> keyValuePairs = new Dictionary<char, char> {
            {'M','A' },
            {'A','S' }
        };

        private static bool CheckForDoCalc(ref string list1)
        {
            if (list1.Substring(0, 3) == "o()")
            {
                list1 = list1.Substring(3);
                return true;
            }
            return false;
        }

        private bool CheckForDontCalc(ref string list1)
        {
            if (list1.Substring(0, 6) == "on't()")
            {
                list1 = list1.Substring(6);
                return false;
            }
            return true;
        }

        private static int FindNumber(ref string list1)
        {
            int x = 0;
            int number = 0;
            while (int.TryParse(list1[0].ToString(), out x))
            {
                number = number * 10 + x;
                list1 = list1.Substring(1);
                if (number > 99)
                {
                    break;
                }
            }

            return number;
        }

        private bool IsRowOk(List<int> row)
        {
            bool first = true;
            bool up = RowIsUp(row);
            bool ok = false;
            int before = 0;
            int after;
            for (int i = 0; i < row.Count - 1; i++)
            {
                var item = row[i];

                if (first)
                {
                    before = item;
                    first = false;
                    continue;
                }
                if (i < row.Count - 1)
                {
                    after = row[i + 1];
                } else
                {
                    after = up ? item++ : item--; // we added a valid item
                }

                ok = false;

                if (up)
                {
                    if ((item > before && item - before < 4) && (item < after && after - item < 4))
                    {
                        ok = true;
                        before = item;
                        continue;
                    }
                } else
                {
                    if ((item < before && before - item < 4) && (item > after && item - after < 4))
                    {
                        ok = true;
                        before = item;
                        continue;
                    }
                }
                break;
            }

            return ok;
        }

        private bool RowIsUp(List<int> row)
        {
            int countup = 0;
            int countdown = 0;
            for (int i = 0; i < row.Count - 1; i++)
            {
                if (row[i] < row[i + 1])
                {
                    countup++;
                    continue;
                }

                if (row[i] > row[i + 1])
                {
                    countdown++;
                }
            }

            return countup > countdown;
        }
    }
}


