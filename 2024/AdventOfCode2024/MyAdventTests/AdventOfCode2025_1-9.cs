using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MyAdventTests

{
  [TestClass]
  public class AdventOfCode2025
  {

    [TestCategory("L0")]
    [TestMethod]
    public void Day1A()
    {
      string data = FileReader.LoadFileIntoOneString("2025/01A.txt");
      List<string> lines = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
      int startpos = 50;
      int password = 0;
      foreach (string line in lines)
      {
        char direction = line[0];
        int number = int.Parse(line.Substring(1));
        number = number % 100;
        if (number > 100)
        {
          Console.WriteLine(number);

        }
        if (direction == 'R')
        {

          startpos = (startpos + number);
          if (startpos > 99)
          { startpos = startpos - 100; }
        } else
        {
          startpos = (startpos - number);
          if (startpos < 0)
          {
            startpos = startpos + 100;
          }

        }
        if (startpos == 0)
        {
          password++;
        }

      }
      Assert.AreEqual(1168, password);
    }

    [TestMethod]
    public void Day1B()
    {
      string data = FileReader.LoadFileIntoOneString("2025/01A.txt");
      List<string> lines = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
      int startpos = 50;
      int password = 0;
      foreach (string line in lines)
      {
        char direction = line[0];
        int number = int.Parse(line.Substring(1));
        int oldpos = startpos;
        if ( number > 99)
        {
          Console.WriteLine(number);

        }
        if (direction == 'R')
        {
          startpos = (startpos + number);
          while (startpos > 99)
          { startpos = startpos - 100;
            password++;
          }
        } else
        { 
          if (oldpos == 0)
          {
            password--;
          }
          startpos = (startpos - number);

          while (startpos < 0)
          {
            startpos = startpos + 100;
            password++;
          }
          if (startpos == 0)
          { password++; }
        }
      }
      Assert.AreEqual(7199, password);
    }
  }
}