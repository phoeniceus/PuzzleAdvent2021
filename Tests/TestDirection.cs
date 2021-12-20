using PuzzleAdvent2021;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PuzzleAdvent2021Test
{
  public class TestDirection
  {
    [Theory]
    [InlineData("-1,-1,1", "1,-1,1")]
    [InlineData("-2,-2,2", "2,-2,2")]
    [InlineData("-3,-3,3", "3,-3,3")]
    [InlineData("-2,-3,1", "2,-1,3")]
    [InlineData("5,6,-4", "-5,4,-6")]
    [InlineData("8,0,7", "-8,-7,0")]
    public void FacingNegXUpNegZ(string before, string after)
    {
      var s = new Puzzle_Day19.Scanner();
      var p = new Puzzle_Day19.Position(before);
      var o = new Puzzle_Day19.Orientation(new Puzzle_Day19.Direction(Puzzle_Day19.Dimension.x, false), new Puzzle_Day19.Direction(Puzzle_Day19.Dimension.z, false));
      p = s.Convert(p, o);
      Assert.Equal(after, p.ToString());
    }

    [Theory]
    [InlineData("-1,-1,1", "-1,-1,-1")]
    [InlineData("-2,-2,2", "-2,-2,-2")]
    [InlineData("-3,-3,3", "-3,-3,-3")]
    [InlineData("-2,-3,1", "-1,-3,-2")]
    [InlineData("5,6,-4", "4,6,5")]
    [InlineData("8,0,7", "-7,0,8")]
    public void FacingNegZUpPosY(string before, string after)
    {
      var s = new Puzzle_Day19.Scanner();
      var p = new Puzzle_Day19.Position(before);
      var o = new Puzzle_Day19.Orientation(new Puzzle_Day19.Direction(Puzzle_Day19.Dimension.z, false), new Puzzle_Day19.Direction(Puzzle_Day19.Dimension.y, true));
      p = s.Convert(p, o);
      Assert.Equal(after, p.ToString());
    }

    [Theory]
    [InlineData("-1,-1,1", "1,1,-1")]
    [InlineData("-2,-2,2", "2,2,-2")]
    [InlineData("-3,-3,3", "3,3,-3")]
    [InlineData("-2,-3,1", "1,3,-2")]
    [InlineData("5,6,-4", "-4,-6,5")]
    [InlineData("8,0,7", "7,0,8")]
    public void FacingPosZUpNegY(string before, string after)
    {
      var s = new Puzzle_Day19.Scanner();
      var p = new Puzzle_Day19.Position(before);
      var o = new Puzzle_Day19.Orientation(new Puzzle_Day19.Direction(Puzzle_Day19.Dimension.z, true), new Puzzle_Day19.Direction(Puzzle_Day19.Dimension.y, false));
      p = s.Convert(p, o);
      Assert.Equal(after, p.ToString());
    }

    [Theory]
    [InlineData("-1,-1,1", "1,1,1")]
    [InlineData("-2,-2,2", "2,2,2")]
    [InlineData("-3,-3,3", "3,3,3")]
    [InlineData("-2,-3,1", "3,1,2")]
    [InlineData("5,6,-4", "-6,-4,-5")]
    [InlineData("8,0,7", "0,7,-8")]
    public void FacingNegYUpPosZ(string before, string after)
    {
      var s = new Puzzle_Day19.Scanner();
      var p = new Puzzle_Day19.Position(before);
      var o = new Puzzle_Day19.Orientation(new Puzzle_Day19.Direction(Puzzle_Day19.Dimension.y, false), new Puzzle_Day19.Direction(Puzzle_Day19.Dimension.z, true));
      p = s.Convert(p, o);
      Assert.Equal(after, p.ToString());
    }

    [Fact]
    public void Match()
    {
      var path = $"data\\Test19.txt";
      var lines = System.IO.File.ReadAllLines(path);
      var scanners = Puzzle_Day19.ReadScanners(lines).ToArray();
      Assert.Equal(5, scanners.Length);

      var converted = scanners[0].RotateOtherToMatchMe(scanners[1]);
      Assert.NotNull(converted);
      Assert.Contains(new Puzzle_Day19.Position("-618,-824,-621"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("-537,-823,-458"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("-447,-329,318"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("404,-588,-901"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("544,-627,-890"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("528,-643,409"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("-661,-816,-575"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("390,-675,-793"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("423,-701,434"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("-345,-311,381"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("459,-707,401"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("-485,-357,347"), converted.Beacons);
      Assert.Equal(new Puzzle_Day19.Position("68,-1246,-43"), converted.Position);

      converted = converted.RotateOtherToMatchMe(scanners[4]);
      Assert.NotNull(converted);
      Assert.Contains(new Puzzle_Day19.Position("459,-707,401"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("-739,-1745,668"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("-485,-357,347"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("432,-2009,850"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("528,-643,409"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("423,-701,434"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("-345,-311,381"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("408,-1815,803"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("534,-1912,768"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("-687,-1600,576"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("-447,-329,318"), converted.Beacons);
      Assert.Contains(new Puzzle_Day19.Position("-635,-1737,486"), converted.Beacons);
      Assert.Equal(new Puzzle_Day19.Position("-20,-1133,1061"), converted.Position);
    }
  }
}
