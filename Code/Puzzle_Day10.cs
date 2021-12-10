using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day10 : Puzzle
  {
    public Puzzle_Day10(int part, bool useTestData = false) : base(10, part, useTestData)
    {
    }

    public override string Solve()
    {
      switch (Part)
      {
        case 1:
          var result1 = ReadLines().Sum(x => x.FindPointsForFirstCorruptedCharacter());
          return result1.ToString();
        case 2:
          var result2 = ReadLines().Select(x => x.FindPointsForAutoComplete()).Where(x => x != 0).OrderBy(x => x).ToArray();
          return result2[(result2.Length - 1) / 2].ToString();
        default:
          throw new ApplicationException("Part value not supported.");
      }
    }

    private IEnumerable<NavigationLine> ReadLines()
    {
      return ReadInput().Select(x => new NavigationLine(x));
    }

    public class NavigationLine
    {
      public string Text { get; private set; }
      public NavigationLine(string text)
      {
        Text = text;
      }

      public int FindPointsForFirstCorruptedCharacter()
      {
        var stack = new Stack<char>();
        for (int i = 0; i < Text.Length; i++)
        {
          switch (Text[i])
          {
            case '(':
            case '[':
            case '{':
            case '<':
              stack.Push(Text[i]);
              break;
            case ')':
              if (stack.Pop() != '(') { return 3; }
              break;
            case ']':
              if (stack.Pop() != '[') { return 57; }
              break;
            case '}':
              if (stack.Pop() != '{') { return 1197; }
              break;
            case '>':
              if (stack.Pop() != '<') { return 25137; }
              break;
            default:
              throw new ApplicationException("Illegal character in data.");
          }
        }
        return 0;
      }

      public long FindPointsForAutoComplete()
      {
        var stack = new Stack<char>();
        for (int i = 0; i < Text.Length; i++)
        {
          switch (Text[i])
          {
            case '(':
            case '[':
            case '{':
            case '<':
              stack.Push(Text[i]);
              break;
            case ')':
              if (stack.Pop() != '(') { return 0; }
              break;
            case ']':
              if (stack.Pop() != '[') { return 0; }
              break;
            case '}':
              if (stack.Pop() != '{') { return 0; }
              break;
            case '>':
              if (stack.Pop() != '<') { return 0; }
              break;
            default:
              throw new ApplicationException("Illegal character in data.");
          }
        }
        return ScoreStack(stack);
      }

      private long ScoreStack(Stack<char> stack)
      {
        long score = 0;
        while (stack.Count > 0)
        {
          switch (stack.Pop())
          {
            case '(':
              score = (score * 5) + 1;
              break;
            case '[':
              score = (score * 5) + 2;
              break;
            case '{':
              score = (score * 5) + 3;
              break;
            case '<':
              score = (score * 5) + 4;
              break;
            default:
              throw new ApplicationException("Illegal character in data.");
          }
        }
        return score;
      }
    }
  }
}
