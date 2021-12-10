using System;

namespace PuzzleAdvent2021
{
  class Program
  {
    static void Main(string[] args)
    {
      for (int puzzle = 9; puzzle <= 9; puzzle++)
      {
        for (int part = 1; part <= 2; part++)
        {
          Console.WriteLine($"The answer to Puzzle #{puzzle}.{part} is [{GetPuzzle(puzzle, part).Solve()}].");
        }
      }
    }

    static Puzzle GetPuzzle(int day, int part) =>
      day switch
      {
        1 => new Puzzle_Day01(part),
        2 => new Puzzle_Day02(part),
        3 => new Puzzle_Day03(part),
        4 => new Puzzle_Day04(part),
        5 => new Puzzle_Day05(part),
        6 => new Puzzle_Day06(part),
        7 => new Puzzle_Day07(part),
        8 => new Puzzle_Day08(part),
        9 => new Puzzle_Day09(part),
        _ => null
      };
  }
}
