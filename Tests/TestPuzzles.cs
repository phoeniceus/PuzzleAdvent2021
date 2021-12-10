using PuzzleAdvent2021;
using System;
using Xunit;

namespace PuzzleAdvent2021Test
{
  public class TestPuzzles
  {
    [Theory]
    [InlineData(1, 7)]
    [InlineData(2, 5)]
    public void TestPuzzle_Day1(int part, int result)
    {
      var puzzle = new Puzzle_Day01(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 150)]
    [InlineData(2, 900)]
    public void TestPuzzle_Day2(int part, int result)
    {
      var puzzle = new Puzzle_Day02(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 198)]
    [InlineData(2, 230)]
    public void TestPuzzle_Day3(int part, int result)
    {
      var puzzle = new Puzzle_Day03(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 4512)]
    [InlineData(2, 1924)]
    public void TestPuzzle_Day4(int part, int result)
    {
      var puzzle = new Puzzle_Day04(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 12)]
    public void TestPuzzle_Day5(int part, int result)
    {
      var puzzle = new Puzzle_Day05(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 5934)]
    [InlineData(2, 26984457539)]
    public void TestPuzzle_Day6(int part, long result)
    {
      var puzzle = new Puzzle_Day06(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 37)]
    [InlineData(2, 168)]
    public void TestPuzzle_Day7(int part, long result)
    {
      var puzzle = new Puzzle_Day07(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 26)]
    [InlineData(2, 61229)]
    public void TestPuzzle_Day8(int part, long result)
    {
      var puzzle = new Puzzle_Day08(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 15)]
    [InlineData(2, 1134)]
    public void TestPuzzle_Day9(int part, long result)
    {
      var puzzle = new Puzzle_Day09(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 26397)]
    [InlineData(2, 288957)]
    public void TestPuzzle_Day10(int part, long result)
    {
      var puzzle = new Puzzle_Day10(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }
  }
}
