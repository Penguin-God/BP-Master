public class BonusDeltaCalculator
{
    private readonly TeamBonusCalculator _teamBonusCalculator;

    public BonusDeltaCalculator(TeamBonusCalculator teamBonusCalculator)
    {
        _teamBonusCalculator = teamBonusCalculator;
    }

    public int Calculate(GameScoreInfo before, GameScoreInfo after, Team myTeam)
    {
        int myBeforeBonus = CalculateBonus(before, myTeam);
        int enemyBeforeBonus = CalculateBonus(before, GetEnemyTeam(myTeam));

        int myAfterBonus = CalculateBonus(after, myTeam);
        int enemyAfterBonus = CalculateBonus(after, GetEnemyTeam(myTeam));

        int myGain = myAfterBonus - myBeforeBonus;
        int enemyLoss = enemyBeforeBonus - enemyAfterBonus;

        return myGain + enemyLoss;
    }

    int CalculateBonus(GameScoreInfo gameScore, Team team)
    {
        ScoreInfo teamScore = (team == Team.Blue) ? gameScore.Blue : gameScore.Red;
        return _teamBonusCalculator.CalculateTeamBonus(teamScore);
    }

    private Team GetEnemyTeam(Team team) => (team == Team.Blue) ? Team.Red : Team.Blue;
}