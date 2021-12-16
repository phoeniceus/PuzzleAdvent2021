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

    public class UnvisitedList
    {
      private Dictionary<int, List<Position>> _positionsByDistance;
      public UnvisitedList(IEnumerable<Position> _positions)
      {
        _positionsByDistance = _positions.GroupBy(p => p.distance).ToDictionary(x => x.Key, x => x.ToList());
      }

      public void Remove(Position position)
      {
        if (_positionsByDistance.TryGetValue(position.distance, out List<Position> list))
        {
          list.Remove(position);
          if (list.Count == 0) { _positionsByDistance.Remove(position.distance); }
        }
        position.visited = true;
      }

      public void Add(Position position)
      {
        List<Position> list = null;
        if (!_positionsByDistance.TryGetValue(position.distance, out  list))
        {
          list = new List<Position>();
          _positionsByDistance.Add(position.distance, list);
        }
        list.Add(position);
        position.visited = false;
      }

      public Position PopNext()
      {
        if (_positionsByDistance.Count > 0)
        {
          var key = _positionsByDistance.Keys.Min();
          var list = _positionsByDistance[key];
          var index = list.Count - 1;
          var position = list[index];
          list.RemoveAt(index);
          if (list.Count == 0) { _positionsByDistance.Remove(position.distance); }
          position.visited = true;
          return position;
        }
        return null;
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
        var unvisited = new UnvisitedList(GetAll());
        unvisited.Remove(Initial);
        Initial.distance = 0;
        for (Position current = Initial; !Destination.visited && current != Destination && current != null && current.distance != Int32.MaxValue; current = unvisited.PopNext())
        {
          DykstraStep(current, unvisited);
        }
      }

      private void DykstraStep(Position current, UnvisitedList unvisited)
      {
        foreach (var neighbor in GetNeighbors(current))
        {
          var tentative = current.distance + neighbor.risk;
          if (tentative < neighbor.distance) { 
            neighbor.distance = tentative; 
            if (!neighbor.visited)
            {
              unvisited.Remove(neighbor);
              unvisited.Add(neighbor);
            }
          }
        }
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

      private Position Initial => _positions[0, 0];
      public Position Destination => _positions[_positions.GetLength(0) - 1, _positions.GetLength(1) - 1];
    }
  }
}
