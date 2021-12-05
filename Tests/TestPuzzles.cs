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
      var puzzle = new Puzzle_Day01(part, useTestData:true);
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
  }
}
