public enum Participant { Player, AI }

public class MatchRecord
{
    public int PlayerWins { get; private set; }
    public int AiWins { get; private set; }
    public int TargetWins { get; }

    public MatchRecord(int targetWins = 2) => TargetWins = targetWins;

    public void AddWin(Participant winner)
    {
        if (winner == Participant.Player) PlayerWins++;
        else if (winner == Participant.AI) AiWins++;
    }

    public bool IsMatchFinished => PlayerWins >= TargetWins || AiWins >= TargetWins;
    public Participant MatchWinner => PlayerWins >= TargetWins ? Participant.Player : Participant.AI;
}