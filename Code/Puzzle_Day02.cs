using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day02 : Puzzle
  {
    public Puzzle_Day02(int part, bool useTestData = false) : base(2, part, useTestData)
    {
    }

    public override string Solve()
    {

      IPosition position = StartPosition;
      foreach (var command in ReadCommands())
      {
        position = position.DoCommand(command);
      }
      return position.Product.ToString();
    }

    public IEnumerable<Command> ReadCommands()
    {
      foreach (var line in this.ReadInput())
      {
        yield return new Command(line);
      }
    }

    private IPosition StartPosition
    {
      get
      {
        switch (Part)
        {
          case 1:
            return new Position1();
          case 2:
            return new Position2();
          default:
            throw new ArgumentOutOfRangeException(nameof(Part));
        }
      }
    }
  }

  public interface IPosition
  {
    int Product { get; }
    IPosition DoCommand(Command c);
  }

  public struct Position1 : IPosition
  {
    public int Horizontal { get; }
    public int Depth { get; }
    public int Product => Horizontal * Depth;
    public Position1(int horizontal, int depth) { Horizontal = horizontal; Depth = depth; }

    public IPosition DoCommand(Command c)
    {
      switch (c.Direction)
      {
        case Direction.down:
          return new Position1(Horizontal, Depth + c.Value);
        case Direction.up:
          return new Position1(Horizontal, Depth - c.Value);
        case Direction.forward:
          return new Position1(Horizontal + c.Value, Depth);
        default:
          throw new InvalidOperationException("Unsupported enum value.");
      }
    }

  }

  public struct Position2 : IPosition
  {
    public int Horizontal { get; }
    public int Depth { get; }
    public int Aim { get; }
    public int Product => Horizontal * Depth;

    public Position2(int horizontal, int depth, int aim) { Horizontal = horizontal; Depth = depth; Aim = aim; }

    public IPosition DoCommand(Command c)
    {
      switch (c.Direction)
      {
        case Direction.down:
          return new Position2(Horizontal, Depth, Aim + c.Value);
        case Direction.up:
          return new Position2(Horizontal, Depth, Aim - c.Value);
        case Direction.forward:
          return new Position2(Horizontal + c.Value, Depth + (Aim * c.Value), Aim);
        default:
          throw new InvalidOperationException("Unsupported enum value.");
      }
    }
  }

  public struct Command
  {
    public static Regex regex = new Regex(@$"(?<direction>({String.Join("|", Enum.GetNames<Direction>())}))\s+(?<value>\d+)");
    public Direction Direction { get; }
    public int Value { get; }
    public Command(Direction direction, int distance) { Direction = direction; Value = distance; }
    public Command(string command)
    {
      if (String.IsNullOrEmpty(command)) { throw new ArgumentNullException(nameof(command)); }
      var match = regex.Match(command);
      if (match.Success)
      {
        Direction = Enum.Parse<Direction>(match.Groups["direction"].Value);
        Value = Int32.Parse(match.Groups["value"].Value);
      }
      else
      {
        throw new ArgumentException($"Invalid command '{command}'.", nameof(command));
      }
    }
  }

  public enum Direction
  {
    forward = 1,
    down = 2,
    up = 3
  }
}
