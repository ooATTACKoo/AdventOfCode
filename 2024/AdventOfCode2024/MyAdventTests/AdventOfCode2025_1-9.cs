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
        if (number > 99)
        {
          Console.WriteLine(number);

        }
        if (direction == 'R')
        {
          startpos = (startpos + number);
          while (startpos > 99)
          {
            startpos = startpos - 100;
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


    [TestMethod]
    public void Day2a()
    {
      string data = FileReader.LoadFileIntoOneString("2025/02A.txt");
      data = data.Replace("\r\n", "").Replace("\n", "");
      List<string> ranges = data.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
      long result = 0;
      foreach (string range in ranges)
      {
        List<string> numbers = range.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        string start = numbers.First();
        string end = numbers.Last();

        long startint = long.Parse(start);
        long endint = long.Parse(end);
        for (long i = startint; i <= endint; i++)
        {
          string checknumber = i.ToString();
          if (checknumber.Length % 2 == 1)
          { continue; }

          string frontnumber = checknumber.Substring(0, checknumber.Length / 2);
          string endnumber = checknumber.Substring(checknumber.Length / 2, checknumber.Length / 2);

          if (frontnumber == endnumber)
          {
            result += i;
          }

        }
      }
      Assert.AreEqual(result, 31000881061);
    }


    [TestMethod]
    public void Day2b()
    {
      string data = FileReader.LoadFileIntoOneString("2025/02A.txt");
      data = data.Replace("\r\n", "").Replace("\n", "");
      List<string> ranges = data.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
      long result = 0;
      foreach (string range in ranges)
      {
        List<string> numbers = range.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        string start = numbers.First();
        string end = numbers.Last();

        long startint = long.Parse(start);
        long endint = long.Parse(end);
        for (long i = startint; i <= endint; i++)
        {
          string checknumber = i.ToString();
          result = IdFinderByString(result, i, checknumber);
        }

      }
      Assert.AreEqual(result, 46769308485);
    }

    private static long IdFinderByString(long result, long i, string checknumber)
    {
      for (int splitpos = 1; splitpos <= checknumber.Length / 2; splitpos++)
      {
        if (checknumber.Length % splitpos != 0)
        {
          continue;
        }

        string frontnumber = checknumber.Substring(0, splitpos);
        bool isId = true;
        for (int pos = splitpos; pos < checknumber.Length; pos += splitpos)
        {
          string nextpart = checknumber.Substring(pos, splitpos);
          if (frontnumber != nextpart)
          {
            isId = false;
            break;
          }
        }

        if (isId)
        {
          result += i;
          break;
        }
      }

      return result;
    }
  }
}