
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

public class ChampionPersenter
{
    public ChampionViewModel PresentStat(ChampionStatData stat) => new ChampionViewModel(
        $"공격력 : {stat.Attack}",
        $"방어력 : {stat.Defense}",
        $"속도 : {stat.Speed}",
        ""
        );
}
