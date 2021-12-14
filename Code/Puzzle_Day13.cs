using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day13 : Puzzle
  {
    public Puzzle_Day13(int part, bool useTestData = false) : base(13, part, useTestData)
    {
    }

    public override string Solve()
    {
      var (graph, folds) = ReadData();
      for (int i = 0; i < ((Part == 1) ? 1 : folds.Length); i++)
      {
        switch (folds[i].axis)
        {
          case "x":
            graph = graph.XFold(folds[i].index);
            break;
          case "y":
            graph = graph.YFold(folds[i].index);
            break;
          default:
            throw new ApplicationException("Invalid fold.");
        }
      }
      return (Part == 1) ? graph.DotCount.ToString() : graph.ToString();
    }

    private (Graph, Fold[]) ReadData()
    {
      var lines = new List<string>();
      var folds = new List<Fold>();
      bool foundBlank = false;
      foreach (var line in ReadInput())
      {
        if (String.IsNullOrWhiteSpace(line)) { foundBlank = true; }
        else
        {
          if (foundBlank)
          {
            folds.Add(new Fold(line));
          }
          else
          {
            lines.Add(line);
          }
        }
      }
      return (new Graph(lines), folds.ToArray());
    }

    public struct Point
    {
      public int x;
      public int y;
      public Point(int x, int y) { this.x = x; this.y = y; }
    }

    public struct Fold
    {
      public string axis;
      public int index;
      public Fold(string line)
      {
        var regex = new Regex(@"^fold along (?<char>.{1})=(?<index>\d+)$");
        var match = regex.Match(line);
        if (!match.Success) { throw new ApplicationException("Invalid file."); }
        axis = match.Groups["char"].Value;
        index = Int32.Parse(match.Groups["index"].Value);
      }
    }

    private class Graph
    {
      private bool[,] _dots;

      public int Width => _dots.GetLength(0);
      public int Height => _dots.GetLength(1);
      public int DotCount
      {
        get
        {
          var result = 0;
          for (int y = 0; y < Height; y++)
          {
            for (int x = 0; x < Width; x++)
            {
              if (_dots[x, y]) { result++; }
            }
          }
          return result;
        }
      }

      public Graph(IEnumerable<string> lines)
      {
        var points = new List<Point>(100);
        foreach (var line in lines)
        {
          if (String.IsNullOrWhiteSpace(line)) { break; }
          var split = line.Split(',');
          var point = new Point(Int32.Parse(split[0]), Int32.Parse(split[1]));
          points.Add(point);
        }
        MakeDots(points);
      }

      public Graph(IEnumerable<Point> points, int width = 0, int height = 0)
      {
        MakeDots(points, width, height);
      }

      private void MakeDots(IEnumerable<Point> points, int width = 0, int height = 0)
      {
        if (width == 0) { width = points.Max(p => p.x) + 1; }
        if (height == 0) { height = points.Max(p => p.y) + 1; }
        _dots = new bool[width, height];
        foreach (var point in points)
        {
          _dots[point.x, point.y] = true;
        }
      }

      public Graph XFold(int xFold)
      {
        var points = new List<Point>();
        for (int y = 0; y < Height; y++)
        {
          for (int x = 0; x < Width; x++)
          {
            if (_dots[x, y])
            {
              if (x < xFold) { points.Add(new Point(x, y)); }
              if (x > xFold) { points.Add(new Point((2 * xFold) - x, y)); }
            }
          }
        }
        return new Graph(points, xFold, Height);
      }

      public Graph YFold(int yFold)
      {
        var points = new List<Point>();
        for (int y = 0; y < Height; y++)
        {
          for (int x = 0; x < Width; x++)
          {
            if (_dots[x, y])
            {
              if (y < yFold) { points.Add(new Point(x, y)); }
              if (y > yFold) { points.Add(new Point(x, (2 * yFold) - y)); }
            }
          }
        }
        return new Graph(points, Width, yFold);
      }

      public override string ToString()
      {
        var sb = new StringBuilder();
        for (int y = 0; y < Height; y++)
        {
          for (int x = 0; x < Width; x++)
          {
            sb.Append(_dots[x, y] ? "#" : ".");
          }
          sb.AppendLine();
        }
        return sb.ToString();
      }
    }
  }
}
