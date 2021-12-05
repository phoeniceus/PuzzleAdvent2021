using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day03 : Puzzle
  {
    public Puzzle_Day03(int part, bool useTestData = false) : base(3, part, useTestData)
    {
    }

    public override string Solve()
    {
      var report = new DiagnosticReport(ReadBitMaps());
      switch (Part)
      {
        case 1:
          var gamma = report.Gamma;
          var epsilon = gamma.Inverse;
          var result = gamma.Value * epsilon.Value;
          return result.ToString();
        case 2:
          return report.LifeSupportRating.ToString();
        default:
          throw new ArgumentOutOfRangeException(nameof(Part));
      }
    }

    public IEnumerable<Bitmap> ReadBitMaps()
    {
      foreach (var line in this.ReadInput())
      {
        yield return Bitmap.Create(line);
      }
    }
  }

  public struct Bitmap
  {
    private uint _bits;

    public int Length { get; }

    public uint Value => _bits << (32 - Length) >> (32 - Length);

    public Bitmap(int length)
    {
      if (length >= 32) { throw new ArgumentOutOfRangeException(nameof(length)); }
      _bits = 0;
      Length = length;
    }

    public void SetBit(int bitIndex, bool value)
    {
      _bits = (_bits & ~(1u << bitIndex)) | ((value ? 1u : 0u) << bitIndex);
    }

    public bool GetBit(int bitIndex) => (_bits & (1u << bitIndex)) != 0;

    public Bitmap Inverse
    {
      get
      {
        var result = new Bitmap(Length);
        result._bits = ~_bits;
        return result;
      }
    }

    public static Bitmap Create(string line)
    {
      if (line == null) { throw new ArgumentNullException(nameof(line)); }

      var result = new Bitmap(line.Length);
      var bitIndex = 0;
      foreach (var ch in line.Reverse())
      {
        switch (ch)
        {
          case '0':
            result.SetBit(bitIndex, false);
            break;
          case '1':
            result.SetBit(bitIndex, true);
            break;
          default:
            throw new ArgumentException(nameof(line));
        }
        bitIndex++;
      }
      return result;
    }

    public override string ToString()
    {
      return Convert.ToString(Value, 2);
    }
  }

  public class DiagnosticReport
  {
    public Bitmap[] Data { get; }
    private int _bitmapLength;

    public DiagnosticReport(IEnumerable<Bitmap> data)
    {
      if (data == null) { throw new ArgumentNullException(nameof(data)); }
      Data = data.ToArray();
      _bitmapLength = (Data.Length > 0) ? Data[0].Length : 0;
    }

    public Bitmap Gamma
    {
      get
      {
        if (Data.Length == 0) { return new Bitmap(); }
        var result = new Bitmap(Data[0].Length);
        for (int i = 0; i < result.Length; i++)
        {
          result.SetBit(i, MoreCommonBitValue(Data, i, true));
        }
        return result;
      }
    }

    public Bitmap Epsilon
    {
      get
      {
        if (Data.Length == 0) { return new Bitmap(); }
        var result = new Bitmap(Data[0].Length);
        for (int i = 0; i < result.Length; i++)
        {
          result.SetBit(i, LessCommonBitValue(Data, i, false));
        }
        return result;
      }
    }

    private bool MoreCommonBitValue(IEnumerable<Bitmap> inputs, int bitIndex, bool tie)
    {
      var count0 = 0;
      var count1 = 0;
      foreach (var input in inputs)
      {
        if (input.GetBit(bitIndex)) { count1++; } else { count0++; }
      }
      return (count1 > count0) ? true : ((count1 < count0) ? false : tie);
    }

    private bool LessCommonBitValue(IEnumerable<Bitmap> inputs, int bitIndex, bool tie)
    {
      var count0 = 0;
      var count1 = 0;
      foreach (var input in inputs)
      {
        if (input.GetBit(bitIndex)) { count1++; } else { count0++; }
      }
      return (count1 < count0) ? true : ((count1 > count0) ? false : tie);
    }

    private IEnumerable<Bitmap> Filter(IEnumerable<Bitmap> inputs, int bitIndex, bool match)
    {
      foreach (var input in inputs)
      {
        if (input.GetBit(bitIndex) == match) { yield return input; }
      }
    }

    private Bitmap OxygenGeneratingRating
    {
      get
      {
        if (Data.Length == 0) { throw new ApplicationException("No Data."); }

        var list = Data;
        var position = 1;
        while (list.Length > 1 && position <= _bitmapLength)
        {
          var bitIndex = _bitmapLength - position;
          var moreCommon = MoreCommonBitValue(list, bitIndex, true);
          list = Filter(list, bitIndex, moreCommon).ToArray();
          position++;
        }
        if (list.Length == 1) { return list[0]; }
        throw new ApplicationException("Data must contain duplicate entries.");
      }
    }

    private Bitmap CO2ScrubberRating
    {
      get
      {
        if (Data.Length == 0) { throw new ApplicationException("No Data."); }

        var list = Data;
        var position = 1;
        while (list.Length > 1 && position <= _bitmapLength)
        {
          var bitIndex = _bitmapLength - position;
          var moreCommon = LessCommonBitValue(list, bitIndex, false);
          list = Filter(list, bitIndex, moreCommon).ToArray();
          position++;
        }
        if (list.Length == 1) { return list[0]; }
        throw new ApplicationException("Data must contain duplicate entries.");
      }
    }

    public uint LifeSupportRating => OxygenGeneratingRating.Value * CO2ScrubberRating.Value;
  }
}
