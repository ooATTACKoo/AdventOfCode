using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAdventTests;

namespace AdventOfCode2020
{
    [TestClass]
    public class AOC20201_8

    {
        [TestMethod]
        public void Day1()
        {
            int a = 0;
            int b = 0;
            int goal = 2020;
            var data = FileReader.LoadFileIntoAList("2020/01.txt");
            for(int i=0; i < data.Count; i++)
            {
                for(int j = i+1; j < data.Count; j++)
                {
                    if (data[i] + data[j] == goal)
                    {
                         a = data[i];
                         b = data[j];
                        break;
                    }
                }
                if (a!=0 && b!=0)
                {
                    break;
                }
            }

            long result = a * b;
            Assert.AreEqual(121396, result);
        }

        [TestMethod]
        public void Day1B()
        {
            int a = 0;
            int b = 0;
            int c = 0;
            int goal = 2020;
            var data = FileReader.LoadFileIntoAList("2020/01.txt");
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = i + 1; j < data.Count; j++)
                {
                    for (int k = j + 1; k < data.Count; k++)
                    {
                        if (data[i] + data[j] + data[k] == goal  && data[i] * data[j] * data[k]>0)
                        {
                            a = data[i];
                            b = data[j];
                            c= data[k];
                            break;
                        }
                    }
                }
                if (a != 0 && b != 0 && c!=0)
                {
                    break;
                }
            }

            long result = a * b * c;
            Assert.AreEqual(73616634, result);
        }
    }
}
