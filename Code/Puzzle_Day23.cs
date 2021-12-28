using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day23 : Puzzle
  {
    public Puzzle_Day23(int part, bool useTestData = false) : base(23, part, useTestData)
    {
      /*
       * This puzzle presented memory and CPU challenges.
       * 
       * It is not possible to store cube values in arrays because there are trillions of cubes
       * and C# cannot store an array with more than 2 billion elements.
       * 
       * It is not even possible to run each cube through all the steps, counting as we go, because 
       * it would take hours to run.
       * 
       * The solution is to model cuboids as ranges of x, y, and z values (6 ints) and to 
       * treat each step as a set of cuboids covering the cubes that were turned on by this
       * step and by no later step. Then it is just a matter of erasing later step cuboid
       * from earlier step cuboids.
       * 
       * After all steps are processed, each individual step is reduced to a small number of 
       * cuboids (6 tuples of integers) where it was the last step to turn those cubes on.
       * Then we can very easily count them.
       */
    }

    public override string Solve()
    {
      var board = new Board(ReadInput().ToArray());
      return board.GetSolution().ToString();
    }

    public enum Direction { Up, Down, Left, Right }

    public struct Position
    {
      public readonly int col;
      public readonly int row;

      public Position(int col, int row) { this.col = col; this.row = row; }

      public Position Move(Direction d)
      {
        switch (d)
        {
          case Direction.Up: return new Position(col, row - 1);
          case Direction.Down: return new Position(col, row + 1);
          case Direction.Left: return new Position(col - 1, row);
          case Direction.Right: return new Position(col + 1, row);
          default: throw new ApplicationException("Invalid direction.");
        }
      }

      public bool IsHallway => row == 1 && col > 0 && col < 12;
      public bool IsAlcove => row == 1 && (col == 3 || col == 5 || col == 7 || col == 9);
      public static int RoomColFor(char ch) => ((ch - 'A') * 2) + 3;
      public bool IsRoomFor(char ch, int roomSize) => col == RoomColFor(ch) && row >= 2 && row <= roomSize + 1;
      public bool IsRoom(int roomSize) => (col == 3 || col == 5 || col == 7 || col == 9) && row >= 2 && row <= roomSize + 1;


      public static IEnumerable<Position> GetAllOccupiable(int roomSize)
      {
        for (int c = 1; c < 12; c++)
        {
          yield return new Position(c, 1);
        }
        for (int c = 3; c <= 9; c += 2)
        {
          for (int r = 2; r <= roomSize + 1; r++)
          {
            yield return new Position(c, r);
          }
        }
      }

      public override bool Equals(object obj) => obj is Position other && this.Equals(other);

      public bool Equals(Position p) => col == p.col && row == p.row;

      public override int GetHashCode() => (col, row).GetHashCode();

      public static bool operator ==(Position lhs, Position rhs) => lhs.Equals(rhs);

      public static bool operator !=(Position lhs, Position rhs) => !(lhs == rhs);

      public override string ToString() => $"{col},{row}";
    }

    public class Path : List<Position>
    {
      public char Amphipod { get; set; }
      public Path() : base() { }
      public Path(IEnumerable<Position> items) : base(items) { }
      public Position Start => this[0];
      public Position End => this[Count - 1];

      public Path Extend(Position p, char? amphipod = null)
      {
        var result = new Path();
        result.AddRange(this);
        result.Add(p);
        result.Amphipod = amphipod ?? Amphipod;
        return result;
      }
    }

    public class Board
    {
      private char[,] _board;
      private int _energy;
      private Stack<Path> _moves;
      private int _roomSize;
      private Board(int roomSize)
      {
        _roomSize = roomSize;
        _board = new char[13, 3 + roomSize];
        _energy = 0;
        _moves = new Stack<Path>();
        for (int row = 0; row < 3 + roomSize; row++)
        {
          for (int col = 0; col < 13; col++)
          {
            _board[col, row] = CharExtensions.OUTSIDE;
          }
        }
      }

      public Board(string[] lines) : this(lines.Length - 3)
      {
        for (int row = 0; row < lines.Length; row++)
        {
          for (int col = 0; col < Math.Min(_board.GetLength(0), lines[row].Length); col++)
          {
            _board[col, row] = lines[row][col];
          }
        }
      }

      public override string ToString()
      {
        var sb = new StringBuilder();
        for (int row = 0; row < _board.GetLength(1); row++)
        {
          for (int col = 0; col < _board.GetLength(0); col++)
          {
            sb.Append(_board[col, row]);
          }
          sb.AppendLine();
        }
        return sb.ToString();
      }

      public bool Success => Position.GetAllOccupiable(_roomSize).All(p => p.IsHallway || p.IsRoomFor(GetOccupant(p), _roomSize));

      public void PushMove(Path move)
      {
        SetOccupant(move.Start, CharExtensions.OPEN);
        SetOccupant(move.End, move.Amphipod);
        _energy += (move.Count - 1) * move.Amphipod.EnergyPerMove();
        _moves.Push(move);
      }

      public void PopMove()
      {
        var move = _moves.Pop();
        SetOccupant(move.End, CharExtensions.OPEN);
        SetOccupant(move.Start, move.Amphipod);
        _energy -= (move.Count - 1) * move.Amphipod.EnergyPerMove();
      }

      private char GetOccupant(Position p) => _board[p.col, p.row];
      private void SetOccupant(Position p, char ch) { _board[p.col, p.row] = ch; }


      private int _leastEnergy;
      private int _percentDone;

      public int GetSolution()
      {
        _leastEnergy = Int32.MaxValue;// ((25 * 1000) + (25 * 100) + (25 * 10)) * 4;
        _percentDone = 0;
        GetSolutionIter(0, 100);
        return _leastEnergy;
      }

      private void GetSolutionIter(double minPercent, double maxPercent)
      {
        var lastMove = _moves.TryPeek(out Path m) ? m : null;
        var possibleNextMoves = GetAllPossibleNextMoves().ToArray();
        if (possibleNextMoves.Length == 0) { return; }
        var percent = minPercent;
        var deltaPercent = (maxPercent - minPercent) / possibleNextMoves.Length;
        foreach (var move in possibleNextMoves)
        {
          // Don't move an amphipod twice in a row. This prevents moving it back and forth forever.
          if (lastMove != null && move.Start == lastMove.End) { continue; }

          PushMove(move);
          if (_energy + MinimumEnergyNeededToFinish() < _leastEnergy)
          {
            if (Success)
            {
              if (_energy < _leastEnergy) { _leastEnergy = _energy; }
            }
            else
            {
              GetSolutionIter(percent, percent + deltaPercent);
            }
          }
          PopMove();
          percent += deltaPercent;
          if ((int)percent > _percentDone)
          {
            _percentDone = (int)percent;
            Debug.Write(_percentDone + " ");
          }
        }
      }

      private int MinimumEnergyNeededToFinish()
      {
        var energy = 0;
        var count = new int[4];
        foreach (var start in GetValidStartPositions())
        {
          var ch = GetOccupant(start);
          var end = new Position(Position.RoomColFor(ch), 2);
          var distance = (start.row - 1) + Math.Abs(end.col - start.col) + 1 + count[ch - 'A'];
          count[ch - 'A']++;
          energy += distance * ch.EnergyPerMove();
        }
        return energy;
      }

      private IEnumerable<Position> GetValidStartPositions() => Position.GetAllOccupiable(_roomSize).Where(p => IsAmphipodButNotHome(p));

      private bool IsAmphipodButNotHome(Position position)
      {
        // Is this position occupied by an amphipod? If not, return false.
        var ch = GetOccupant(position);
        return ch.IsAmphipod() && !PositionIsFinalSpotForAmphipod(position, ch);
      }

      private bool PositionIsFinalSpotForAmphipod(Position position, char amphipod)
      {
        // Is this position a home spot for the amphipod?
        if (!position.IsRoomFor(amphipod, _roomSize)) { return false; }

        // Are all home spots below me occupied by the same type amphipod?
        for (var p = position; p.row <= _roomSize + 1; p = p.Move(Direction.Down))
        {
          if (GetOccupant(p) != amphipod) { return false; }
        }

        // Otherwise, the amphipod is done moving.
        return true;
      }

      private bool IsValidEnd(Position start, Position end)
      {
        // Must end on an open position
        if (!GetOccupant(end).IsOpen()) { return false; }

        // May not end on an alcove
        if (end.IsAlcove) { return false; }

        // May not move from hallway to hallway
        if (start.IsHallway)
        {
          if (end.IsHallway) { return false; }
        }

        // May only move to a room if it is the destination room 
        // AND room does not contain other amphipods that don't belong there.
        // AND all spots are filled from bottom up
        if (end.IsRoom(_roomSize))
        {
          var ch = GetOccupant(start);

          // If I'm not in an end room, return false
          if (!end.IsRoomFor(ch, _roomSize)) { return false; }

          // If any home spot below me is not occupied by the same type amphipod, return false;
          for (int r = end.row + 1; r <= _roomSize + 1; r++)
          {
            if (GetOccupant(new Position(end.col, r)) != ch) { return false; }
          }
        }

        return true;
      }

      public IEnumerable<Path> GetAllPossibleNextMoves() => GetValidStartPositions().SelectMany(x => GetPossibleNextMovesFromPosition(x));

      public IEnumerable<Path> GetPossibleNextMovesFromPosition(Position start) => GetValidPaths(new Path().Extend(start, GetOccupant(start)));

      private IEnumerable<Path> GetValidPaths(Path pathSoFar)
      {
        if (IsValidEnd(pathSoFar.Start, pathSoFar.End)) { yield return pathSoFar; }

        var current = pathSoFar.End;
        var above = current.Move(Direction.Up);
        var below = current.Move(Direction.Down);
        var left = current.Move(Direction.Left);
        var right = current.Move(Direction.Right);

        Position? prev = (pathSoFar.Count > 1) ? pathSoFar[pathSoFar.Count - 2] : null;
        foreach (var next in new[] { above, below, left, right })
        {
          if (next == prev) { continue; } // Don't move same amphipod twice in a row.

          if (GetOccupant(next).IsOpen())
          {
            var newPath = pathSoFar.Extend(next);
            foreach (var result in GetValidPaths(newPath)) { yield return result; }
          }
        }
      }
    }
  }

  public static class CharExtensions
  {
    public const char WALL = '#';
    public const char OPEN = '.';
    public const char OUTSIDE = ' ';

    public static int EnergyPerMove(this char ch) => ch switch
    {
      'A' => 1,
      'B' => 10,
      'C' => 100,
      'D' => 1000,
      _ => throw new ArgumentOutOfRangeException(nameof(ch), $"Not expected amphipod type: {ch}"),
    };

    public static bool IsAmphipod(this char ch) => ch >= 'A' && ch <= 'D';
    public static bool IsOpen(this char ch) => ch == OPEN;
  }
}
