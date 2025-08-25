using System.Linq;

public class AttackWeaker : ITraitAction
{
    readonly int Amount;
    public AttackWeaker(int amount) => Amount = amount;

    public ChampionStatData Do(ChampionStatData stat) => stat.ChangeAttack(stat.Attack - Amount);

    public ChampionStatData[] DoAtIndex(ChampionStatData[] datas, int targetIndex)
    {
        return datas.Select((data, i) 
            => i == targetIndex ? data.ChangeAttack(data.Attack - Amount) : data).ToArray();
    }
}
