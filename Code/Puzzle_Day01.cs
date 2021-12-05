using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day01 : Puzzle
  {
    public Puzzle_Day01(int part, bool useTestData = false) : base(1, part, useTestData)
    {
    }

    private int WindowSize
    {
      get
      {
        switch (Part)
        {
          case 1: return 1;
          case 2: return 3;
          default: throw new ArgumentOutOfRangeException(nameof(Part));
        }
      }
    }

    public override string Solve()
    {
      int result = 0;
      var values = ReadInput().Select(x => Int32.Parse(x)).ToArray();
      int lastValue = 0;
      for (int i = 0; i < values.Length; i++)
      {
        var nextValue = lastValue + values[i];
        if (i >= WindowSize)
        {
          nextValue -= values[i - WindowSize];
          var diff = nextValue - lastValue;
          if (diff > 0) { result++; }
        }
        lastValue = nextValue;
      }
      return result.ToString();
    }

  }
}
