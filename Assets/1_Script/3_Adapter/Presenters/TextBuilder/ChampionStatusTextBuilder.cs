
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

public static class ChampionStatusTextBuilder
{
    public static ChampionStatModel CreateStatViewModel(ChampionStatData stat) =>
        new ChampionStatModel(
        $"공 {stat.Attack}",
        $"방 {stat.Defense}",
        $"속도 {stat.Speed}"
    );
}