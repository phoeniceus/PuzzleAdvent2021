using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day14 : Puzzle
  {
    public Puzzle_Day14(int part, bool useTestData = false) : base(14, part, useTestData)
    {
    }

    public override string Solve()
    {
      var (polymer, insertions) = ReadData();
      for (int i = 0; i < (Part == 1 ? 10 : 40); i++)
      {
        polymer.Insert(insertions);
      }
      var histogram = polymer.GetHistogram();
      var max = histogram.Max(x => x.Value);
      var min = histogram.Min(x => x.Value);
      var result = max - min;
      return result.ToString();
    }

    private (Polymer, Dictionary<string, string>) ReadData()
    {
      var lines = ReadInput().ToArray();
      return (new Polymer(lines[0]), lines.Skip(2).ToDictionary(p => p[0..2], p => p.Last().ToString()));
    }

    public string Insert(string polymer, Dictionary<string, string> insertions)
    {
      var sb = new StringBuilder();
      for (int i = 0; i < polymer.Length - 1; i++)
      {
        sb.Append(polymer[i]);
        var pair = polymer[i..(i + 2)];
        if (insertions.TryGetValue(pair, out string insertion))
        {
          sb.Append(insertion);
        }
      }
      sb.Append(polymer.Last());
      return sb.ToString();
    }

    public class Polymer
    {
      private Dictionary<string, long> _pairs;
      private char _firstLetter;
      private char _lastLetter;

      public Polymer(string line)
      {
        _pairs = new Dictionary<string, long>();
        for (int i = 0; i < line.Length - 1; i++)
        {
          var key = line[i..(i + 2)];
          _pairs[key] = (_pairs.ContainsKey(key) ? _pairs[key] : 0) + 1;
        }
        _firstLetter = line[0];
        _lastLetter = line[^1];
      }

      public void Insert(Dictionary<string, string> insertions)
      {
        var newPairs = new Dictionary<string, long>();
        foreach (var insertion in insertions)
        {
          if (_pairs.TryGetValue(insertion.Key, out long count))
          {
            var newPair1 = insertion.Key[0] + insertion.Value;
            var newPair2 = insertion.Value + insertion.Key[1];

            newPairs[newPair1] = (newPairs.ContainsKey(newPair1) ? newPairs[newPair1] : 0) + count;
            newPairs[newPair2] = (newPairs.ContainsKey(newPair2) ? newPairs[newPair2] : 0) + count;
            _pairs.Remove(insertion.Key);
          }
        }
        _pairs = newPairs;
      }

      public Dictionary<char, long> GetHistogram()
      {
        var result = new Dictionary<char, long>();
        foreach (var pair in _pairs)
        {
          var countNew = pair.Value;

          var pair0 = pair.Key[0];
          if (!result.TryGetValue(pair0, out long count0)) { count0 = 0; }
          result[pair0] = count0 + countNew;

          var pair1 = pair.Key[1];
          if (!result.TryGetValue(pair1, out long count1)) { count1 = 0; }
          result[pair1] = count1 + countNew;
        }

        result[_firstLetter]++;
        result[_lastLetter]++;

        foreach (var key in result.Keys)
        {
          result[key] /= 2;
        }

        return result;
      }
    }
  }
}
