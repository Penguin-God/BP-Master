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
