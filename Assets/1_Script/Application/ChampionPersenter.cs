using System;

public readonly struct ChampionViewModel
{
    public readonly string Attack;
    public readonly string Defense;
    public readonly string Speed;
    public readonly string Trait;

    public ChampionViewModel(string attack, string defense, string speed, string trait)
    {
        Attack = attack;
        Defense = defense;
        Speed = speed;
        Trait = trait;
    }
}

public struct TraitUI_Data
{
    public readonly TraitType TraitType;
    public readonly Side TargetSide;
    public readonly TargetRange Range;
    public readonly int Amount;

    public TraitUI_Data(TraitType traitType, Side targetSide, TargetRange range, int amount)
    {
        TraitType = traitType;
        TargetSide = targetSide;
        Range = range;
        Amount = amount;
    }
}

public class ChampionPersenter
{
    public ChampionViewModel CreateViewModel(ChampionStatData stat, TraitUI_Data traitData) => new ChampionViewModel(
        $"공격력 : {stat.Attack}",
        $"방어력 : {stat.Defense}",
        $"속도 : {stat.Speed}",
        $"{BuildTargetRuleText(traitData.TargetSide, traitData.Range)} {BuildTraitText(traitData.TraitType, traitData.Amount)}"
        );

    string BuildTargetRuleText(Side side, TargetRange range) => (side, range) switch
    {
        (Side.Self, TargetRange.Single) => "아군 단일 대상",
        (Side.Self, TargetRange.All) => "아군 전체",
        (Side.Opponent, TargetRange.Single) => "적군 단일 대상",
        (Side.Opponent, TargetRange.All) => "적군 전체",
        (Side.All, TargetRange.All) => "양팀 전체",
        _ => "대상 없음"
    };

    string BuildTraitText(TraitType traitType, int amount) => (traitType) switch
    {
        TraitType.AttackChanger => $"공격력 {Math.Abs(amount)} {GetChangeLabel(amount)}",
        TraitType.DefenseChanger => $"방어력 {Math.Abs(amount)} {GetChangeLabel(amount)}",
        TraitType.SpeedChanger => $"속도 {Math.Abs(amount)} {GetChangeLabel(amount)}",
        _ => ""
    };

    string GetChangeLabel(int amount) => amount > 0 ? "증가" : "감소";
}
