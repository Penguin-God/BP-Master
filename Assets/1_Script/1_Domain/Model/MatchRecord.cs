using System.Collections.Generic;
using System.Linq;

public enum Participant { Player, AI }

public class MatchRecord
{
    public int PlayerWinCount { get; private set; }
    public int AiWinCount { get; private set; }
    public int TargetWins { get; }

    public MatchRecord(int targetWins = 2) => TargetWins = targetWins;

    public void AddWin(Participant winner)
    {
        if (winner == Participant.Player) PlayerWinCount++;
        else if (winner == Participant.AI) AiWinCount++;
    }

    public bool IsMatchFinished => PlayerWinCount >= TargetWins || AiWinCount >= TargetWins;
    public Participant MatchWinner => PlayerWinCount >= TargetWins ? Participant.Player : Participant.AI;
}

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