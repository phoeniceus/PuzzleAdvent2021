using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day07 : Puzzle
  {
    public Puzzle_Day07(int part, bool useTestData = false) : base(7, part, useTestData)
    {
    }

    public override string Solve()
    {
      var positions = ReadPositions();
      var min = positions.Min();
      var max = positions.Max();
      var best = Int32.MaxValue;
      for (int i = min; i <= max; i++)
      {
        var fuel = FuelCost(positions, i);
        if (fuel < best) {best = fuel; }
      }
      return best.ToString();
    }

    private int[] ReadPositions()
    {
      return ReadInput().First().Split(',').Select(x => Int32.Parse(x)).ToArray();
    }

    private int FuelCost(int[] positions, int newPosition)
    {
      switch (Part)
      {
        case 1:
          return positions.Sum(x => Math.Abs(x - newPosition));
        case 2:
          return positions.Sum(x => Factorial(Math.Abs(x - newPosition)));
        default:
          throw new ApplicationException("Part value not supported.");
      }
    }

    private int Factorial(int i) => (i * (i + 1)) / 2;
  }
}
