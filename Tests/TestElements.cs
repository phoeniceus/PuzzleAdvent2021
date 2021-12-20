using PuzzleAdvent2021;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PuzzleAdvent2021Test
{
  public class TestElements
  {
    [Fact]
    public void NumberConstructorWorks()
    {
      var number = new Puzzle_Day18.Number(5);
      Assert.Equal(5, number.Value);
    }

    [Fact]
    public void PairConstructorWorks()
    {
      var left = new Puzzle_Day18.Number(1);
      var right = new Puzzle_Day18.Number(2);
      var pair = new Puzzle_Day18.Pair(left, right);
      Assert.Equal(left, pair.Left);
      Assert.Equal(right, pair.Right);
      Assert.Equal(pair, pair.Left.Parent);
      Assert.Equal(pair, pair.Right.Parent);
    }

    [Fact]
    public void ElementCreateWorks()
    {
      var literal = "[[1,9],[8,5]]";
      var element = Puzzle_Day18.Element.Create(literal);
      Assert.Equal(literal, element.ToString());
    }

    [Theory]
    [InlineData("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
    [InlineData("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
    [InlineData("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
    [InlineData("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]")]
    [InlineData("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
    public void ExplodeWorks(string before, string after)
    {
      var pair = (Puzzle_Day18.Pair)Puzzle_Day18.Element.Create(before);
      var child = pair.FindNextToExplode();
      Assert.NotNull(child);
      child.Explode();
      Assert.Equal(after, pair.ToString());
    }

    [Theory]
    [InlineData("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
    public void ReduceWorks(string before, string after)
    {
      var pair = (Puzzle_Day18.Pair)Puzzle_Day18.Element.Create(before);
      pair.Reduce();
      Assert.Equal(after, pair.ToString());
    }

    [Theory]
    [InlineData("[1,1]", "[2,2]", "[3,3]", "[4,4]", "[[[[1,1],[2,2]],[3,3]],[4,4]]")]
    [InlineData("[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]", "[[[[3,0],[5,3]],[4,4]],[5,5]]")]
    [InlineData("[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]", "[6,6]", "[[[[5,0],[7,4]],[5,5]],[6,6]]")]
    [InlineData("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]", "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]", "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]", "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]", "[7,[5,[[3,8],[1,4]]]]", "[[2,[2,2]],[8,[8,1]]]", "[2,9]", "[1,[[[9,3],9],[[9,0],[0,7]]]]", "[[[5,[7,4]],7],1]", "[[[[4,2],2],6],[8,7]]", "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]")]
    public void AddReduceWorks(params string[] items)
    {
      var pair = (Puzzle_Day18.Pair)Puzzle_Day18.Element.Create(items[0]);
      for (int i = 1; i < items.Length - 1; i++)
      {
        pair += (Puzzle_Day18.Pair)Puzzle_Day18.Element.Create(items[i]);
      }
      pair.Reduce();
      Assert.Equal(items[items.Length - 1], pair.ToString());
    }

  }
}
