using System.Collections.Generic;
using System.Linq;


public class MatchWinCounter
{
    public int TargetWins { get; }
    readonly Dictionary<int, int> winCountById = new();

    public MatchWinCounter(MatchData matchData, int targetWins)
    {
        TargetWins = targetWins;
        this.winCountById.Add(matchData.Id1, 0);
        this.winCountById.Add(matchData.Id2, 0);
    }

    public void AddWin(int id) => winCountById[id]++;
    public int GetWin(int id) => winCountById[id];
    public bool IsMatchFinished => winCountById.Values.Any(x => x >= TargetWins);
    public int TotalWins => winCountById.Values.Sum();
}