using System.Collections.Generic;
using System.Drawing;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyAdventTests
{
  public static class Visualizer
  {

    public static void CreateGifFromImages(string[] imageFiles, string outputGif)
    {
      using (var collection = new MagickImageCollection())
      {
        foreach (var file in imageFiles)
        {
          var image = new MagickImage(file);
          image.AnimationDelay = 3; // 10 = 0.1s per frame
          collection.Add(image);
        }
        collection.Write(outputGif);
      }
    }

    public static void SaveMatrixAsImage(List<List<char>> matrix, string filename)
    {
      int cellSize = 10;
      int width = matrix[0].Count * cellSize;
      int height = matrix.Count * cellSize;
      using (var bmp = new Bitmap(width, height))
      using (var g = Graphics.FromImage(bmp))
      {
        g.Clear(Color.Blue);
        for (int row = 0; row < matrix.Count; row++)
        {
          for (int col = 0; col < matrix[row].Count; col++)
          {
            Color color = matrix[row][col] == '@' ? Color.Yellow : Color.Blue;
            g.FillRectangle(new SolidBrush(color), (col * cellSize) + 1, (row * cellSize) + 1, cellSize - 2, cellSize - 2);
          }
        }
        bmp.Save(filename);
      }
    }
  }
}