public struct ChampionStatData
{
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int Speed { get; private set; }
    public ChampionStatData(int attack, int defense, int speed)
    {
        Attack = MinZero(attack);
        Defense = MinZero(defense);
        Speed = MinZero(speed);
    }

    static int MinZero(int value) => 0 > value ? 0 : value;

    public ChampionStatData ChangeAttack(int att) => new ChampionStatData(att, Defense, Speed);
    public ChampionStatData ChangeDefense(int def) => new ChampionStatData(Attack, def, Speed);
    public ChampionStatData ChangeSpeed(int speed) => new ChampionStatData(Attack, Defense, speed);
}

public class Champion
{
    readonly public int Id;
    readonly public string Name;
    public ChampionStatData StatData { get; private set; }
    readonly public TraitTargetRule TraitTargetRule;
    readonly public TraitExecutor TraitExecutor;

    public Champion(int id, string name, ChampionStatData statData, TraitTargetRule traitTargetRule, TraitExecutor traitExecutor)
    {
        Id = id;
        Name = name;
        StatData = statData;
        TraitTargetRule = traitTargetRule;
        TraitExecutor = traitExecutor;
    }

    public void ChangeStat(ChampionStatData newStat) => StatData = newStat;
}
