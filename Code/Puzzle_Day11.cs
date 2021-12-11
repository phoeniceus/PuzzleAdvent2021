using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day11 : Puzzle
  {
    public Puzzle_Day11(int part, bool useTestData = false) : base(11, part, useTestData)
    {
    }

    public override string Solve()
    {
      var octopi = GetOctopi();
      var flashes = 0;
      switch (Part)
      {
        case 1:
          for (int i = 0; i < 100; i++)
          {
            flashes += octopi.DoStep();
          }
          return flashes.ToString();
        case 2:
          var step = 1;
          while ((flashes = octopi.DoStep()) != octopi.Count) { step++; }
          return step.ToString();
        default:
          throw new ApplicationException("Part value not supported.");
      }
    }

    private Octopi GetOctopi()
    {
      return new Octopi(ReadInput());
    }

    public class Octopi
    {
      private static readonly int FLASHED = -1;
      private int[,] _energy;
      public int Count => _energy.GetLength(0) * _energy.GetLength(1);

      public Octopi(IEnumerable<string> lines)
      {
        var input = new List<int[]>();
        foreach (var line in lines)
        {
          input.Add(line.Select(x => x - '0').ToArray());
        }
        _energy = new int[input[0].Length, input.Count];
        for (int r = 0; r < _energy.GetLength(1); r++)
        {
          for (int c = 0; c < _energy.GetLength(0); c++)
          {
            _energy[c, r] = input[r][c];
          }
        }
      }

      public int DoStep()
      {
        var totalCount = 0;
        int flashCount = 0;

        IncrementEveryOctopus();
        while ((flashCount = FindNewFlashersAndIncrementNeighbors()) > 0)
        {
          totalCount += flashCount;
        }
        ResetFlashers();

        return totalCount;
      }

      private void IncrementEveryOctopus()
      {
        for (int r = 0; r < _energy.GetLength(1); r++)
        {
          for (int c = 0; c < _energy.GetLength(0); c++)
          {
            IncrementOctopus(c, r);
          }
        }
      }

      private int FindNewFlashersAndIncrementNeighbors()
      {
        var flashers = FindNewFlashers().ToList();
        foreach (var (c, r) in flashers)
        {
          IncrementOctopus(c - 1, r - 1);
          IncrementOctopus(c - 1, r);
          IncrementOctopus(c - 1, r + 1);
          IncrementOctopus(c, r - 1);
          IncrementOctopus(c, r + 1);
          IncrementOctopus(c + 1, r - 1);
          IncrementOctopus(c + 1, r);
          IncrementOctopus(c + 1, r + 1);
        }
        foreach (var (c,r) in flashers) { _energy[c, r] = FLASHED; }
        return flashers.Count;
      }

      private IEnumerable<(int, int)> FindNewFlashers()
      {
        for (int r = 0; r < _energy.GetLength(1); r++)
        {
          for (int c = 0; c < _energy.GetLength(0); c++)
          {
            if (_energy[c, r] > 9)
            {
              yield return (c, r);
            }
          }
        }
      }

      private void IncrementOctopus(int col, int row)
      {
        if (0 <= col && col < _energy.GetLength(0) && 0 <= row && row < _energy.GetLength(1))
        {
          if (_energy[col, row] != FLASHED)
          {
            _energy[col, row]++;
          }
        }
      }

      private void ResetFlashers()
      {
        for (int r = 0; r < _energy.GetLength(1); r++)
        {
          for (int c = 0; c < _energy.GetLength(0); c++)
          {
            if (_energy[c, r] == FLASHED)
            {
              _energy[c, r] = 0;
            }
          }
        }
      }
    }
  }
}
