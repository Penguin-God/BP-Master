public readonly struct ChampionStatData
{
    public readonly int Attack;
    public readonly int Defense;
    public readonly int Speed;
    public ChampionStatData(int attack, int defense, int speed)
    {
        Attack = MinZero(attack);
        Defense = MinZero(defense);
        Speed = MinZero(speed);
    }

    static int MinZero(int value) => 0 > value ? 0 : value;

    public readonly ChampionStatData ChangeAttack(int att) => new ChampionStatData(att, Defense, Speed);
    public readonly ChampionStatData ChangeDefense(int def) => new ChampionStatData(Attack, def, Speed);
    public readonly ChampionStatData ChangeSpeed(int speed) => new ChampionStatData(Attack, Defense, speed);
}