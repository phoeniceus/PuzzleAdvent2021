using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day15 : Puzzle
  {
    public Puzzle_Day15(int part, bool useTestData = false) : base(15, part, useTestData)
    {
    }

    public override string Solve()
    {
      var map = new Map(ReadInput().ToArray(), Part);
      map.Dykstra();
      var result = map.Destination.distance;
      return result.ToString();
    }

    public class Position
    {
      public int col;
      public int row;
      public int risk;
      public bool visited;
      public int distance;
      public Position(int col, int row, int risk)
      {
        this.col = col;
        this.row = row;
        this.risk = risk;
        this.visited = false;
        this.distance = Int32.MaxValue;
      }
      public override string ToString()
      {
        return $"({col},{row}) => Risk={risk} Distance={distance} Visited={visited}";
      }
    }

    public class Map
    {
      public Position[,] _positions;

      public Map(string[] lines, int part)
      {
        var factor = (part == 1) ? 1 : 5;
        _positions = new Position[lines[0].Length * factor, lines.Length * factor];

        for (int tileRow = 0; tileRow < factor; tileRow++)
        {
          var r0 = tileRow * lines.Length;
          for (int tileCol = 0; tileCol < factor; tileCol++)
          {
            var c0 = tileCol * lines[0].Length;
            for (int r = 0; r < lines.Length; r++)
            {
              var row = r0 + r;
              var line = lines[r];
              for (int c = 0; c < line.Length; c++)
              {
                var col = c0 + c;
                var value = ((line[c] - '1' + tileRow + tileCol) % 9) + 1;
                _positions[col, row] = new Position(col, row, value);
              }
            }
          }
        }
      }

      public void Dykstra()
      {
        Initial.distance = 0;
        var unvisited = GetAll().ToList();
        unvisited.Remove(Initial);
        for (Position current = Initial; !Destination.visited && current != Destination && current != null && current.distance != Int32.MaxValue; current = GetNext(unvisited))
        {
          DykstraStep(current);
        }
      }

      private void DykstraStep(Position current)
      {
        //Debug.WriteLine($"Current = {current}");
        foreach (var neighbor in GetNeighbors(current))
        {
          var tentative = current.distance + neighbor.risk;
          if (tentative < neighbor.distance) { neighbor.distance = tentative; }
        }
        current.visited = true;
      }

      private IEnumerable<Position> GetAll()
      {
        for (int r = 0; r < _positions.GetLength(1); r++)
        {
          for (int c = 0; c < _positions.GetLength(0); c++)
          {
            yield return _positions[c, r];
          }
        }
      }

      private IEnumerable<Position> GetNeighbors(Position p)
      {
        if (p.col > 0) { yield return _positions[p.col - 1, p.row]; }
        if (p.row > 0) { yield return _positions[p.col, p.row - 1]; }
        if (p.col < _positions.GetLength(0) - 1) { yield return _positions[p.col + 1, p.row]; }
        if (p.row < _positions.GetLength(1) - 1) { yield return _positions[p.col, p.row + 1]; }
      }

      private Position GetNext(List<Position> unvisited)
      {
        Position best = null;
        foreach (var position in unvisited)
        {
          if (position.visited) { continue; }
          if (best == null || position.distance < best.distance) { best = position; }
        }
        if (best != null)
        {
          unvisited.Remove(best);
        }
        return best;
      }

      private Position Initial => _positions[0, 0];
      public Position Destination => _positions[_positions.GetLength(0) - 1, _positions.GetLength(1) - 1];
    }
  }
}
