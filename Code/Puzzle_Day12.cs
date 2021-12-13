using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day12 : Puzzle
  {
    public Puzzle_Day12(int part, bool useTestData = false) : base(12, part, useTestData)
    {
    }

    public override string Solve()
    {
      var graph = GetGraph();
      var paths = graph.GetPaths().ToArray();
      return paths.Length.ToString();
    }

    private Graph GetGraph()
    {
      return new Graph(ReadInput(), Part);
    }

    public class Edge
    {
      public string Cave1 { get; set; }
      public string Cave2 { get; set; }
    }

    private class Graph
    {
      private Edge[] Edges { get; set; }
      private int Part { get; set; }
      public Graph(IEnumerable<string> lines, int part)
      {
        var edges = new List<Edge>();
        foreach (var line in lines)
        {
          var split = line.Split('-');
          edges.Add(new Edge { Cave1 = split[0], Cave2 = split[1] });
        }
        Edges = edges.ToArray();
        Part = part;
      }

      public IEnumerable<string[]> GetPaths(string[] pathSoFar = null)
      {
        if (pathSoFar == null)
        {
          pathSoFar = new[] { "start" };
        }

        var last = pathSoFar.Last();
        if (last == "end") { yield return pathSoFar; }
        else
        {
          var others = GetNeighbors(last).ToArray();
          foreach (var other in others)
          {
            var next = Append(pathSoFar, other);
            if (IsValidPath(next))
            {
              foreach (var path in GetPaths(next)) { yield return path; }
            }
          }
        }
      }

      private IEnumerable<string> GetNeighbors(string cave)
      {
        foreach (var edge in Edges)
        {
          if (edge.Cave1 == cave) { yield return edge.Cave2; }
          else if (edge.Cave2 == cave) { yield return edge.Cave1; }
        }
      }

      private string[] Append(string[] path, string append)
      {
        return path.ToList().Append(append).ToArray();
      }

      private bool IsValidPath(string[] path)
      {
        var smallCaves = path.Where(x => x.ToLower() == x);
        var smallCaveCount = smallCaves.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        var revisitedCaves = smallCaveCount.Where(x => x.Value > 1).Select(x => x.Key).ToArray();
        if (revisitedCaves.Length == 0) { return true; }

        if (Part == 2)
        {
          if (revisitedCaves.Length > 1) { return false; }
          var revisitedCave = revisitedCaves[0];
          if (revisitedCave == "start") { return false; }
          if (revisitedCave == "end") { return false; }
          return smallCaveCount[revisitedCave] == 2;
        }
        return false;
      }
    }
  }
}
