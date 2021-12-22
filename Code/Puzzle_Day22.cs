using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day22 : Puzzle
  {
    public Puzzle_Day22(int part, bool useTestData = false) : base(22, part, useTestData)
    {
      /*
       * This puzzle presented memory and CPU challenges.
       * 
       * It is not possible to store cube values in arrays because there are trillions of cubes
       * and C# cannot store an array with more than 2 billion elements.
       * 
       * It is not even possible to run each cube through all the steps, counting as we go, because 
       * it would take hours to run.
       * 
       * The solution is to model cuboids as ranges of x, y, and z values (6 ints) and to 
       * treat each step as a set of cuboids covering the cubes that were turned on by this
       * step and by no later step. Then it is just a matter of erasing later step cuboid
       * from earlier step cuboids.
       * 
       * After all steps are processed, each individual step is reduced to a small number of 
       * cuboids (6 tuples of integers) where it was the last step to turn those cubes on.
       * Then we can very easily count them.
       */
    }

    public override string Solve()
    {
      var limit = (Part == 1) ? new Cuboid(-50, 50, -50, 50, -50, 50) : null;
      var steps = ReadInput().Select(x => new Step(x)).ToArray();
      var reactor = new ReactorCalculator();

      for (int i = 0; i < steps.Length; i++)
      {
        reactor.ApplyRebootStep(steps[i]);
      }
      return reactor.Count(limit).ToString();
    }

    public class Step
    {
      public Cuboid Cuboid { get; private set; }
      public List<Cuboid> LastToggledOnByMe { get; private set; }
      public bool Toggle { get; private set; }

      public Step(string line)
      {
        var regex = new Regex(@"^(?<toggle>on|off)\sx=(?<x1>\-?\d+)\.\.(?<x2>\-?\d+)\,y=(?<y1>\-?\d+)\.\.(?<y2>\-?\d+)\,z=(?<z1>\-?\d+)\.\.(?<z2>\-?\d+)$");
        var match = regex.Match(line);
        if (!match.Success) { throw new ArgumentException("line is not correct format."); }
        var toggle = match.Groups["toggle"].Value == "on";
        var x1 = Int32.Parse(match.Groups["x1"].Value);
        var x2 = Int32.Parse(match.Groups["x2"].Value);
        var y1 = Int32.Parse(match.Groups["y1"].Value);
        var y2 = Int32.Parse(match.Groups["y2"].Value);
        var z1 = Int32.Parse(match.Groups["z1"].Value);
        var z2 = Int32.Parse(match.Groups["z2"].Value);
        Cuboid = new Cuboid(x1, x2, y1, y2, z1, z2);
        LastToggledOnByMe = new List<Cuboid>();
        if (toggle) { LastToggledOnByMe.Add(Cuboid.Clone()); }
        Toggle = toggle;
      }

      public void Erase(Step later)
      {
        LastToggledOnByMe = LastToggledOnByMe.SelectMany(x => x - later.Cuboid).ToList();
      }
    }

    public class Cuboid
    {
      public int X1 { get; private set; }
      public int Y1 { get; private set; }
      public int Z1 { get; private set; }
      public int X2 { get; private set; }
      public int Y2 { get; private set; }
      public int Z2 { get; private set; }
      public int Width => X2 - X1 + 1;
      public int Height => Y2 - Y1 + 1;
      public int Depth => Z2 - Z1 + 1;

      public Cuboid(int x1, int x2, int y1, int y2, int z1, int z2)
      {
        if (x1 > x2) { throw new ArgumentOutOfRangeException(nameof(x1)); }
        if (y1 > y2) { throw new ArgumentOutOfRangeException(nameof(y1)); }
        if (z1 > z2) { throw new ArgumentOutOfRangeException(nameof(z1)); }
        X1 = x1;
        Y1 = y1;
        Z1 = z1;
        X2 = x2;
        Y2 = y2;
        Z2 = z2;
      }

      public Cuboid Clone()
      {
        return new Cuboid(X1, X2, Y1, Y2, Z1, Z2);
      }

      public static Cuboid operator &(Cuboid a, Cuboid b)
      {
        if (a == null) { return null; }
        if (b == null) { return null; }
        var x1 = Math.Max(a.X1, b.X1);
        var x2 = Math.Min(a.X2, b.X2);
        var y1 = Math.Max(a.Y1, b.Y1);
        var y2 = Math.Min(a.Y2, b.Y2);
        var z1 = Math.Max(a.Z1, b.Z1);
        var z2 = Math.Min(a.Z2, b.Z2);
        if (x1 > x2) { return null; }
        if (y1 > y2) { return null; }
        if (z1 > z2) { return null; }
        return new Cuboid(x1, x2, y1, y2, z1, z2);
      }

      public static Cuboid operator |(Cuboid a, Cuboid b)
      {
        if (a == null) { return b; }
        if (b == null) { return a; }
        var x1 = Math.Min(a.X1, b.X1);
        var x2 = Math.Max(a.X2, b.X2);
        var y1 = Math.Min(a.Y1, b.Y1);
        var y2 = Math.Max(a.Y2, b.Y2);
        var z1 = Math.Min(a.Z1, b.Z1);
        var z2 = Math.Max(a.Z2, b.Z2);
        return new Cuboid(x1, x2, y1, y2, z1, z2);
      }

      // Return a set of Cuboids that covers all cubes in a not in b.
      public static IEnumerable<Cuboid> operator -(Cuboid a, Cuboid b)
      {
        if (a == null) { yield return null; yield break; }
        if (b == null) { yield return a; yield break; }

        var c = new Cuboid(a.X1, a.X2, a.Y1, a.Y2, a.Z1, a.Z2);
        var d = a & b;
        if (d == null) { yield return a; yield break; }

        if (c.X1 < d.X1)
        {
          yield return new Cuboid(c.X1, d.X1 - 1, c.Y1, c.Y2, c.Z1, c.Z2);
          c = new Cuboid(d.X1, c.X2, c.Y1, c.Y2, c.Z1, c.Z2);
        }
        if (d.X2 < c.X2)
        {
          yield return new Cuboid(d.X2 + 1, c.X2, c.Y1, c.Y2, c.Z1, c.Z2);
          c = new Cuboid(c.X1, d.X2, c.Y1, c.Y2, c.Z1, c.Z2);
        }
        if (c.Y1 < d.Y1)
        {
          yield return new Cuboid(c.X1, c.X2, c.Y1, d.Y1 - 1, c.Z1, c.Z2);
          c = new Cuboid(c.X1, c.X2, d.Y1, c.Y2, c.Z1, c.Z2);
        }
        if (d.Y2 < c.Y2)
        {
          yield return new Cuboid(c.X1, c.X2, d.Y2 + 1, c.Y2, c.Z1, c.Z2);
          c = new Cuboid(c.X1, c.X2, c.Y1, d.Y2, c.Z1, c.Z2);
        }
        if (c.Z1 < d.Z1)
        {
          yield return new Cuboid(c.X1, c.X2, c.Y1, c.Y2, c.Z1, d.Z1 - 1);
          c = new Cuboid(c.X1, c.X2, c.Y1, c.Y2, d.Z1, c.Z2);
        }
        if (d.Z2 < c.Z2)
        {
          yield return new Cuboid(c.X1, c.X2, c.Y1, c.Y2, d.Z2 + 1, c.Z2);
          //c = new Cuboid(c.X1, c.X2, c.Y1, c.Y2, c.Z1, d.Z2);
        }
      }

      public long Count => (long)Width * (long)Height * (long)Depth;
    }

    public class ReactorCalculator
    {
      public List<Step> _steps = new List<Step>();

      public void ApplyRebootStep(Step step)
      {
        foreach (var prev in _steps) { prev.Erase(step); }
        _steps.Add(step);
      }

      public long Count(Cuboid limit = null)
      {
        var on = _steps.SelectMany(s => s.LastToggledOnByMe);
        if (limit != null) { on = on.Select(x => x & limit).Where(x => x != null); }
        return on.Sum(x => x.Count);
      }
    }
  }
}
