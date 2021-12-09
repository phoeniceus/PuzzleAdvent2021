using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day08 : Puzzle
  {
    public Puzzle_Day08(int part, bool useTestData = false) : base(8, part, useTestData)
    {
    }

    public override string Solve()
    {
      switch (Part)
      {
        case 1:
          var countOf1478 = ReadEntries().Sum(x => x.CountOf1478);
          return countOf1478.ToString();
        case 2:
          var sum = ReadEntries().Sum(x => x.Output);
          return sum.ToString();
        default:
          throw new ApplicationException("Part value not supported.");
      }
    }

    private IEnumerable<Entry> ReadEntries()
    {
      return ReadInput().Select(x => new Entry(x));
    }
  }

  public class Entry
  {
    /*
     * 0 -> 6 segments
     * 1 -> 2 segments - unique
     * 2 -> 5 segments
     * 3 -> 5 segments
     * 4 -> 4 segments - unique
     * 5 -> 5 segments
     * 6 -> 6 segments
     * 7 -> 3 segments - unique
     * 8 -> 7 segments - unique
     * 9 -> 6 segments
     */
    public string[] SignalPatterns { get; }
    public string[] OutputValues { get; }
    public string[] Digits { get; }
    public Entry(string line)
    {
      var split = line.Split("|");
      SignalPatterns = split[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
      OutputValues = split[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();

      var signalPattern1 = SignalPatterns.First(x => x.Length == 2);
      var signalPattern4 = SignalPatterns.First(x => x.Length == 4);
      var signalPattern7 = SignalPatterns.First(x => x.Length == 3);
      var signalPattern8 = SignalPatterns.First(x => x.Length == 7);
      var signalPatterns069 = SignalPatterns.Where(x => x.Length == 6).ToArray();

      var crossCF = signalPattern1;
      var crossBD = signalPattern4.Subtract(signalPattern1);
      var crossEG = signalPattern8.Subtract(signalPattern7).Subtract(signalPattern4);
      var crossCDE = (signalPattern8.Subtract(signalPatterns069[0])).Add
        (signalPattern8.Subtract(signalPatterns069[1])).Add
        (signalPattern8.Subtract(signalPatterns069[2]));

      var crossA = signalPattern7.Subtract(signalPattern1);
      var crossB = crossBD.Subtract(crossCDE);
      var crossF = crossCF.Subtract(crossCDE);
      var crossG = crossEG.Subtract(crossCDE);
      var crossD = crossBD.Subtract(crossB);
      var crossC = crossCF.Subtract(crossF);
      var crossE = crossEG.Subtract(crossG);

      Digits = new string[10];
      Digits[0] = signalPattern8.Subtract(crossD);
      Digits[1] = signalPattern1;
      Digits[2] = signalPattern8.Subtract(crossB).Subtract(crossF);
      Digits[3] = signalPattern8.Subtract(crossB).Subtract(crossE);
      Digits[4] = signalPattern4;
      Digits[5] = signalPattern8.Subtract(crossC).Subtract(crossE);
      Digits[6] = signalPattern8.Subtract(crossC);
      Digits[7] = signalPattern7;
      Digits[8] = signalPattern8;
      Digits[9] = signalPattern8.Subtract(crossE);
    }

    public int CountOf1478 => OutputValues.Count(x => new[] { 2, 3, 4, 7 }.Contains(x.Length));

    private int GetDigit(string x)
    {
      for (int i = 0; i < 10; i++)
      {
        if (x.IsEquivalentTo(Digits[i])) { return i; }
      }
      throw new ArgumentException("Invalid LetterSet.");
    }

    public int Output
    {
      get
      {
        return (GetDigit(OutputValues[0]) * 1000) +
          (GetDigit(OutputValues[1]) * 100) +
          (GetDigit(OutputValues[2]) * 10) +
          (GetDigit(OutputValues[3]) * 1);
      }
    }
  }

  public static class StringExtensions
  {
    public static bool IsEquivalentTo(this string literal, string other)
    {
      for (char c = 'a'; c <= 'g'; c++)
      {
        if (literal.Contains(c) != other.Contains(c)) { return false; }
      }
      return true;
    }

    public static string Add(this string a, string b)
    {
      var newLiteral = new StringBuilder();
      for (char c = 'a'; c <= 'g'; c++)
      {
        if (a.Contains(c) || b.Contains(c)) { newLiteral.Append(c); }
      }
      return newLiteral.ToString();
    }

    public static string Subtract(this string a, string b)
    {
      var newLiteral = new StringBuilder();
      for (char c = 'a'; c <= 'g'; c++)
      {
        if (a.Contains(c) && !b.Contains(c)) { newLiteral.Append(c); }
      }
      return newLiteral.ToString();
    }
  }
}
