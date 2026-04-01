public struct LeagueRecord
{
    public readonly int Win;
    public readonly int Lose;
    public readonly int Score;

    public LeagueRecord(int win = 0, int lose = 0, int score = 0)
    {
        Win = win;
        Lose = lose;
        Score = score;
    }

    public LeagueRecord ApplyMatchResult(int myWins, int opponentWins)
    {
        int newWin = Win;
        int newLose = Lose;

        if (myWins > opponentWins) newWin++;
        else if (myWins < opponentWins) newLose++;

        int newScore = Score + (myWins - opponentWins);

        return new LeagueRecord(newWin, newLose, newScore);
    }
}