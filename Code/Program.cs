using System;

namespace PuzzleAdvent2021
{
  class Program
  {
    static void Main(string[] args)
    {
      var puzzle = 23;
      Console.WriteLine($"The answer to Puzzle #{puzzle}.1 is\r\n{GetPuzzle(puzzle, 1).Solve()}.");
      //Console.WriteLine($"The answer to Puzzle #{puzzle}.2 is\r\n{GetPuzzle(puzzle, 2).Solve()}.");
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
        10 => new Puzzle_Day10(part),
        11 => new Puzzle_Day11(part),
        12 => new Puzzle_Day12(part),
        13 => new Puzzle_Day13(part),
        14 => new Puzzle_Day14(part),
        15 => new Puzzle_Day15(part),
        16 => new Puzzle_Day16(part),
        17 => new Puzzle_Day17(part),
        18 => new Puzzle_Day18(part),
        19 => new Puzzle_Day19(part),
        20 => new Puzzle_Day20(part),
        21 => new Puzzle_Day21(part),
        22 => new Puzzle_Day22(part),
        23 => new Puzzle_Day23(part),
        _ => null
      };
  }
}
