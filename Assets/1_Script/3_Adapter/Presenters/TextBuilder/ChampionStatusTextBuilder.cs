
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

public readonly struct CombatModifierTextModel
{
    public readonly string IncreaseRateText;
    public readonly string DecreaseRateText;

    public CombatModifierTextModel(string increaseRateText, string decreaseRateText)
    {
        IncreaseRateText = increaseRateText;
        DecreaseRateText = decreaseRateText;
    }
}

public class ChampionStatusTextBuilder
{
    public ChampionStatModel CreateStatViewModel(ChampionStatData stat) =>
        new ChampionStatModel(
        $"공 {stat.Attack}",
        $"방 {stat.Defense}",
        $"속도 {stat.Speed}"
    );

    public CombatModifierTextModel BuildCombatModel(float increaseRate, float decreaseRate)
    {
        return new CombatModifierTextModel(
            $"증가율 : {increaseRate.ToString("0.##")}",
            $"감소율 : {decreaseRate.ToString("0.##")}"
            );
    }
}