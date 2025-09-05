

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
}

public class Champion
{
    readonly public int Id;
    readonly public string Name;
    public ChampionStatData StatData { get; private set; }
    readonly public Trait Trait;

    public Champion(int id, string name, ChampionStatData statData, Trait trait)
    {
        Id = id;
        Name = name;
        StatData = statData;
        Trait = trait;
    }
}
