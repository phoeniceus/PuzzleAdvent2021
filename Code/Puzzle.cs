using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace PuzzleAdvent2021
{
  public abstract class Puzzle
  {
    private string _dataPath;

    public int Day { get; private set; }
    public int Part { get; private set; }

    public Puzzle(int day, int part, bool useTestData = false)
    {
      Day = day;
      Part = part;
      _dataPath = $"data\\{(useTestData ? "Test" : "Input")}{Day}.txt";
    }

    public abstract string Solve();
    
    public IEnumerable<string> ReadInput()
    {
      using (var sr = new StreamReader(_dataPath))
      {
        var line = "";
        while ((line = sr.ReadLine()) != null)
        {
          yield return line;
        }
      }
    }
  }
}
