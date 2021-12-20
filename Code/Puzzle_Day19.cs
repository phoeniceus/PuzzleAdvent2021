using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day19 : Puzzle
  {
    public Puzzle_Day19(int part, bool useTestData = false) : base(19, part, useTestData)
    {
    }

    public override string Solve()
    {
      var scanners = ReadScanners(ReadInput().ToArray()).ToList();
      var (preMatch, postMatch) = FindAMatch(scanners);
      while (preMatch != null)
      {
        scanners.Remove(preMatch);
        scanners.Add(postMatch);
        (preMatch, postMatch) = FindAMatch(scanners);
      }
      if (scanners.Skip(1).Any(s => s.Position == new Position(0,0,0))) { throw new ApplicationException("Could not find matches for all scanners."); }

      switch (Part)
      {
        case 1:
          var beacons = scanners.SelectMany(x => x.Beacons).Distinct().ToArray();
          return beacons.Length.ToString();
        case 2:
          var best = 0;
          for (int i = 0; i < scanners.Count - 1; i++)
          {
            for (int j = i + 1; j < scanners.Count; j++)
            {
              var delta = scanners[i].Position - scanners[j].Position;
              var manhattanDistance = Math.Abs(delta.x) + Math.Abs(delta.y) + Math.Abs(delta.z);
              if (manhattanDistance > best) { best = manhattanDistance; }
            }
          }
          return best.ToString();
        default:
          return "no such part";
      }
    }

    private (Scanner preMatch, Scanner postMatch) FindAMatch(List<Scanner> scanners)
    {
      var preMatches = scanners.Skip(1).Where(s => s.Position == new Position(0, 0, 0)).ToList();
      var resolved = scanners.Except(preMatches).ToList();
      foreach (var preMatch in preMatches)
      {
        foreach (var resolve in resolved)
        {
          var postMatch = resolve.RotateOtherToMatchMe(preMatch);
          if (postMatch != null) { return (preMatch, postMatch); }
        }
      }
      return (null, null);
    }

    public static IEnumerable<Scanner> ReadScanners(string[] lines)
    {
      var beacons = new List<Position>();
      foreach (var line in lines)
      {
        if (line.Trim() == "")
        {
          yield return new Scanner(beacons: beacons);
          beacons = new List<Position>();
        }
        else if (!line.StartsWith("---"))
        {
          beacons.Add(new Position(line));
        }
      }
      if (beacons.Count > 0) { yield return new Scanner(beacons: beacons); }
    }

    public enum Dimension { x, y, z }

    public struct Direction
    {
      public readonly Dimension dimension;
      public readonly bool positive;
      public Direction(Dimension dimension, bool positive)
      {
        this.dimension = dimension;
        this.positive = positive;
      }

      public static Direction GetRight(Direction facing, Direction up)
      {
        switch (facing.dimension)
        {
          case Dimension.x:
            switch (up.dimension)
            {
              case Dimension.y: return new Direction(Dimension.z, facing.positive == up.positive);
              case Dimension.z: return new Direction(Dimension.y, facing.positive != up.positive);
            }
            break;
          case Dimension.y:
            switch (up.dimension)
            {
              case Dimension.x: return new Direction(Dimension.z, facing.positive != up.positive);
              case Dimension.z: return new Direction(Dimension.x, facing.positive == up.positive);
            }
            break;
          case Dimension.z:
            switch (up.dimension)
            {
              case Dimension.x: return new Direction(Dimension.y, facing.positive == up.positive);
              case Dimension.y: return new Direction(Dimension.x, facing.positive != up.positive);
            }
            break;
        }
        throw new ArgumentException("Invalid facing and up arguments.");
      }

      public static Direction[] AllDirections = new[]
      {
        new Direction(Dimension.x, true),
        new Direction(Dimension.x, false),
        new Direction(Dimension.y, true),
        new Direction(Dimension.y, false),
        new Direction(Dimension.z, true),
        new Direction(Dimension.z, false),
      };
    }

    public class Orientation
    {
      // First coordinate direction
      public Direction Facing { get; private set; }
      // Second coordinate direction
      public Direction Up { get; private set; }
      // Third coordinate direction
      public Direction Right => Direction.GetRight(Facing, Up);

      public Orientation(Direction facing, Direction up)
      {
        if (facing.dimension == up.dimension) { throw new ArgumentException("Facing and Up cannot have the same dimension."); }
        Facing = facing;
        Up = up;
      }

      public static Orientation[] AllOrientations = new[]
      {
        new Orientation(new Direction(Dimension.x, true), new Direction(Dimension.y, true)),
        new Orientation(new Direction(Dimension.x, true), new Direction(Dimension.y, false)),
        new Orientation(new Direction(Dimension.x, true), new Direction(Dimension.z, true)),
        new Orientation(new Direction(Dimension.x, true), new Direction(Dimension.z, false)),
        new Orientation(new Direction(Dimension.x, false), new Direction(Dimension.y, true)),
        new Orientation(new Direction(Dimension.x, false), new Direction(Dimension.y, false)),
        new Orientation(new Direction(Dimension.x, false), new Direction(Dimension.z, true)),
        new Orientation(new Direction(Dimension.x, false), new Direction(Dimension.z, false)),
        new Orientation(new Direction(Dimension.y, true), new Direction(Dimension.x, true)),
        new Orientation(new Direction(Dimension.y, true), new Direction(Dimension.x, false)),
        new Orientation(new Direction(Dimension.y, true), new Direction(Dimension.z, true)),
        new Orientation(new Direction(Dimension.y, true), new Direction(Dimension.z, false)),
        new Orientation(new Direction(Dimension.y, false), new Direction(Dimension.x, true)),
        new Orientation(new Direction(Dimension.y, false), new Direction(Dimension.x, false)),
        new Orientation(new Direction(Dimension.y, false), new Direction(Dimension.z, true)),
        new Orientation(new Direction(Dimension.y, false), new Direction(Dimension.z, false)),
        new Orientation(new Direction(Dimension.z, true), new Direction(Dimension.y, true)),
        new Orientation(new Direction(Dimension.z, true), new Direction(Dimension.y, false)),
        new Orientation(new Direction(Dimension.z, true), new Direction(Dimension.x, true)),
        new Orientation(new Direction(Dimension.z, true), new Direction(Dimension.x, false)),
        new Orientation(new Direction(Dimension.z, false), new Direction(Dimension.y, true)),
        new Orientation(new Direction(Dimension.z, false), new Direction(Dimension.y, false)),
        new Orientation(new Direction(Dimension.z, false), new Direction(Dimension.x, true)),
        new Orientation(new Direction(Dimension.z, false), new Direction(Dimension.x, false))
      };
    }

    public struct Position : IComparable<Position>
    {
      public readonly int x;
      public readonly int y;
      public readonly int z;

      public Position(string line)
      {
        var regex = new Regex(@"^(?<x>\-?\d+),(?<y>\-?\d+),(?<z>\-?\d+)$");
        var match = regex.Match(line);
        if (!match.Success) { throw new ArgumentException("line is not in the correct format."); }
        x = Int32.Parse(match.Groups["x"].Value);
        y = Int32.Parse(match.Groups["y"].Value);
        z = Int32.Parse(match.Groups["z"].Value);
      }

      public Position(int x, int y, int z)
      {
        this.x = x;
        this.y = y;
        this.z = z;
      }

      public override string ToString()
      {
        return $"{x},{y},{z}";
      }

      public override bool Equals(object obj) => obj is Position other && this.Equals(other);

      public bool Equals(Position p) => x == p.x && y == p.y && z == p.z;

      public override int GetHashCode() => (x, y, z).GetHashCode();

      public int CompareTo(Position other)
      {
        var compareTo = x.CompareTo(other.x);
        if (compareTo != 0) { return compareTo; }
        compareTo = y.CompareTo(other.y);
        if (compareTo != 0) { return compareTo; }
        return z.CompareTo(other.z);
      }

      public static bool operator ==(Position lhs, Position rhs) => lhs.Equals(rhs);

      public static bool operator !=(Position lhs, Position rhs) => !(lhs == rhs);

      public static Position operator +(Position a, Position b) => new Position(a.x + b.x, a.y + b.y, a.z + b.z);
      public static Position operator -(Position a, Position b) => new Position(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public class Scanner
    {
      public Orientation Orientation { get; set; }
      public Position[] Beacons { get; private set; }
      public Position Position { get; set; }

      public Scanner(Orientation orientation = null, IEnumerable<Position> beacons = null, Position? position = null)
      {
        Orientation = orientation ?? new Orientation(
          new Direction(Dimension.x, true),
          new Direction(Dimension.y, true)
          );
        Beacons = beacons?.OrderBy(a => a).ToArray();
        Position = position.HasValue ? position.Value : new Position(0, 0, 0);
      }

      public Scanner Convert(Orientation newOrientation)
      {
        var beacons = Beacons?.Select(b => Convert(b, newOrientation)).ToList();
        return new Scanner(newOrientation, beacons);
      }

      public Position Convert(Position beacon, Orientation newOrientation)
      {
        return new Position(GetCoordinate(beacon, newOrientation.Facing), GetCoordinate(beacon, newOrientation.Up), GetCoordinate(beacon, newOrientation.Right));
      }

      private int GetCoordinate(Position p, Direction d)
      {
        if (d.dimension == Orientation.Facing.dimension)
        {
          return (d.positive == Orientation.Facing.positive) ? p.x : -p.x;
        }
        else if (d.dimension == Orientation.Up.dimension)
        {
          return (d.positive == Orientation.Up.positive) ? p.y : -p.y;
        }
        else
        {
          return (d.positive == Orientation.Right.positive) ? p.z : -p.z;
        }
      }

      public Scanner RotateOtherToMatchMe(Scanner other)
      {
        foreach (var orientation in Orientation.AllOrientations)
        {
          var convertedOther = other.Convert(orientation);

          for (int i = 0; i < Beacons.Length; i++)
          {
            for (int j = 0; j < convertedOther.Beacons.Length; j++)
            {
              var delta = Beacons[i] - convertedOther.Beacons[j];
              var translatedBeacons = convertedOther.Beacons.Select(b => b + delta).ToArray();
              var common = Beacons.Intersect(translatedBeacons).ToArray();
              if (common.Length >= 12)
              {
                return new Scanner(orientation, translatedBeacons, convertedOther.Position + delta);
              }
            }
          }
        }
        return null;
      }
    }
  }
}
