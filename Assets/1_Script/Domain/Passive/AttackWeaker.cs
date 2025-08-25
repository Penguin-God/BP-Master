using System.Linq;
using UnityEngine;

public class AttackWeaker
{
    readonly int Amount;
    public AttackWeaker(int amount) => Amount = amount;

    public ChampionStatData[] Do(ChampionStatData[] datas) 
        => datas.Select(x => new ChampionStatData(x.Attack - Amount, x.Defense, x.Range, x.Speed)).ToArray();
}
