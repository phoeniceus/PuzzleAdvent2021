using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day05 : Puzzle
  {
    public Puzzle_Day05(int part, bool useTestData = false) : base(5, part, useTestData)
    {
    }

    public override string Solve()
    {
      var lines = ReadLines(includeHorizontal: true, includeVertical: true, includeDiagonal: Part == 2).ToArray();
      var map = new Map(lines);
      return map.CountWhereTwoLinesOverlap.ToString();
    }

    public IEnumerable<Line> ReadLines(bool includeHorizontal = false, bool includeVertical = false, bool includeDiagonal = false)
    {
      return ReadInput().Select(x => new Line(x)).Where(x =>
        (includeHorizontal && x.IsHorizontal) ||
        (includeVertical && x.IsVertical) ||
        (includeDiagonal && x.IsDiagonal));
    }
  }

  public struct Point
  {
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) { X = x; Y = y; }
    public override string ToString()
    {
      return $"{X},{Y}";
    }
    public override bool Equals(object obj)
    {
      if (obj is Point)
      {
        var o = (Point)obj;
        return (X == o.X && Y == o.Y);
      }
      return false;
    }
    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public static bool operator ==(Point a, Point b) => a.Equals(b);
    public static bool operator !=(Point a, Point b) => !a.Equals(b);
  }

  public struct Line
  {
    private static Regex Regex_Line = new Regex(@"^(?<x1>\d+)\,(?<y1>\d+)\s\-\>\s(?<x2>\d+)\,(?<y2>\d+)$");
    public Point From { get; }
    public Point To { get; }
    public Line(Point from, Point to) { From = from; To = to; }
    public Line(string input)
    {
      if (input == null) { throw new ArgumentNullException(nameof(input)); }
      var match = Regex_Line.Match(input);
      if (!match.Success) { throw new ArgumentException($"{nameof(input)} is not valid format."); }
      From = new Point(Int32.Parse(match.Groups["x1"].Value), Int32.Parse(match.Groups["y1"].Value));
      To = new Point(Int32.Parse(match.Groups["x2"].Value), Int32.Parse(match.Groups["y2"].Value));
    }
    public bool IsHorizontal => From.Y == To.Y;
    public bool IsVertical => From.X == To.X;
    public bool IsDiagonal => Math.Abs(From.X - To.X) == Math.Abs(From.Y - To.Y);
    public IEnumerable<Point> GetPointsInLine()
    {
      for (
        var point = From;
        ((From.X <= point.X && point.X <= To.X) || (To.X <= point.X && point.X <= From.X)) &&
        ((From.Y <= point.Y && point.Y <= To.Y) || (To.Y <= point.Y && point.Y <= From.Y));
        point = new Point(point.X + Math.Sign(To.X - From.X), point.Y + Math.Sign(To.Y - From.Y))
        )
      {
        yield return point;
      }
    }

    public override string ToString()
    {
      return $"{From} -> {To}";
    }
  }

  public class Map
  {
    private int[,] _data;

    public Map(Line[] lines)
    {
      var maxX1 = lines.Max(z => z.From.X);
      var maxY1 = lines.Max(z => z.From.Y);
      var maxX2 = lines.Max(z => z.To.X);
      var maxY2 = lines.Max(z => z.To.Y);
      _data = new int[Math.Max(maxX1, maxX2) + 1, Math.Max(maxY1, maxY2) + 1];
      foreach (var line in lines)
      {
        foreach (var point in line.GetPointsInLine())
        {
          _data[point.X, point.Y]++;
        }
      }
    }

    public int CountWhereTwoLinesOverlap
    {
      get
      {
        var result = 0;
        for (int y = 0; y < _data.GetLength(1); y++)
        {
          for (int x = 0; x < _data.GetLength(0); x++)
          {
            if (_data[x, y] >= 2) { result++; }
          }
        }
        return result;
      }
    }

    public override string ToString()
    {
      var sb = new StringBuilder();
      for (int y = 0; y < _data.GetLength(1); y++)
      {
        for (int x = 0; x < _data.GetLength(0); x++)
        {
          if (x != 0) { sb.Append(" "); }
          sb.Append(_data[x, y].ToString("00"));
        }
        sb.AppendLine();
      }
      return sb.ToString();
    }
  }
}
