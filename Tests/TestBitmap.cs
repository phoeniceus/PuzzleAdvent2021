using PuzzleAdvent2021;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PuzzleAdvent2021Test
{
  public class TestBitmap
  {
    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("11111", 31)]
    [InlineData("01111", 15)]
    [InlineData("10000", 16)]
    [InlineData("1100100100101", 1 + 4 + 32 + 256 + 2048 + 4096)]
    public void ConstructorValid(string line, ulong value)
    {
      var bitmap = Bitmap.Create(line);
      Assert.Equal(value, bitmap.Value);
      Assert.Equal(line.Length, bitmap.Length);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("2")]
    [InlineData("-1")]
    [InlineData(" 010")]
    [InlineData("010 ")]
    [InlineData("01 0")]
    [InlineData("ABC")]
    public void ConstructorInvalid(string line)
    {
      Assert.ThrowsAny<ArgumentException>(() => Bitmap.Create(line));
    }


    [Theory]
    [InlineData("10010110", 0, false)]
    [InlineData("10010110", 1, true)]
    [InlineData("10010110", 2, true)]
    [InlineData("10010110", 3, false)]
    [InlineData("10010110", 4, true)]
    [InlineData("10010110", 5, false)]
    [InlineData("10010110", 6, false)]
    [InlineData("10010110", 7, true)]
    public void GetBit(string line, int bitIndex, bool value)
    {
      var bitmap = Bitmap.Create(line);
      Assert.Equal(value, bitmap.GetBit(bitIndex));
    }

    [Theory]
    [InlineData("10010110", 0, false, 150)]
    [InlineData("10010110", 0, true, 151)]
    [InlineData("10010110", 1, false, 148)]
    [InlineData("10010110", 6, true, 214)]
    public void SetBit(string line, int bitIndex, bool newBit, ulong newValue)
    {
      var bitmap = Bitmap.Create(line);
      for (int i = 0; i < 8; i++)
      {
        bitmap.SetBit(bitIndex, newBit);
        Assert.Equal(newValue, bitmap.Value);
      }
    }
  }
}
