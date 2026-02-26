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