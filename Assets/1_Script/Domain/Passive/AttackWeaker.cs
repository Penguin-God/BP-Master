using System;
using System.Linq;

public class AttackWeaker
{
    readonly int Amount;
    public AttackWeaker(int amount) => Amount = amount;

    public ChampionStatData[] Do(ChampionStatData[] datas) => datas.Select(x => x.ChangeAttack(x.Attack - Amount)).ToArray();

    public ChampionStatData[] DoAtIndex(ChampionStatData[] datas, int targetIndex)
    {
        if (datas == null) throw new ArgumentNullException(nameof(datas));
        if ((uint)targetIndex >= (uint)datas.Length)
            throw new ArgumentOutOfRangeException(nameof(targetIndex));

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
