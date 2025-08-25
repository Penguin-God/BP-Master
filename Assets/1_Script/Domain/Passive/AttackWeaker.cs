using System;
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

public class SelectAttackWeaker : IActivePassive
{
    readonly int Amount;
    public SelectAttackWeaker(int amount) => Amount = amount;

    public void Do(int target)
    {
        
    }

    public ChampionStatData[] DoAtIndex(ChampionStatData[] datas, int targetIndex)
    {
        if (targetIndex >= datas.Length)
            throw new ArgumentOutOfRangeException(nameof(targetIndex) + " 인덱스 이거 맞아요?");

        return datas.Select((data, i)
            => i == targetIndex ? data.ChangeAttack(data.Attack - Amount) : data).ToArray();
    }
}
