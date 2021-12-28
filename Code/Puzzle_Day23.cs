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
    }

    public override string Solve()
    {
      var board = new Board(ReadInput().ToArray());
      return board.GetSolution().ToString();
    }

    public struct Position
    {
      public readonly int col;
      public readonly int row;

      public Position(int col, int row) { this.col = col; this.row = row; }

      public Position Above => new Position(col, row - 1);
      public Position Below => new Position(col, row + 1);
      public Position Left => new Position(col - 1, row);
      public Position Right => new Position(col + 1, row);

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

    public struct Move
    {
      public char Amphipod { get; set; }
      public Position Start { get; set; }
      public Position End { get; set; }
      public int Count { get; set; }
    }

    public class Board
    {
      private char[,] _board;
      private int _energy;
      private Stack<Move> _moves;
      private int _roomSize;
      private int _leastEnergy;

      private Board(int roomSize)
      {
        _roomSize = roomSize;
        _board = new char[13, 3 + roomSize];
        _energy = 0;
        _moves = new Stack<Move>();
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

      public bool Success => Position.GetAllOccupiable(_roomSize).All(p => p.IsHallway || p.IsRoomFor(GetOccupant(p), _roomSize));

      public void PushMove(Move move)
      {
        SetOccupant(move.Start, CharExtensions.OPEN);
        SetOccupant(move.End, move.Amphipod);
        _energy += move.Count * move.Amphipod.EnergyPerMove();
        _moves.Push(move);
      }

      public void PopMove()
      {
        var move = _moves.Pop();
        SetOccupant(move.End, CharExtensions.OPEN);
        SetOccupant(move.Start, move.Amphipod);
        _energy -= move.Count * move.Amphipod.EnergyPerMove();
      }

      private char GetOccupant(Position p) => _board[p.col, p.row];
      private void SetOccupant(Position p, char ch) { _board[p.col, p.row] = ch; }

      public int GetSolution()
      {
        _leastEnergy = Int32.MaxValue;
        GetSolutionIter(0, 100);
        return _leastEnergy;
      }

      private void GetSolutionIter()
      {
        Move lastMove = _moves.TryPeek(out Move m) ? m : new Move();
        var possibleNextMoves = GetAllPossibleNextMoves().ToArray();
        if (possibleNextMoves.Length == 0) { return; }
        foreach (var move in possibleNextMoves)
        {
          // Don't move an amphipod twice in a row. This prevents moving it back and forth forever.
          if (move.Start == lastMove.End) { continue; }

          PushMove(move);
          if (_energy + MinimumEnergyNeededToFinish() < _leastEnergy)
          {
            if (Success)
            {
              if (_energy < _leastEnergy) { _leastEnergy = _energy; }
            }
            else
            {
              GetSolutionIter();
            }
          }
          PopMove();
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

      private IEnumerable<(Position position, char amphipod)> FindMoveableAmphipods()
      {
        foreach (var position in Position.GetAllOccupiable(_roomSize))
        {
          var ch = GetOccupant(position);
          if (ch.IsAmphipod())
          {
            if (!PositionIsFinalSpotForAmphipod(position, ch))
            {
              yield return (position, ch);
            }
          }
        }
      }

      private bool PositionIsFinalSpotForAmphipod(Position position, char amphipod)
      {
        // Is this position a home spot for the amphipod?
        if (!position.IsRoomFor(amphipod, _roomSize)) { return false; }

        // Are all home spots below me occupied by the same type amphipod?
        for (var p = position; p.row <= _roomSize + 1; p = p.Below)
        {
          if (GetOccupant(p) != amphipod) { return false; }
        }

        // Otherwise, the amphipod is done moving.
        return true;
      }

      public IEnumerable<Move> GetAllPossibleNextMoves() => FindMoveableAmphipods().SelectMany(x => GetPossibleMoves(x.position, x.amphipod));

      public IEnumerable<Move> GetPossibleMoves(Position start, char amphipod)
      {
        // Note - this relies on Move being a struct, not a class
        var move = new Move();
        move.Start = start;
        move.Amphipod = amphipod;

        // Go up until we reach the hallway
        var above = start;
        while (GetOccupant(above.Above).IsOpen())
        {
          above = above.Above;
          move.Count++;
        }
        if (!above.IsHallway) { yield break; } // blocked before reaching hallway

        var startIsHallway = start.IsHallway;
        var homeColumn = Position.RoomColFor(amphipod);
        var countToHomeAlcove = 0;
        var countToAbove = move.Count;

        // Go Left until we hit the wall
        var left = above;
        while (GetOccupant(left.Left).IsOpen())
        {
          left = left.Left;
          move.Count++;
          if (!left.IsAlcove)
          {
            if (!startIsHallway)
            {
              move.End = left;
              yield return move;
            }
          }
          else if (left.col == homeColumn)
          {
            countToHomeAlcove = move.Count;
          }
        }

        // Go Right until we hit the wall
        var right = above;
        move.Count = countToAbove;
        while (GetOccupant(right.Right).IsOpen())
        {
          right = right.Right;
          move.Count++;
          if (!right.IsAlcove)
          {
            if (!startIsHallway)
            {
              move.End = right;
              yield return move;
            }
          }
          else if (right.col == homeColumn)
          {
            countToHomeAlcove = move.Count;
          }
        }

        // Go Down from alcove until we determine if this is a final spot
        if (countToHomeAlcove > 0)
        {
          move.Count = countToHomeAlcove;
          var below = new Position(homeColumn, 1);
          while (GetOccupant(below.Below).IsOpen())
          {
            below = below.Below;
            move.Count++;
          }
          if (below.IsAlcove) { yield break; } // Room is blocked

          // below = last reachable open spot in room. Make sure everything below me is a matching amphipod.
          var under = below;
          while (GetOccupant(under.Below) == amphipod)
          {
            under = under.Below;
          }
          if (under.row == _roomSize + 1)
          {
            move.End = below;
            yield return move;
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
