using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day18 : Puzzle
  {
    public Puzzle_Day18(int part, bool useTestData = false) : base(18, part, useTestData)
    {
    }

    public override string Solve()
    {
      var items = ReadInput().Select(p => (Pair)Element.Create(p)).ToArray();
      switch (Part)
      {
        case 1:
          var pair = items[0];
          for (int i = 1; i < items.Length; i++)
          {
            pair = pair + items[i];
          }
          return pair.Magnitude.ToString();
        case 2:
          long best = 0;
          for (int i = 0; i < items.Length; i++)
          {
            for (int j = i + 1; j < items.Length; j++)
            {
              var mag = (items[i] + items[j]).Magnitude;
              if (mag > best) { best = mag; }
              mag = (items[j] + items[i]).Magnitude;
              if (mag > best) { best = mag; }
            }
          }
          return best.ToString();
        default:
          throw new ArgumentException("Invalid pair value.");
      }
    }

    public abstract class Element
    {
      public Pair Parent { get; set; }

      public int Nesting => (Parent == null) ? 0 : Parent.Nesting + 1;

      public abstract long Magnitude { get; }

      public static Pair operator +(Element a, Element b)
      {
        var result = new Pair(a, b).Clone() as Pair;
        result.Reduce();
        return result;
      }

      public static Element Create(string line)
      {
        if (line.StartsWith('['))
        {
          line = line.Substring(1, line.Length - 2);
          var level = 0;
          var comma = -1;
          for (int i = 0; i < line.Length; i++)
          {
            switch (line[i])
            {
              case '[':
                level++;
                break;
              case ']':
                level--;
                break;
              case ',':
                if (level == 0) { comma = i; }
                break;
            }
            if (comma != -1) { break; }
          }
          var left = Create(line.Substring(0, comma));
          var right = Create(line.Substring(comma + 1));
          return new Pair(left, right);
        }
        else
        {
          return new Number(Int32.Parse(line));
        }
      }

      public Element Clone()
      {
        return Create(this.ToString());
      }
    }

    public class Number : Element
    {
      public int Value { get; set; }

      public override long Magnitude => Value;

      public Number(int value, Pair parent = null)
      {
        Value = value;
        Parent = parent;
      }

      public void Split()
      {
        var left = (int)Math.Floor(Value / 2.0);
        var right = (int)Math.Ceiling(Value / 2.0);
        var leftNumber = new Number(left);
        var rightNumber = new Number(right);
        var newItem = new Pair(leftNumber, rightNumber, Parent);
        if (Parent.Left == this) { Parent.Left = newItem; }
        else if (Parent.Right == this) { Parent.Right = newItem; }
        else { throw new ApplicationException("I am not a child of my parent."); }
      }

      public override string ToString()
      {
        return Value.ToString();
      }
    }

    public class Pair : Element
    {
      public Element Left { get; set; }
      public Element Right { get; set; }
      public override long Magnitude => (3 * Left.Magnitude) + (2 * Right.Magnitude);
      public Pair(Element left, Element right, Pair parent = null)
      {
        Left = left;
        Right = right;
        Left.Parent = this;
        Right.Parent = this;
        Parent = parent;
      }

      public void Reduce()
      {
        Pair explodee = null;
        Number splitee = null;
        do
        {
          explodee = FindNextToExplode();
          if (explodee != null) { explodee.Explode(); }
          else
          {
            splitee = FindNextToSplit();
            if (splitee != null) { splitee.Split(); }
          }
        } while (explodee != null || splitee != null);
      }

      public Pair FindNextToExplode()
      {
        switch (Nesting)
        {
          case 0:
          case 1:
          case 2:
          case 3:
            var result = (Left as Pair)?.FindNextToExplode();
            if (result == null) { result = (Right as Pair)?.FindNextToExplode(); }
            return result;
          case 4:
            return (Left is Number && Right is Number) ? this : null;
          default:
            return null;
        }
      }

      public Number FindNextToSplit()
      {
        if (Left is Number)
        {
          var left = (Number)Left;
          if (left.Value >= 10) { return left; }
        }
        else
        {
          var result = ((Pair)Left).FindNextToSplit();
          if (result != null) { return result; }
        }
        if (Right is Number)
        {
          var right = (Number)Right;
          if (right.Value >= 10) { return right; }
        }
        else
        {
          var result = ((Pair)Right).FindNextToSplit();
          if (result != null) { return result; }
        }
        return null;
      }

      public void Explode()
      {
        var left = FindClosestNumberToMyLeft();
        if (left != null) { left.Value += ((Number)Left).Value; }
        var right = FindClosestNumberToMyRight();
        if (right != null) { right.Value += ((Number)Right).Value; }

        if (Parent.Left == this) { Parent.Left = new Number(0, Parent); }
        else if (Parent.Right == this) { Parent.Right = new Number(0, Parent); }
        else { throw new ApplicationException("I am not a child of my parent."); }
      }

      public Number FindClosestNumberToMyLeft()
      {
        var pointer = this;

        // Get the first parent in which I am not the left child
        while (pointer != null && pointer.Parent?.Left == pointer) { pointer = pointer.Parent; }
        pointer = pointer?.Parent;
        if (pointer == null) { return null; }

        // Go down one child to the left.
        if (pointer.Left is Number) { return (Number)pointer.Left; }
        pointer = (Pair)pointer.Left;

        // Trace down to the right until we hit a Number
        while (pointer.Right is Pair) { pointer = (Pair)pointer.Right; }
        return (Number)pointer.Right;
      }

      public Number FindClosestNumberToMyRight()
      {
        var pointer = this;

        // Get the first parent in which I am not the left child
        while (pointer != null && pointer.Parent?.Right == pointer) { pointer = pointer.Parent; }
        pointer = pointer?.Parent;
        if (pointer == null) { return null; }

        // Go down one child to the left.
        if (pointer.Right is Number) { return (Number)pointer.Right; }
        pointer = (Pair)pointer.Right;

        // Trace down to the right until we hit a Number
        while (pointer.Left is Pair) { pointer = (Pair)pointer.Left; }
        return (Number)pointer.Left;
      }

      public override string ToString()
      {
        return $"[{Left},{Right}]";
      }
    }
  }
}
