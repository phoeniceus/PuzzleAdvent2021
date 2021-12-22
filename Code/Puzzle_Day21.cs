using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuzzleAdvent2021
{
  public class Puzzle_Day21 : Puzzle
  {
    public Puzzle_Day21(int part, bool useTestData = false) : base(21, part, useTestData)
    {
    }

    public override string Solve()
    {
      var players = ReadInput().Select(line => new Player(line)).ToArray();
      var game = new Game(players[0], players[1], 1);
      switch (Part)
      {
        case 1:
          Game.Goal = 1000;
          var die = new Die(Part == 1 ? 100 : 3);
          while (!game.GameOver)
          {
            var rolls = die.Rollx3();
            game = game.Play1(rolls);
          }
          var solution = die.RollCount * game.LoserScore;
          return solution.ToString();
        case 2:
          Game.Goal = 21;
          var (count1, count2) = game.Play2();
          return Math.Max(count1, count2).ToString();
        default:
          throw new ApplicationException("Unsuppport Part value.");
      }
    }


    public class Game
    {
      public static long Count { get; set; }
      public static int Goal { get; set; }
      public static int[] _direcRolls;
      public Player Player1 { get; private set; }
      public Player Player2 { get; private set; }
      public int NextPlayer { get; private set; }
      public bool GameOver => Player1.Score >= Goal || Player2.Score >= Goal;
      public int Winner => Player1.Score >= Goal ? 1 : (Player2.Score >= Goal ? 2 : 0);
      static Game()
      {
        _direcRolls = Die2.GetRollsAndFrequency();
      }

      public Game(Player player1, Player player2, int nextPlayer)
      {
        Player1 = player1;
        Player2 = player2;
        NextPlayer = nextPlayer;
      }

      public Game Play1(int rolls)
      {
        if (NextPlayer == 1)
        {
          return new Game(Player1.AddToScore(rolls), Player2, 2);
        }
        else
        {
          return new Game(Player1, Player2.AddToScore(rolls), 1);
        }
      }

      public int LoserScore => (Player1.Score < Player2.Score) ? Player1.Score : Player2.Score;

      public (long CountOfPlayer1Wins, long CountOfPlayer2Wins) Play2()
      {
        if (Player1.Score >= Goal)
        {
          Count++;
          return (1, 0);
        }
        else if (Player2.Score >= Goal)
        {
          Count++;
          return (0, 1);
        }
        else
        {
          long count1 = 0;
          long count2 = 0;
          for (int i = 3; i <= 9; i++)
          {
            var newGame = Play1(i);
            var (c1, c2) = newGame.Play2();
            count1 += c1 * _direcRolls[i];
            count2 += c2 * _direcRolls[i];
          }
          return (count1, count2);
        }
      }
    }

    public struct Player
    {
      public int Score { get; private set; }
      public int Position { get; private set; }

      public Player(string line)
      {
        var regex = new Regex(@"^Player\s(?<player>\d+)\sstarting\sposition\:\s(?<start>\d+)$");
        var match = regex.Match(line);
        if (!match.Success) { throw new ArgumentException("line is not correct format."); }
        Score = 0;
        Position = Int32.Parse(match.Groups["start"].Value);
      }

      public Player(int score, int position) { Score = score; Position = position; }

      public Player AddToScore(int rolls)
      {
        var position = (((Position - 1) + rolls) % 10) + 1;
        var score = Score + position;
        var result = new Player(score, position);
        return result;
      }
    }

    public class Die
    {
      private int _sides;
      public Die(int sides) { _sides = sides; }
      public int RollCount { get; private set; }


      public int Rollx3() => Roll() + Roll() + Roll();
      public int Roll() => (RollCount++ % _sides) + 1;
    }

    public static class Die2
    {
      public static int[] GetRollsAndFrequency()
      {
        var result = new int[10];
        for (int i = 1; i <= 3; i++)
        {
          for (int j = 1; j <= 3; j++)
          {
            for (int k = 1; k <= 3; k++)
            {
              var rolls = i + j + k;
              result[rolls]++;
            }
          }
        }
        return result;
      }
    }
  }
}
