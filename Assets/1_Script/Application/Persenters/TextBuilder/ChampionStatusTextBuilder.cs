using System;

public readonly struct StatViewModel
{
    public readonly string Attack;
    public readonly string Defense;
    public readonly string Speed;

    public StatViewModel(string attack, string defense, string speed)
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
    public StatViewModel CreateStatViewModel(ChampionStatData stat) =>
        new StatViewModel(
        $"공 {stat.Attack}",
        $"방 {stat.Defense}",
        $"속도 {stat.Speed}"
    );

    public string BuildTraitText(TraitType traitType) => traitType switch
    {
        TraitType.None => "없음",
        TraitType.Charge => "돌격",
        TraitType.Guard => "가드",
        TraitType.Amplifier => "증폭",
        _ => throw new NotImplementedException(),
    };

    public CombatModifierTextModel BuildCombatModel(float increaseRate, float decreaseRate)
    {
        return new CombatModifierTextModel(increaseRate.ToString(), decreaseRate.ToString());
    }
}