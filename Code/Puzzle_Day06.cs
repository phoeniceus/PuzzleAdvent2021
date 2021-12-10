using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day06 : Puzzle
  {
    public Puzzle_Day06(int part, bool useTestData = false) : base(6, part, useTestData)
    {
    }

    public override string Solve()
    {
      var fish = ReadLines();
      var days = Part switch
      {
        1 => 80,
        2 => 256,
        _ => throw new ArgumentOutOfRangeException(nameof(Part))
      };
      for (int i = 0; i < days; i++)
      {
        fish.OneDayLater();
      }
      return fish.Count.ToString();
    }

    public LanternFish ReadLines()
    {
      return new LanternFish(ReadInput().First());
    }
  }

  public class LanternFish
  {
    private long[] _histogram;

    public LanternFish(string line)
    {
      _histogram = new long[9];
      foreach (var item in line.Split(","))
      {
        var timer = Int64.Parse(item);
        _histogram[timer]++;
      }
    }

    public void OneDayLater()
    {
      var zeroCount = _histogram[0];
      for (int i = 0; i < 8; i++)
      {
        _histogram[i] = _histogram[i + 1];
      }
      _histogram[6] += zeroCount;
      _histogram[8] = zeroCount;
    }

    public long Count => _histogram.Sum();
  }
}
