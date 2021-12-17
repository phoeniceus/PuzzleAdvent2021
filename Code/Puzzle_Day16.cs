using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day16 : Puzzle
  {
    public string _testData;

    public Puzzle_Day16(int part, string testData = null) : base(16, part, testData != null)
    {
      _testData = testData;
    }

    public override string Solve()
    {
      var bitmap = new BitMap(_testData ?? ReadInput().First());
      var packet = Parse(bitmap);
      //Console.WriteLine(packet.Format());
      var result = (Part == 1) ? (ulong)packet.VersionSum : packet.PacketValue;
      return result.ToString();
    }

    private Packet Parse(BitMap bitmap)
    {
      var version = bitmap.Read(3);
      var typeId = bitmap.Read(3);
      if (typeId == 4)
      {
        var packet = new LiteralPacket { Version = version, TypeId = typeId };
        bool cont;
        do
        {
          cont = bitmap.Read(1) != 0;
          packet.Value *= 16;
          packet.Value += (ulong)bitmap.Read(4);
        } while (cont);
        return packet;
      }
      else
      {
        var packet = new OperatorPacket { Version = version, TypeId = typeId };
        packet.LengthTypeId = bitmap.Read(1);
        if (packet.LengthTypeId == 0)
        {
          var totalSubPacketsLength = bitmap.Read(15);
          packet.SubPackets = ReadSubPacketsByLength(bitmap, totalSubPacketsLength).ToArray();
        }
        else
        {
          var subPacketCount = bitmap.Read(11);
          packet.SubPackets = ReadSubPacketsByCount(bitmap, subPacketCount).ToArray();
        }
        return packet;
      }
    }

    private IEnumerable<Packet> ReadSubPacketsByLength(BitMap bitmap, int totalSubPacketsLength)
    {
      var length = 0;
      while (length < totalSubPacketsLength)
      {
        var start = bitmap.pointer;
        var packet = Parse(bitmap);
        length += bitmap.pointer - start;
        yield return packet;
      }
      if (length != totalSubPacketsLength) { throw new ApplicationException("Read beyond SubPacket length;"); }
    }

    private IEnumerable<Packet> ReadSubPacketsByCount(BitMap bitmap, int subPacketCount)
    {
      for (int i = 0; i < subPacketCount; i++)
      {
        yield return Parse(bitmap);
      }
    }


    public class BitMap
    {
      protected bool[] _bits; // From left (most significant) to right (least significant) 
      protected int _length;
      public int pointer { get; protected set; }

      public BitMap(string hex)
      {
        var bits = new List<bool>();
        foreach (var ch in hex)
        {
          var b = byte.Parse(ch.ToString(), System.Globalization.NumberStyles.HexNumber);
          bits.Add((b & (1 << 3)) != 0);
          bits.Add((b & (1 << 2)) != 0);
          bits.Add((b & (1 << 1)) != 0);
          bits.Add((b & (1 << 0)) != 0);
        }
        _bits = bits.ToArray();
        _length = _bits.Length;
        pointer = 0;
      }

      public int Read(int length)
      {
        if (pointer + length > _length) { throw new ArgumentOutOfRangeException(nameof(length)); }

        var result = 0;
        for (int i = pointer; i < pointer + length; i++)
        {
          result *= 2;
          if (_bits[i]) { result += 1; }
        }
        pointer += length;
        return result;
      }
    }

    public abstract class Packet
    {
      public int Version { get; set; }
      public int TypeId { get; set; }
      public abstract string Format(int indent = 0);
      public abstract int VersionSum { get; }
      public abstract ulong PacketValue { get; }
      public abstract string TypeAbbr { get; }
    }

    public class LiteralPacket : Packet
    {
      public ulong Value { get; set; }
      public override string Format(int indent = 0)
      {
        return new string(' ', indent) + $"<Literal {Value} {TypeAbbr} {PacketValue}>\r\n";
      }
      public override int VersionSum => Version;
      public override ulong PacketValue => (ulong)Value;
      public override string TypeAbbr => "Lit";
    }

    public class OperatorPacket : Packet
    {
      public int LengthTypeId { get; set; }
      public Packet[] SubPackets { get; set; }
      public override string Format(int indent = 0)
      {
        var sb = new StringBuilder();
        sb.Append(new string(' ', indent));
        sb.AppendLine($"<Operator {LengthTypeId} {TypeAbbr} {PacketValue}>");
        foreach (var packet in SubPackets)
        {
          sb.Append(packet.Format(indent + 2));
        }
        sb.AppendLine();
        return sb.ToString();
      }
      public override int VersionSum => Version + SubPackets.Sum(p => p.VersionSum);
      public override ulong PacketValue
      {
        get
        {
          switch (TypeId)
          {
            case 0:
              return (SubPackets.Length == 1) ? SubPackets[0].PacketValue : SubPackets.Select(x => x.PacketValue).Aggregate((a, b) => a + b);
            case 1:
              return (SubPackets.Length == 1) ? SubPackets[0].PacketValue : SubPackets.Select(x => x.PacketValue).Aggregate((a, b) => a * b);
            case 2:
              return SubPackets.Min(p => p.PacketValue);
            case 3:
              return SubPackets.Max(p => p.PacketValue);
            case 5:
              if (SubPackets.Length != 2) { throw new ApplicationException("Cannot perform greater than. SubPackets length is not 2."); }
              return (SubPackets[0].PacketValue > SubPackets[1].PacketValue) ? 1U : 0;
            case 6:
              if (SubPackets.Length != 2) { throw new ApplicationException("Cannot perform less than. SubPackets length is not 2."); }
              return (SubPackets[0].PacketValue < SubPackets[1].PacketValue) ? 1U : 0;
            case 7:
              if (SubPackets.Length != 2) { throw new ApplicationException("Cannot perform equal to. SubPackets length is not 2."); }
              return (SubPackets[0].PacketValue == SubPackets[1].PacketValue) ? 1U : 0;
            default:
              throw new ApplicationException("Unvalid TypeId");
          }
        }
      }
      public override string TypeAbbr
      {
        get
        {
          switch (TypeId)
          {
            case 0:
              return "Add";
            case 1:
              return "Mul";
            case 2:
              return "Min";
            case 3:
              return "Max";
            case 5:
              return " > ";
            case 6:
              return " < ";
            case 7:
              return " = ";
            default:
              throw new ApplicationException("Unvalid TypeId");
          }
        }
      }
    }
  }
}
