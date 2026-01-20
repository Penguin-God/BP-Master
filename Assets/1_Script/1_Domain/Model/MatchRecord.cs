
public class MatchRecord
{
    public int BlueWins { get; private set; }
    public int RedWins { get; private set; }
    readonly int TargetWins;

    public MatchRecord(int targetWins = 2) => TargetWins = targetWins;

    public void AddWin(Team winner)
    {
        if (winner == Team.Blue) BlueWins++;
        else if (winner == Team.Red) RedWins++;
    }

    public bool IsMatchFinished => BlueWins >= TargetWins || RedWins >= TargetWins;
    public Team MatchWinner => BlueWins >= TargetWins ? Team.Blue : Team.Red;
}