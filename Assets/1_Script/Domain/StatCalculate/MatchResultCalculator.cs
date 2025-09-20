using System.Collections.Generic;

public readonly struct TeamScoreInfo
{
    public int AttackTotal { get; }
    public int DefenseTotal { get; }
    public int DefaultScore => AttackTotal + DefenseTotal;

    public int AttackBonus { get; }
    public int DefenseBonus { get; }
    public int SpeedBonus { get; }
    public int BonusScore => AttackBonus + DefenseBonus + SpeedBonus;

    public int Total => DefaultScore + BonusScore;

    public TeamScoreInfo(int attackTotal, int defenseTotal, int attackBonus, int defenseBonus, int speedBonus)
    {
        AttackTotal = attackTotal;
        DefenseTotal = defenseTotal;
        AttackBonus = attackBonus;
        DefenseBonus = defenseBonus;
        SpeedBonus = speedBonus;
    }
}

public readonly struct MatchResult
{
    public readonly TeamScoreInfo BlueInfo;
    public readonly TeamScoreInfo RedInfo;
    public readonly Team Winner;

    public MatchResult(TeamScoreInfo blueInfo, TeamScoreInfo redInfo, Team winner)
    {
        BlueInfo = blueInfo;
        RedInfo = redInfo;
        Winner = winner;
    }
}


public class MatchResultCalculator
{
    readonly DefaultScoreCalculator scoreCalculator;
    readonly TeamBonusCalculator teamBonusCalculator;
    public MatchResultCalculator(DefaultScoreCalculator teamScoreCalculator, TeamBonusCalculator teamBonusCalculator)
    {
        this.scoreCalculator = teamScoreCalculator;
        this.teamBonusCalculator = teamBonusCalculator;
    }

    public MatchResult CalculateResult(IEnumerable<ChampionStatData> blue, IEnumerable<ChampionStatData> red)
    {
        TeamScoreInfo blueInfo = CreateInfo(blue);
        TeamScoreInfo redInfo = CreateInfo(red);

        Team winner;
        if (blueInfo.Total == redInfo.Total) winner = Team.All;
        else if (blueInfo.Total > redInfo.Total) winner = Team.Blue;
        else winner = Team.Red;

        return new MatchResult(blueInfo, redInfo, winner);
    }

    TeamScoreInfo CreateInfo(IEnumerable<ChampionStatData> team) => new TeamScoreInfo(
        scoreCalculator.CalculateAttack(team),
        scoreCalculator.CalculateDefense(team),
        teamBonusCalculator.CalculateAttackBonus(team),
        teamBonusCalculator.CalculateDefenseBonus(team),
        teamBonusCalculator.CalculateSpeedBonus(team)
        );
}
