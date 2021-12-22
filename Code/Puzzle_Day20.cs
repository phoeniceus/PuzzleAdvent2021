using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day20 : Puzzle
  {
    public Puzzle_Day20(int part, bool useTestData = false) : base(20, part, useTestData)
    {
    }

    public override string Solve()
    {
      var lines = ReadInput().ToArray();
      var enhancer = Enhancer.Read(lines[0]);
      var image = new Image(lines.Skip(2).ToArray());
      for (int i = 0; i < ((Part==1) ? 2 : 50); i++)
      {
        image = image.Enhance(enhancer);
      }
      return image.LightCount.ToString();
    }

    public static class Enhancer
    {
      public static string Read(string line)
      {
        if (line == null) { throw new ArgumentNullException(nameof(line)); }
        var regex = new Regex(@"^[\#\.]{512}$");
        if (!regex.IsMatch(line)) { throw new ArgumentException("line is not correct format."); }
        return line;
      }
    }

    public class Image
    {
      public const char DARK = '.';
      public const char LIGHT = '#';

      public char[,] Pixels { get; private set; }
      public char InfinitePixel { get; private set; }
      public int Width => Pixels.GetLength(0);
      public int Height => Pixels.GetLength(1);
      public Image(string[] lines)
      {
        Pixels = new char[lines[0].Length, lines.Length];
        for (int r = 0; r < lines.Length; r++)
        {
          for (int c = 0; c < lines[r].Length; c++)
          {
            Pixels[c, r] = lines[r][c];
          }
        }
        InfinitePixel = DARK;
      }

      public Image(char[,] pixels, char infinitePixel) { Pixels = pixels; InfinitePixel = infinitePixel; }

      public Image Enhance(string enhancer)
      {
        var newPixels = new char[Width + 2, Height + 2];
        for (int newRow = 0; newRow < Height + 2; newRow++)
        {
          var row = newRow - 1;
          for (int newCol = 0; newCol < Width + 2; newCol++)
          {
            var col = newCol - 1;
            newPixels[newCol, newRow] = Enhance(enhancer, col, row);
          }
        }
        return new Image(newPixels, (InfinitePixel == DARK) ? enhancer[0]: enhancer[511]);
      }

      public char Enhance(string enhancer, int col, int row)
      {
        var index = 0;
        for (int r = row - 1; r <= row + 1; r++)
        {
          for (int c = col - 1; c <= col + 1; c++)
          {
            index *= 2;
            if (0 <= r && r <= Height - 1 && 0 <= c && c <= Width - 1)
            {
              if (Pixels[c, r] == LIGHT) { index += 1; }
            }
            else if (InfinitePixel == LIGHT) { index += 1; }
          }
        }
        return enhancer[index];
      }

      public int LightCount
      {
        get
        {
          if (InfinitePixel == LIGHT) { return Int32.MaxValue; }
          int count = 0;
          for (int r = 0; r < Height; r++)
          {
            for (int c = 0; c < Width; c++)
            {
              if (Pixels[c, r] == LIGHT) { count++; }
            }
          }
          return count;
        }
      }
    }
  }
}
