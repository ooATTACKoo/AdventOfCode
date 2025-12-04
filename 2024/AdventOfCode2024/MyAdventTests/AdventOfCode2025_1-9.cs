using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using ImageMagick;

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

          if (frontnumber.Equals(endnumber))
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
           result = CheckIdForRepeatingStructure(result, i);
        }

      }
      Assert.AreEqual(result, 46769308485);
    }

    [TestMethod]
    public void Day3a()
    {
      string data = FileReader.LoadFileIntoOneString("2025/03A.txt");
      List<string> lines = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
      int second = 0;
      int first = 0;
      int result = 0;
      foreach (string line in lines)
      {
        for (int i = 0; i < line.Length; i++)
        {
          int bat = int.Parse(line[i].ToString());
          if (bat > first && i != line.Length - 1)
          {
            second = 0;
            first = bat;
          } else if (bat > second)
          {
            second = bat;
          }
        }
        int jolt = (first * 10 + second);
        first = second = 0;
        result += jolt;
      }
      Assert.AreEqual(result, 17316);
    }

    [TestMethod]
    public void Day3b()
    {
      string data = FileReader.LoadFileIntoOneString("2025/03A.txt");
      List<string> lines = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
      long result = 0;
      foreach (string line in lines)
      {
        int killIx = -1;
        string changedline = line;
        for (int t = 0; t < line.Length-12; t++)
        {
          for (int i = 0; i < changedline.Length - 1; i++)
          {
            int before = int.Parse(changedline[i].ToString());
            int after = int.Parse(changedline[i + 1].ToString());
            if (before < after)
            {
              killIx = i;
              break;
            }
            if (i == changedline.Length - 2)
            {
              killIx = i+1;
            }
          }
          changedline = changedline.Remove(killIx, 1);
        }

        long jolt = long.Parse(changedline);
        result += jolt;       

      }
      Assert.AreEqual(result, 171741365473332);
    }

    [TestMethod]
    public void Day4a()
    {
      var matrix = FileReader.LoadFileIntoAStringMatrix("2025/04A.txt");
      int counter = 0;
      for (int row = 0; row < matrix.Count; row++)
      {
        var line = matrix[row];
        for (int col = 0; col < line.Count; col++)
        { 
          if (line[col] != '@') {
            continue;
          }
          int atCounter = CountSymbolsInMatrixAroundMe(matrix, row, col, '@');
          if ( atCounter<4)
          {
            counter++;
          }
        }
      }
      Assert.AreEqual(1428, counter);
    }

    [TestMethod]
    public void Day4b()
    {
      var matrix = FileReader.LoadFileIntoAStringMatrix("2025/04A.txt");
      int counter = 0;
      int imageprintcounter = 0;
      bool changed = true;
      List<string> imageFiles = new List<string>();
      while (changed)
      {
        changed = false;
        for (int row = 0; row < matrix.Count; row++)
        {
          var line = matrix[row];
          for (int col = 0; col < line.Count; col++)
          {
            if (line[col] != '@')
            {
              continue;
            }
            int atCounter = CountSymbolsInMatrixAroundMe(matrix, row, col, '@');
            if (atCounter < 4)
            {
              changed = true;
              matrix[row][col] = '.';
              imageprintcounter++;
              counter++;
              if (imageprintcounter % 20 == 0)
              {
                Visualizer.SaveMatrixAsImage(matrix, $"c:\\aoc\\Day4b_Step_{counter}.png");
                imageFiles.Add($"c:\\aoc\\Day4b_Step_{counter}.png");
              }

            }
          }

        }
        imageprintcounter = 0;
        Visualizer.SaveMatrixAsImage(matrix, $"c:\\aoc\\Day4b_Step_{counter}.png");
        imageFiles.Add($"c:\\aoc\\Day4b_Step_{counter}.png");
      }
      Visualizer.CreateGifFromImages(imageFiles.ToArray(), "c:\\aoc\\Day4b_AnimationBlue.gif");
      foreach (var file in imageFiles)
      {
        File.Delete(file);
      }
      Assert.AreEqual(8936, counter);
    }

    List<(int row, int col)> directions = new List<(int row, int col)>
    {
      (-1, -1), (-1, 0), (-1, 1),
      (0, -1),          (0, 1),
      (1, -1),  (1, 0), (1, 1)
    };

    private int CountSymbolsInMatrixAroundMe(List<List<char>> matrix, int row, int col, char v)
    {
      int count = 0;
      foreach (var dir in directions)
      {
        (int row,int col) checkPos = (row + dir.row, col + dir.col);
        if (checkPos.row<0 || checkPos.col <0 || checkPos.row >= matrix.Count || checkPos.col >= matrix[0].Count )
        {
            continue;
        }
        if (matrix[checkPos.row][checkPos.col] == v)
        {
          count++;
        }
      }
      return count;
    }

    private long BuildJolt(string line, List<int> notUsed) {
      long result = 0;

      for (int i = 0; i < line.Length; i++) {
        if (notUsed.Contains(i)) {
          continue;
        }
        int point = int.Parse(line[i].ToString());
        result = result * 10 + point;
      }
      return result;
    }


    private static long CheckIdForRepeatingStructure(long result, long idToCheck)
    {
      string idToCheckAsString = idToCheck.ToString();
      for (int splitpos = 1; splitpos <= idToCheckAsString.Length / 2; splitpos++)
      {
        if (idToCheckAsString.Length % splitpos != 0)
        {
          continue;
        }

        string frontnumber = idToCheckAsString.Substring(0, splitpos);
        bool isId = true;
        for (int pos = splitpos; pos < idToCheckAsString.Length; pos += splitpos)
        {
          string nextpart = idToCheckAsString.Substring(pos, splitpos);
          if (!frontnumber.Equals(nextpart))
          {
            isId = false;
            break;
          }
        }

        if (isId)
        {
          result += idToCheck;
          break;
        }
      }

      return result;
    }
  }
}