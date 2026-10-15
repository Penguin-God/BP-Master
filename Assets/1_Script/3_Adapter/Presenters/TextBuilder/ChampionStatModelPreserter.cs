
public readonly struct ChampionStatModel
{
    public readonly string Attack;
    public readonly string Defense;
    public readonly string Speed;

    public ChampionStatModel(string attack, string defense, string speed)
    {
        Attack = attack;
        Defense = defense;
        Speed = speed;
    }
}

public static class ChampionStatModelPreserter
{
    public static ChampionStatModel CreateStatModel(ChampionStatData stat) =>
        new ChampionStatModel(
            $"공 {stat.Attack}",
            $"방 {stat.Defense}",
            $"속도 {stat.Speed}"
    );

    // 보너스 스탯에 따라 색상을 적용
    public static ChampionStatModel CreateStatModel(ChampionStatData baseStat, ChampionStatData bonusStat)
    {
        var finalStat = baseStat + bonusStat;
        return new ChampionStatModel(
            Format("공", finalStat.Attack, bonusStat.Attack > 0),
            Format("방", finalStat.Defense, bonusStat.Defense > 0),
            Format("속도", finalStat.Speed, bonusStat.Speed > 0)
        );
        string Format(string prefix, int value, bool isUpgraded)
        {
            string text = $"{prefix} {value}";
            return isUpgraded ? $"<color=green>{text}</color>" : text;
        }
    }
}