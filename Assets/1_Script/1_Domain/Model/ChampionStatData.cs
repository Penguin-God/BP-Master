public readonly struct ChampionStatData
{
    public readonly int Attack;
    public readonly int Defense;

    public ChampionStatData(int attack, int defense)
    {
        Attack = MinZero(attack);
        Defense = MinZero(defense);
    }

    static int MinZero(int value) => 0 > value ? 0 : value;

    public readonly ChampionStatData ChangeAttack(int att) => new ChampionStatData(att, Defense);
    public readonly ChampionStatData ChangeDefense(int def) => new ChampionStatData(Attack, def);

    public int GetStatValue(StatType statType) => statType switch
    {
        StatType.Attack => Attack,
        StatType.Defense => Defense,
        _ => 0
    };

    public static ChampionStatData operator +(ChampionStatData a, ChampionStatData b) => new ChampionStatData(a.Attack + b.Attack, a.Defense + b.Defense);
}
