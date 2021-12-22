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

    [Theory]
    [InlineData(1, 1656)]
    [InlineData(2, 195)]
    public void TestPuzzle_Day11(int part, long result)
    {
      var puzzle = new Puzzle_Day11(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 226)]
    [InlineData(2, 3509)]
    public void TestPuzzle_Day12(int part, long result)
    {
      var puzzle = new Puzzle_Day12(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 17)]
    //[InlineData(2, 3509)]
    public void TestPuzzle_Day13(int part, long result)
    {
      var puzzle = new Puzzle_Day13(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 1588)]
    [InlineData(2, 2188189693529)]
    public void TestPuzzle_Day14(int part, long result)
    {
      var puzzle = new Puzzle_Day14(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 40)]
    [InlineData(2, 315)]
    public void TestPuzzle_Day15(int part, long result)
    {
      var puzzle = new Puzzle_Day15(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, "8A004A801A8002F478", 16)]
    [InlineData(1, "620080001611562C8802118E34", 12)]
    [InlineData(1, "C0015000016115A2E0802F182340", 23)]
    [InlineData(1, "A0016C880162017C3686B18A3D4780", 31)]
    [InlineData(2, "C200B40A82", 3)]
    [InlineData(2, "04005AC33890", 54)]
    [InlineData(2, "880086C3E88112", 7)]
    [InlineData(2, "CE00C43D881120", 9)]
    [InlineData(2, "D8005AC2A8F0", 1)]
    [InlineData(2, "F600BC2D8F", 0)]
    [InlineData(2, "9C005AC2F8F0", 0)]
    [InlineData(2, "9C0141080250320F1802104A08", 1)]
    public void TestPuzzle_Day16(int part, string code, ulong result)
    {
      var puzzle = new Puzzle_Day16(part, code);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 45)]
    [InlineData(2, 112)]
    public void TestPuzzle_Day17(int part, long result)
    {
      var puzzle = new Puzzle_Day17(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 4140)]
    [InlineData(2, 3993)]
    public void TestPuzzle_Day18(int part, long result)
    {
      var puzzle = new Puzzle_Day18(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 79)]
    [InlineData(2, 3621)]
    public void TestPuzzle_Day19(int part, long result)
    {
      var puzzle = new Puzzle_Day19(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 35)]
    [InlineData(2, 3351)]
    public void TestPuzzle_Day20(int part, long result)
    {
      var puzzle = new Puzzle_Day20(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 739785)]
    [InlineData(2, 444356092776315)]
    public void TestPuzzle_Day21(int part, long result)
    {
      var puzzle = new Puzzle_Day21(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }

    [Theory]
    [InlineData(1, 590784)]
    [InlineData(2, 2758514936282235)]
    public void TestPuzzle_Day22(int part, long result)
    {
      var puzzle = new Puzzle_Day22(part, useTestData: true);
      Assert.Equal(result.ToString(), puzzle.Solve());
    }
  }
}
