using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day17 : Puzzle
  {
    public Puzzle_Day17(int part, bool useTestData = false) : base(17, part, useTestData)
    {
    }

    public override string Solve()
    {
      var ta = new TargetArea(ReadInput().First());
      var result = (Part == 1) ? ta.BestHit() : ta.TotalHits();
      return result.ToString();
    }

    public class TargetArea
    {
      public readonly int x1;
      public readonly int y1;
      public readonly int x2;
      public readonly int y2;

      public TargetArea(string line)
      {
        var regex = new Regex(@"^target\sarea:\sx=(?<x1>\-?\d+)..(?<x2>\-?\d+),\sy=(?<y1>\-?\d+)..(?<y2>\-?\d+)$");
        var match = regex.Match(line);
        if (!match.Success) { throw new ArgumentException("line is not correct format."); }
        x1 = Int32.Parse(match.Groups["x1"].Value);
        y1 = Int32.Parse(match.Groups["y1"].Value);
        x2 = Int32.Parse(match.Groups["x2"].Value);
        y2 = Int32.Parse(match.Groups["y2"].Value);
        if (x1 > x2) { throw new ArgumentException("x1 > x2"); }
        if (y1 > y2) { throw new ArgumentException("y1 > y2"); }
      }

      public int? BestHit()
      {
        int? bestHeight = null;
        for (int deltaX = 1; deltaX <= x2; deltaX++)
        {
          var maxX = (deltaX * (deltaX + 1)) / 2;
          if (maxX < x1) { continue; }

          for (int deltaY = y1; deltaY <= (-y1 * (x2 / 2)); deltaY++)
          {
            var positions = Shoot(deltaX, deltaY).ToArray();
            if (positions.Any(p => Inside(p)))
            {
              var height = positions.Max(p => p.y);
              if (bestHeight == null || height > bestHeight.Value) { bestHeight = height; }
            }
            else if (positions.Any(p => Overshot(p))) { break; }
          }
        }
        return bestHeight;
      }

      public int TotalHits()
      {
        int count = 0;
        for (int deltaX = 1; deltaX <= x2; deltaX++)
        {
          var maxX = (deltaX * (deltaX + 1)) / 2;
          if (maxX < x1) { continue; }

          for (int deltaY = y1; deltaY <= (-y1 * (x2 / 2)); deltaY++)
          {
            var positions = Shoot(deltaX, deltaY).ToArray();
            if (Inside(positions.Last()))
            {
              //Debug.Write($"({deltaX},{deltaY}) => ");
              //foreach (var p in positions) { Debug.Write($"({p.x},{p.y}) "); }
              //Debug.WriteLine("");
              count++;
            }
            else if (positions.Any(p => Overshot(p))) { break; }
          }
        }
        return count;
      }

      public IEnumerable<Position> Shoot(int deltaX, int deltaY)
      {
        var p = new Position(0, 0);
        while (!Missed(p))
        {
          yield return p;
          if (Inside(p)) { yield break; }
          p = new Position(p.x + deltaX, p.y + deltaY);
          deltaX = (deltaX == 0) ? 0 : ((deltaX > 0) ? deltaX - 1 : deltaX + 1);
          deltaY -= 1;
        }
      }

      public bool Inside(Position p) => x1 <= p.x && p.x <= x2 && y1 <= p.y && p.y <= y2;
      public bool Missed(Position p) => p.x > x2 || p.y < y1;

      public bool Overshot(Position p) => p.x > x2 && p.y >= y2;
    }

    public struct Position
    {
      public readonly int x;
      public readonly int y;
      public Position(int x, int y) { this.x = x; this.y = y; }
    }
  }
}
