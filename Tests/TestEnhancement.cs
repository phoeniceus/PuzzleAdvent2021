using PuzzleAdvent2021;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PuzzleAdvent2021Test
{
  public class TestEnhancement
  {
    [Theory]
    [InlineData(2,2,'#')]
    public void ConstructorValid(int col, int row, char result)
    {
      var path = $"data\\Test20.txt";
      var lines = System.IO.File.ReadAllLines(path);
      var enhancer = Puzzle_Day20.Enhancer.Read(lines[0]);
      var image = new Puzzle_Day20.Image(lines.Skip(2).ToArray());
      Assert.Equal(result, image.Enhance(enhancer, col, row));
    }
  }
}
