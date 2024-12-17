using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ComponentModel;

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
                System.IO.File.AppendAllText(@"07AOut.txt", item + ": " + DateTime.Now.ToString() + "\n");
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

        [TestMethod]
        public void Day13a()
        {
            int buttonA = 3;
            int buttonB = 1;
            List<((int, int), (int, int), (int, int))> maschines = FileReader.LoadButtonInformation("13A.txt");
            int total = 0;
            foreach (var maschine in maschines)
            {
                List<(int, int)> goals = FindAllWaysForThePrice(maschine.Item1, maschine.Item2, maschine.Item3);


                int minimum = int.MaxValue;
                foreach (var goal in goals)
                {
                    if (goal.Item1 * buttonA + goal.Item2 * buttonB < minimum)
                    {
                        minimum = goal.Item1 * buttonA + goal.Item2 * buttonB;
                    }
                }
                if (minimum < int.MaxValue)
                {
                    total += minimum;
                }
            }
            Assert.AreEqual(33921, total);
        }

        [TestMethod]
        public void Day13B()
        {
            int buttonA = 3;
            int buttonB = 1;
            List<((int, int), (int, int), (int, int))> maschines = FileReader.LoadButtonInformation("13A.txt");
            long total = 0;
            foreach (var maschine in maschines)
            {
                long scale = 10000000000000;
                (long, long) movedprice = (maschine.Item3.Item1 + scale, maschine.Item3.Item2 + scale);
                List<(long, long)> goals = FindAllWaysForThePriceWithMAth(maschine.Item1, maschine.Item2, movedprice);
                long minimum = long.MaxValue;
                foreach (var goal in goals)
                {
                    if (goal.Item1 * buttonA + goal.Item2 * buttonB < minimum)
                    {
                        minimum = goal.Item1 * buttonA + goal.Item2 * buttonB;
                    }
                }
                if (minimum < long.MaxValue)
                {
                    total += minimum;
                }
            }
            Assert.AreEqual(82261957837868, total);
        }

        [TestMethod]
        public void Day14A()
        {
            List<((int, int), (int, int))> matrix = FileReader.LoadRobotsFromFile("14A.txt");
            int seconds = 100;
            (int, int) mapsize = (101, 103);
            List<(int, int)> allEndPositions = new List<(int, int)>();
            File.Delete(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\14Out.txt");
            for (int s = 100; s <= seconds; s++)
            {
                allEndPositions = new List<(int, int)>();
                foreach (var robot in matrix)
                {
                    (int, int) position = robot.Item1;
                    (int, int) velocity = robot.Item2;
                    int xEnd = (position.Item1 + velocity.Item1 * s) % mapsize.Item1;
                    int yEnd = (position.Item2 + velocity.Item2 * s) % mapsize.Item2;
                    if (xEnd < 0)
                    {
                        xEnd = mapsize.Item1 + xEnd;
                    }

                    if (yEnd < 0)
                    {
                        yEnd = mapsize.Item2 + yEnd;
                    }
                    allEndPositions.Add((xEnd, yEnd));
                }

                // when running for 10000 seconds, this will find the easter egg at second  8270 to solve part 2
                //    bool found = false;
                //    foreach (var position in allEndPositions)
                //    {
                //        int counter= 0;
                //        foreach (var pos in tree)
                //        {
                //            if (allEndPositions.Contains((position.Item1 + pos.Item1, position.Item2 + pos.Item2)))
                //            {
                //                counter++;
                //            }
                //        }
                //        if (counter >= 20)
                //        {
                //            found = true;
                //        }

                //    }
                //    if (found)
                //    {
                //        File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\14Out.txt", $"\n {s} \n");
                //        PrintMap(allEndPositions, mapsize);
                //    }
            }
            int Q1 = 0;
            int Q2 = 0;
            int Q3 = 0;
            int Q4 = 0;
            int nocountX = (mapsize.Item1 - 1) / 2;
            int nocountY = (mapsize.Item2 - 1) / 2;
            foreach (var position in allEndPositions)
            {
                if (position.Item1 < nocountX && position.Item2 < nocountY)
                {
                    Q1++;
                }
                if (position.Item1 > nocountX && position.Item2 < nocountY)
                {
                    Q2++;
                }
                if (position.Item1 < nocountX && position.Item2 > nocountY)
                {
                    Q3++;
                }
                if (position.Item1 > nocountX && position.Item2 > nocountY)
                {
                    Q4++;
                }
            }
            int total = Q1 * Q2 * Q3 * Q4;
            Assert.AreEqual(230436441, total);
        }

        [TestMethod]
        public void Day15A()
        {
            File.Delete(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt");
            var data = FileReader.LoadFileIntoAStringMatrixWithMoves("15A.txt");
            var matrix = data.Item1;
            var moves = data.Item2;
            (int, int) robot = FindCharInMap(matrix, '@');
            for (int i = 0; i < moves.Length; i++)
            {
                var direction = moves[i];
                robot = MoveRobot(matrix, robot, direction);

            }
            PrintMap(matrix);
            int total = 0;
            for (int i = 0; matrix.Count > i; i++)
            {
                for (int j = 0; matrix[0].Count > j; j++)
                {
                    if (matrix[i][j] == 'O')
                    {
                        total = total + 100 * i + j;
                    }
                }
            }
            Assert.AreEqual(1509074, total);
        }

        [TestMethod]
        public void Day15B()
        {
            File.Delete(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt");
            var data = FileReader.LoadFileIntoAStringMatrixWithMoves("15A.txt");
            var matrix = ModifyMap(data.Item1);
            var moves = data.Item2;
            PrintMap(matrix);
            (int, int) robot = FindCharInMap(matrix, '@');
            for (int i = 0; i < moves.Length; i++)
            {
                var direction = moves[i];
                robot = MoveRobotLarge(matrix, robot, direction);


            }
            PrintMap(matrix);
            int total = 0;
            for (int i = 0; matrix.Count > i; i++)
            {
                for (int j = 0; matrix[0].Count > j; j++)
                {
                    if (matrix[i][j] == '[')
                    {
                        total = total + 100 * i + j;
                    }
                }
            }
            Assert.AreEqual(1521453, total);
        }

        public int TotalCost = 0;
        [TestMethod]
        public void Day16A()
        {
            File.Delete(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt");
            var matrix = FileReader.LoadFileIntoAStringMatrix("16A.txt");
            var start = FindCharInMap(matrix, 'S');
            var end = FindCharInMap(matrix, 'E');
            List<(int, int)> visitedPositions = new List<(int, int)>();
            int cost = 1001;
            var direction = (0, 1);
            TotalCost = 121452;
            RunInTheMaze(matrix, start, direction, end, visitedPositions, cost);
            Assert.AreEqual(105508, TotalCost);
        }

        [TestMethod]
        public void Day16B()
        {
            File.Delete(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt");
            var matrix = FileReader.LoadFileIntoAStringMatrix("16Test3.txt");
            var start = FindCharInMap(matrix, 'S');
            var end = FindCharInMap(matrix, 'E');
            List<(int, int)> visitedPositions = new List<(int, int)>();
            int cost = 1;
            var direction = (0, 1);
            TotalCost = 121452;
            RunInTheMaze(matrix, start, direction, end, visitedPositions, cost);
            File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", $"Map Costs At End{cost}\n");
            Assert.AreEqual(105508, TotalCost);
        }

        private void RunInTheMaze(List<List<char>> matrix, (int, int) myPosition, (int, int) direction, (int, int) end, List<(int, int)> visitedPositions, int cornerCosts)
        {
            List<((int, int), int, (int, int))> notcheckeddirections = new List<((int, int), int, (int, int))>() { (myPosition, 0, direction) };
            Dictionary<(int, int), int> visitedPositionsCosts = new Dictionary<(int, int), int>();
            while (notcheckeddirections.Count > 0)
            {
                var check = notcheckeddirections[0];
                var position = check.Item1;
                var checkdirection = check.Item3;
                var checkcost = check.Item2;

                notcheckeddirections.RemoveAt(0);
                if (position == (1, 15))
                {
                    checkcost = check.Item2;
                }

                if (visitedPositionsCosts.TryGetValue(position, out int poscost))
                {
                    bool better = false;
                    if (position == end && poscost > checkcost)
                    { visitedPositionsCosts[position] = checkcost;
                        better = true;
                    }
                        
                    if (CheckForSpace(position, checkdirection, matrix) && poscost > checkcost)
                    {
                        visitedPositionsCosts[position] = checkcost;
                        better = true;
                    }
                    if (!better)
                    {
                        List<(int, int)> newdir = OtherDirections(checkdirection);
                        if (CheckForSpace(position, newdir[0], matrix) && poscost > checkcost + 1000)
                        {
                            visitedPositionsCosts[position] = checkcost + 1000;
                            better = true;
                        }
                        if (CheckForSpace(position, newdir[1], matrix) && poscost > checkcost + 1000)
                        {
                            visitedPositionsCosts[position] = checkcost + 1000;
                            better = true;
                        }
                    }
                    if (!better)
                    { continue; }
                }

                if (!visitedPositionsCosts.ContainsKey(position))
                {
                    visitedPositionsCosts.Add(position, checkcost);
                    // File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", $"Position {position}: cost {checkcost}\n");
                }

                if (position == end)
                {
                    if (checkcost < TotalCost)
                    {

                        TotalCost = checkcost;
                        File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", $"New TotalCosts{TotalCost}\n");

                    }
                    //File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", $"Map Costs At End{cost}\n");
                    //PrintMapWithPath(matrix, visitedPositions);
                    continue;
                }



                if (CheckForSpace(position, checkdirection, matrix))
                {
                    notcheckeddirections.Add(((position.Item1 + checkdirection.Item1, position.Item2 + checkdirection.Item2), checkcost + 1, checkdirection));
                }

                List<(int, int)> newdirections = OtherDirections(checkdirection);
                bool turned = false;
                if (CheckForSpace(position, newdirections[0], matrix))
                {
                    turned = true;
                    notcheckeddirections.Add(((position.Item1 + newdirections[0].Item1, position.Item2 + newdirections[0].Item2), checkcost + cornerCosts, newdirections[0]));
                }
                if (CheckForSpace(position, newdirections[1], matrix))
                {
                    turned = true;
                    notcheckeddirections.Add(((position.Item1 + newdirections[1].Item1, position.Item2 + newdirections[1].Item2), checkcost + cornerCosts, newdirections[1]));
                }
                if (turned)
                {
                    // visitedPositionsCosts[position] = checkcost+1000;
                }
                //   PrintMapWithPath(matrix, visitedPositionsCosts.Keys.ToList());

            }

            List<((int, int), int)> checkerList = new List<((int, int), int)>() { (end, TotalCost) };
            HashSet<(int, int)> thePATH = new HashSet<(int, int)>();
            while (checkerList.Count > 0)
            {

                var checker = checkerList[0].Item1;
                var oldcheckercosts = checkerList[0].Item2;
                var checkercosts = visitedPositionsCosts[checker];
                checkerList.RemoveAt(0);
                thePATH.Add(checker);
                if (checker == myPosition)
                {
                    continue;
                }


                List<((int, int), int)> directionsToGo = new List<((int, int), int)>();
                foreach (var d in directions)
                {
                    if (visitedPositionsCosts.ContainsKey((checker.Item1 + d.Item1, checker.Item2 + d.Item2)))
                    {
                        visitedPositionsCosts.TryGetValue((checker.Item1 + d.Item1, checker.Item2 + d.Item2), out int newcost);
                        if ((newcost % 1000) + 1 == checkercosts % 1000 && (newcost < oldcheckercosts || oldcheckercosts - newcost ==-998))
                            directionsToGo.Add((d, newcost));
                    }
                }
                if (directionsToGo.Count > 1)
                {
                    File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", $"The PATH divides into {directionsToGo.Count} at {checker} places\n");
                }

                if (directionsToGo.Count == 0)
                {
                    File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", $"The PATH is a dead end {directionsToGo.Count} at {checker} places\n");

                    continue;
                }

                var mincost = directionsToGo.OrderBy(d => d.Item2).ToList()[0];
                int oldCostsFromStepBefore = visitedPositionsCosts[checker];
                foreach (var d in directionsToGo)
                {
                    if (directionsToGo.Count==1|| (d.Item2 % 1000 == mincost.Item2 % 1000  && d.Item2 < oldcheckercosts) )
                    {
                        checkerList.Add(((checker.Item1 + d.Item1.Item1, checker.Item2 + d.Item1.Item2), oldCostsFromStepBefore));
                    }
                }

            }
            File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", $"The PATH contains {thePATH.Count} places\n");
            PrintMapWithPath(matrix, thePATH.ToList());
            CreateCostmap(matrix, visitedPositionsCosts);

        }

        private static List<(int, int)> OtherDirections((int, int) checkdirection)
        {
            List<(int, int)> newdirections;
            if (checkdirection.Item1 == 0)
            {
                newdirections = new List<(int, int)>() { (-1, 0), (1, 0) };
            } else
            {
                newdirections = new List<(int, int)>() { (0, 1), (0, -1) };
            }

            return newdirections;
        }

        private void CreateCostmap(List<List<char>> matrix, Dictionary<(int, int), int> visitedPositionsCosts)
        {
            List<List<char>> map = new List<List<char>>();
            List<char> visited = new List<char>() { ' ' };
            for (int i = 0; i < matrix[0].Count; i++)
            {
                char c = GetIndexforMatrix(i);
                visited.Add(c);
                visited.Add(c);
                visited.Add(c);
                visited.Add(c);
                visited.Add(c);
                visited.Add(c);
                visited.Add(c);
            }
            map.Add(visited);
            for (int i = 0; i < matrix.Count; i++)
            {
                char c = GetIndexforMatrix(i);

                List<char> row = new List<char>();
                row.Add(c);
                for (int j = 0; j < matrix[0].Count; j++)
                {
                    if (visitedPositionsCosts.TryGetValue((i, j), out int cost))
                    {
                        string coststring = cost.ToString("D6");
                        row.Add(' ');
                        row.AddRange(coststring.ToCharArray());
                    } else
                    {
                        row.Add(matrix[i][j]);
                        row.Add(matrix[i][j]);
                        row.Add(matrix[i][j]);
                        row.Add(matrix[i][j]);
                        row.Add(matrix[i][j]);
                        row.Add(matrix[i][j]);
                        row.Add(matrix[i][j]);
                    }
                }
                map.Add(row);
            }
            PrintMap(map);
        }

        private void PrintMapWithPath(List<List<char>> matrix, List<(int, int)> visitedPositions)
        {
            List<List<char>> map = new List<List<char>>();
            List<char> visited = new List<char>() { ' ' };
            for (int i = 0; i < matrix[0].Count; i++)
            {
                char c = GetIndexforMatrix(i);
                visited.Add(c);
            }
            map.Add(visited);
            for (int i = 0; i < matrix.Count; i++)
            {
                char c = GetIndexforMatrix(i);

                List<char> row = new List<char>();
                row.Add(c);
                for (int j = 0; j < matrix[0].Count; j++)
                {
                    if (visitedPositions.Contains((i, j)))
                    {
                        row.Add('*');
                    } else
                    {
                        row.Add(matrix[i][j]);
                    }
                }
                map.Add(row);
            }
            PrintMap(map);
        }

        private char GetIndexforMatrix(int i)
        {
            if (i % 10 == 0)
            {
                return '0';

            }
            if (i % 10 == 5)
            {
                return '5';

            }
            return ' ';
        }

        private bool CheckForSpace((int, int) myPosition, (int, int) direction, List<List<char>> matrix)
        {
            if (matrix[myPosition.Item1 + direction.Item1][myPosition.Item2 + direction.Item2] == '#')
            {
                return false;
            }
            return true;
        }

        private (int, int) MoveRobotLarge(List<List<char>> matrix, (int, int) robot, char direction)
        {
            (int, int) movedirection = FindMoveDirection(direction);

            bool canmove = false;
            List<(int, int)> checkpos = new List<(int, int)>() { (robot.Item1 + movedirection.Item1, robot.Item2 + movedirection.Item2) };
            List<(int, int)> checkedpos = new List<(int, int)>();
            while (checkpos.All(c => matrix[c.Item1][c.Item2] != '#'))
            {
                if (checkpos.All(c => matrix[c.Item1][c.Item2] == '.'))
                {
                    canmove = true;
                    break;
                }
                List<(int, int)> newcheckpos = new List<(int, int)>();

                foreach (var pos in checkpos)
                {
                    if (movedirection.Item1 == 0)
                    {
                        if (matrix[pos.Item1][pos.Item2] == ']')
                        {
                            AddNewPosition((movedirection), newcheckpos, pos, checkedpos);
                        }
                        if (matrix[pos.Item1][pos.Item2] == '[')
                        {
                            AddNewPosition(movedirection, newcheckpos, pos, checkedpos);
                        }
                        continue;
                    }
                    if (matrix[pos.Item1][pos.Item2] == ']')
                    {
                        AddNewPosition(movedirection, newcheckpos, pos, checkedpos);
                        AddNewPosition((0, -1), newcheckpos, pos, checkedpos);
                    }
                    if (matrix[pos.Item1][pos.Item2] == '[')
                    {
                        AddNewPosition(movedirection, newcheckpos, pos, checkedpos);
                        AddNewPosition((0, +1), newcheckpos, pos, checkedpos);
                    }
                }
                checkedpos.AddRange(checkpos);
                checkpos = newcheckpos;
            }

            List<(int, int)> moveUp = new List<(int, int)>() { robot };
            HashSet<((int, int), (int, int))> Allmoves = new HashSet<((int, int), (int, int))>();
            if (canmove)
            {
                while (moveUp.Count > 0)
                {
                    if (matrix[moveUp[0].Item1 + movedirection.Item1][moveUp[0].Item2 + movedirection.Item2] == '[')
                    {
                        moveUp.Add((moveUp[0].Item1 + movedirection.Item1, moveUp[0].Item2 + movedirection.Item2));
                        if (movedirection.Item1 != 0)
                        {
                            moveUp.Add((moveUp[0].Item1 + movedirection.Item1, moveUp[0].Item2 + movedirection.Item2 + 1));
                        }
                    }
                    if (matrix[moveUp[0].Item1 + movedirection.Item1][moveUp[0].Item2 + movedirection.Item2] == ']')
                    {
                        moveUp.Add((moveUp[0].Item1 + movedirection.Item1, moveUp[0].Item2 + movedirection.Item2));
                        if (movedirection.Item1 != 0)
                        {
                            moveUp.Add((moveUp[0].Item1 + movedirection.Item1, moveUp[0].Item2 + movedirection.Item2 - 1));
                        }
                    }
                    Allmoves.Add((moveUp[0], movedirection));
                    moveUp.RemoveAt(0);
                }
                while (Allmoves.Count > 0)
                {
                    var move = Allmoves.Last();
                    var position = move.Item1;
                    var godirection = move.Item2;
                    var temp = matrix[position.Item1 + godirection.Item1][position.Item2 + godirection.Item2];
                    matrix[position.Item1 + godirection.Item1][position.Item2 + godirection.Item2] = matrix[position.Item1][position.Item2];
                    matrix[position.Item1][position.Item2] = temp;
                    Allmoves.Remove(move);
                }
                robot = (robot.Item1 + movedirection.Item1, robot.Item2 + movedirection.Item2);
            }
            return robot;

        }

        private static void AddNewPosition((int, int) movedirection, List<(int, int)> newcheckpos, (int, int) pos, List<(int, int)> checkedPos)
        {
            if (checkedPos.Contains((pos.Item1 + movedirection.Item1, pos.Item2 + movedirection.Item2)))
            {
                return;
            }
            if (!newcheckpos.Contains((pos.Item1 + movedirection.Item1, pos.Item2 + movedirection.Item2)))
            {
                newcheckpos.Add((pos.Item1 + movedirection.Item1, pos.Item2 + movedirection.Item2));
            }
        }

        private List<List<char>> ModifyMap(List<List<char>> item1)
        {
            var newMap = new List<List<char>>();
            for (int i = 0; item1.Count > i; i++)
            {
                var row = new List<char>();
                for (int j = 0; item1[0].Count > j; j++)
                {
                    switch (item1[i][j])
                    {

                        case '.':
                        row.Add('.');
                        row.Add('.');
                        break;
                        case '#':
                        row.Add('#');
                        row.Add('#');
                        break;
                        case 'O':
                        row.Add('[');
                        row.Add(']');
                        break;
                        case '@':
                        row.Add('@');
                        row.Add('.');
                        break;
                    }
                }
                newMap.Add(row);
            }
            return newMap;
        }

        private (int, int) FindCharInMap(List<List<char>> matrix, char c)
        {
            for (int i = 0; matrix.Count > i; i++)
            {
                for (int j = 0; matrix[0].Count > j; j++)
                {
                    if (matrix[i][j] == c)
                    {
                        return (i, j);
                    }
                }
            }
            Assert.Fail("Robot not found");
            return (0, 0);
        }

        private void PrintMap(List<List<char>> matrix)
        {
            PrintMap(matrix, ' ');
        }
        private void PrintMap(List<List<char>> matrix, char c)
        {
            File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", c + "\n");
            for (int i = 0; matrix.Count > i; i++)
            {
                string line = "";
                for (int j = 0; matrix[0].Count > j; j++)
                {
                    line += matrix[i][j];
                }
                File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", line + "\n");
            }
            File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", "\n");
        }

        private (int, int) MoveRobot(List<List<char>> matrix, (int, int) robot, char direction)
        {
            (int, int) movedirection = FindMoveDirection(direction);

            bool canmove = false;
            var checkpos = (robot.Item1 + movedirection.Item1, robot.Item2 + movedirection.Item2);
            while (matrix[checkpos.Item1][checkpos.Item2] != '#')
            {
                if (matrix[checkpos.Item1][checkpos.Item2] == 'O')
                {
                    checkpos = (checkpos.Item1 + movedirection.Item1, checkpos.Item2 + movedirection.Item2);
                    continue;
                }
                if (matrix[checkpos.Item1][checkpos.Item2] == '.')
                {
                    canmove = true;
                    break;
                }
                movedirection = (robot.Item1, robot.Item2);
            }

            if (canmove)
            {
                if (matrix[robot.Item1 + movedirection.Item1][robot.Item2 + movedirection.Item2] == 'O')
                {
                    matrix[checkpos.Item1][checkpos.Item2] = 'O';
                }
                matrix[robot.Item1][robot.Item2] = '.';
                matrix[robot.Item1 + movedirection.Item1][robot.Item2 + movedirection.Item2] = '@';
                return (robot.Item1 + movedirection.Item1, robot.Item2 + movedirection.Item2);
            }
            return robot;

        }

        private static (int, int) FindMoveDirection(char direction)
        {
            (int, int) movedirection = (0, 0);
            switch (direction)
            {

                case '^':
                movedirection = (-1, 0);
                break;
                case 'v':
                movedirection = (1, 0);
                break;
                case '<':
                movedirection = (0, -1);
                break;
                case '>':
                movedirection = (0, 1);
                break;
            }

            return movedirection;
        }

        public List<(int, int)> tree = new List<(int, int)>{
           (-1,0),
           (-1,-1),
           (-1,1),
           (0,-1),
           (0,1),
           (1,0),
           (1,-1),
           (1,1),
            (2,0),
            (2,-1),
            (2,1),
            (2,2),
            (2,-2),
            (0,2),
            (0,-2),
            (-2,0),
            (-2,1),
            (-2,-1),
            (-2,2),
            (-2,-2),
            (1,2),
            (1,-2),
            (-1,2),
            (-1,-2),
        };

        private void PrintMap(List<(int, int)> allEndPositions, (int, int) mapsize)
        {
            string line = "";

            for (int y = 0; y < mapsize.Item2; y++)
            {
                for (int x = 0; x < mapsize.Item1; x++)
                {
                    var numberofRobots = allEndPositions.Where(pos => pos.Item1 == x && pos.Item2 == y);
                    if (numberofRobots.Any())
                    {
                        line += $"{numberofRobots.Count()}";
                    } else
                    {
                        line += ".";
                    }
                }
                File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\14Out.txt", line + "\n");
                line = "";
            }
            File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\14Out.txt", "\n\n");
        }

        private List<(long, long)> FindAllWaysForThePriceWithMAth((int, int) buttonA, (int, int) buttonB, (long, long) movedprice)
        {
            double AX = buttonA.Item1;
            double BX = buttonA.Item2;
            double AY = buttonB.Item1;
            double BY = buttonB.Item2;
            double GA = movedprice.Item1;
            double GB = movedprice.Item2;
            double step1 = (GB / BX) * AX;
            double step2 = (BY / BX) * AX;
            double step3 = GA - step1;
            double step4 = AY - step2;
            double step5 = step3 / step4;
            double step6 = Math.Round(step5, 8);
            if (long.TryParse(step6.ToString(), out long result))
            {
                double res2 = (GA - result * AY) / AX;
                double reoundres2 = Math.Round(res2, 8);
                if (long.TryParse(reoundres2.ToString(), out long result2))
                {
                    if (result >= 0 && result2 >= 0)
                    {
                        return new List<(long, long)> { (result2, result) };
                    }
                }
            }
            return new List<(long, long)>();
        }

        private List<(int, int)> FindAllWaysForThePrice((int, int) buttona, (int, int) buttonb, (int, int) price)
        {
            List<(int, int)> goals = new List<(int, int)>();
            for (int a = 0; a < 100; a++)
            {
                for (int b = 0; b < 100; b++)
                {
                    if (a * buttona.Item1 + b * buttonb.Item1 == price.Item1 &&
                                               a * buttona.Item2 + b * buttonb.Item2 == price.Item2)
                    {
                        goals.Add((a, b));
                    }
                }
            }
            return goals;
        }

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

        private void RunInTheMazeSlow(List<List<char>> matrix, (int, int) myPosition, (int, int) direction, (int, int) end, List<(int, int)> visitedPositions, int cost)
        {



            // CreateLog 
            if (visitedPositions.Contains(myPosition) || cost >= TotalCost)
            {

                //File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", $"Map Costs At wrong End{cost}\n");
                //PrintMapWithPath(matrix, visitedPositions);
                return;
            }

            visitedPositions.Add(myPosition);

            //CheckCost
            if (myPosition == end)
            {
                if (cost < TotalCost)
                {

                    TotalCost = cost;
                    File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", $"New TotalCosts{TotalCost}\n");

                }
                //File.AppendAllText(@"C:\Repos\GitHub\AdventOfCode\2024\Inputs\DebugOutput.txt", $"Map Costs At End{cost}\n");
                PrintMapWithPath(matrix, visitedPositions);
                return;
            }
            List<(int, int)> visitedright = new List<(int, int)>();
            List<(int, int)> visitedleft = new List<(int, int)>();
            visitedleft.AddRange(visitedPositions);
            visitedright.AddRange(visitedPositions);

            // RunDeeper
            if (CheckForSpace(myPosition, direction, matrix))
            {
                RunInTheMazeSlow(matrix, (myPosition.Item1 + direction.Item1, myPosition.Item2 + direction.Item2), direction, end, visitedPositions, cost + 1);
            }
            List<(int, int)> newdirections;
            if (direction.Item1 == 0)
            {
                newdirections = new List<(int, int)>() { (-1, 0), (1, 0) };
            } else
            {
                newdirections = new List<(int, int)>() { (0, 1), (0, -1) };
            }

            if (CheckForSpace(myPosition, newdirections[0], matrix))
            {
                RunInTheMazeSlow(matrix, (myPosition.Item1 + newdirections[0].Item1, myPosition.Item2 + newdirections[0].Item2), newdirections[0], end, visitedright, cost + 1001);
            }
            if (CheckForSpace(myPosition, newdirections[1], matrix))
            {
                RunInTheMazeSlow(matrix, (myPosition.Item1 + newdirections[1].Item1, myPosition.Item2 + newdirections[1].Item2), newdirections[1], end, visitedleft, cost + 1001);
            }
        }
    }
}



