

public struct ChampionStatData
{
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int Range { get; private set; }
    public int Speed { get; private set; }
    public ChampionStatData(int attack, int defense, int range, int speed)
    {
        Attack = MinZero(attack);
        Defense = MinZero(defense);
        Range = MinZero(range);
        Speed = MinZero(speed);
    }

    static int MinZero(int value) => 0 > value ? 0 : value;
}

public class Champion
{
    readonly public int Id;
    readonly public string Name;
    public ChampionStatData StatData { get; private set; }

    public Champion(int id, string name, ChampionStatData statData)
    {
        Id = id;
        Name = name;
        StatData = statData;
    }
}
