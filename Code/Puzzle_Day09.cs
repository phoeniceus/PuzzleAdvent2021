using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day09 : Puzzle
  {
    public Puzzle_Day09(int part, bool useTestData = false) : base(9, part, useTestData)
    {
    }

    public override string Solve()
    {
      var map = ReadMap();
      switch (Part)
      {
        case 1:
          var result1 = map.GetLowest().Sum(b => b.Height + 1);
          return result1.ToString();
        case 2:
          var values = map.GetBasinSizes().OrderByDescending(b => b).Take(3).ToArray();
          var result2 = values[0] * values[1] * values[2];
          return result2.ToString();
        default:
          throw new ApplicationException("Part value not supported.");
      }
    }

    private Map ReadMap()
    {
      return new Map(ReadInput());
    }

    public class Bin
    {
      public int Height { get; set; }
      public bool Lowest { get; set; }
      public int Row { get; set; }
      public int Col { get; set; }
    }


    public class Map
    {
      public Bin[][] Bins { get; set; }

      public Map(IEnumerable<string> lines)
      {
        Bins = lines.Select(x => x.Select(y => new Bin { Height = y - '0' }).ToArray()).ToArray();
        Populate();
      }

      public IEnumerable<Bin> GetLowest()
      {
        for (int r = 0; r < Bins.Length; r++)
        {
          for (int c = 0; c < Bins[r].Length; c++)
          {
            if (Bins[r][c].Lowest)
            {
              yield return Bins[r][c];
            }
          }
        }
      }

      public IEnumerable<int> GetBasinSizes()
      {
        foreach (var bin in GetLowest())
        {
          var neighbors = new List<Bin>();
          GetHigherNeighbors(bin, neighbors);
          yield return neighbors.Count;
        }
      }

      public void GetHigherNeighbors(Bin bin, List<Bin> neighborsSoFar)
      {
        neighborsSoFar.Add(bin);

        var c = bin.Col;
        var r = bin.Row;

        var left = c > 0 ? Bins[r][c - 1] : null;
        if (left != null && left.Height >= bin.Height && left.Height != 9 && !neighborsSoFar.Contains(left))
        {
          GetHigherNeighbors(left, neighborsSoFar);
        }

        var right = c < Bins[r].Length - 1 ? Bins[r][c + 1] : null;
        if (right != null && right.Height >= bin.Height && right.Height != 9 && !neighborsSoFar.Contains(right))
        {
          GetHigherNeighbors(right, neighborsSoFar);
        }

        var above = r > 0 ? Bins[r - 1][c] : null;
        if (above != null && above.Height >= bin.Height && above.Height != 9 && !neighborsSoFar.Contains(above))
        {
          GetHigherNeighbors(above, neighborsSoFar);
        }

        var below = r < Bins.Length - 1 ? Bins[r + 1][c] : null;
        if (below != null && below.Height >= bin.Height && below.Height != 9 && !neighborsSoFar.Contains(below))
        {
          GetHigherNeighbors(below, neighborsSoFar);
        }
      }

      private void Populate()
      {
        for (int r = 0; r < Bins.Length; r++)
        {
          for (int c = 0; c < Bins[r].Length; c++)
          {
            var bin = Bins[r][c];

            bin.Row = r;
            bin.Col = c;

            var height = bin.Height;
            var left = c > 0 ? Bins[r][c - 1].Height : 9;
            var right = c < Bins[r].Length - 1 ? Bins[r][c + 1].Height : 9;
            var above = r > 0 ? Bins[r - 1][c].Height : 9;
            var below = r < Bins.Length - 1 ? Bins[r + 1][c].Height : 9;
            bin.Lowest = height < left && height < right && height < above && height < below;
          }
        }
      }
    }
  }
}
