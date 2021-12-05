using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day04 : Puzzle
  {
    public Puzzle_Day04(int part, bool useTestData = false) : base(4, part, useTestData)
    {
    }

    public override string Solve()
    {
      var (numbers, boards) = ReadBingo();
      switch (Part)
      {
        case 1:
          return WinnersInOrder(numbers, boards).First().ToString();
        case 2:
          return WinnersInOrder(numbers, boards).Last().ToString();
        default:
          throw new ArgumentOutOfRangeException(nameof(Part));
      }
    }

    private IEnumerable<int> WinnersInOrder(int[] numbers, BingoBoard[] boards)
    {
      var list = boards.ToList();
      for (int i = 0; i < numbers.Length; i++)
      {
        var number = numbers[i];
        foreach (var board in list)
        {
          board.AnnounceNumber(number);
        }
        var winners = list.Where(b => b.IsWinner).ToList();
        list = list.Where(b => !b.IsWinner).ToList();
        foreach (var winner in winners)
        {
          var result = winner.UnmarkedSum * number;
          yield return result;
        }
      }
    }

    public (int[], BingoBoard[]) ReadBingo()
    {
      int[] numbers = null;
      List<BingoBoard> boards = new List<BingoBoard>(100);

      List<string> buffer = new List<string>(5);
      foreach (var line in this.ReadInput())
      {
        if (line.Trim() == "") { continue; }
        else if (numbers == null)
        {
          numbers = line.Split(',').Select(x => Int32.Parse(x.Trim())).ToArray();
        }
        else
        {
          buffer.Add(line);
          if (buffer.Count == 5)
          {
            var board = new BingoBoard(buffer.ToArray());
            boards.Add(board);
            buffer.Clear();
          }
        }
      }
      return (numbers, boards.ToArray());
    }
  }

  public class BingoBoard
  {
    private static Regex Regex_BingoLine = new Regex(@"^\s*(?<c1>\d+)\s+(?<c2>\d+)\s+(?<c3>\d+)\s+(?<c4>\d+)\s+(?<c5>\d+)\s*$");
    public int[,] _numbers;
    public bool[,] _marked;

    public BingoBoard()
    {
      _numbers = new int[5, 5];
      _marked = new bool[5, 5];
    }

    public BingoBoard(string[] lines) : this()
    {
      if (lines == null) { throw new ArgumentNullException(nameof(lines)); }
      if (lines.Length != 5) { throw new ArgumentOutOfRangeException(nameof(lines)); }
      for (int row = 0; row < 5; row++)
      {
        var match = Regex_BingoLine.Match(lines[row]);
        if (!match.Success) { throw new ArgumentException($"line[{row + 1}] is not valid."); }
        for (int col = 0; col < 5; col++)
        {
          _numbers[col, row] = Int32.Parse(match.Groups[$"c{col + 1}"].Value);
        }
      }
    }

    public void AnnounceNumber(int number)
    {
      for (int row = 0; row < 5; row++)
      {
        for (int col = 0; col < 5; col++)
        {
          if (_numbers[col, row] == number)
          {
            _marked[col, row] = true;
            return;
          }
        }
      }
    }

    private bool IsColWinner(int col)
    {
      return _marked[col, 0] && _marked[col, 1] && _marked[col, 2] && _marked[col, 3] && _marked[col, 4];
    }
    private bool IsRowWinner(int row)
    {
      return _marked[0, row] && _marked[1, row] && _marked[2, row] && _marked[3, row] && _marked[4, row];
    }
    public bool IsWinner =>
      IsColWinner(0) || IsColWinner(1) || IsColWinner(2) || IsColWinner(3) || IsColWinner(4) ||
      IsRowWinner(0) || IsRowWinner(1) || IsRowWinner(2) || IsRowWinner(3) || IsRowWinner(4);

    public int UnmarkedSum
    {
      get
      {
        int result = 0;
        for (int row = 0; row < 5; row++)
        {
          for (int col = 0; col < 5; col++)
          {
            if (!_marked[col, row])
            {
              result += _numbers[col, row];
            }
          }
        }
        return result;
      }
    }
  }
}
