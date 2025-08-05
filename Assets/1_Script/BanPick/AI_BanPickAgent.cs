using System.Collections.Generic;
using UnityEngine;

public class AI_BanPickAgent
{
    public ChampionSO SelectChampion(IReadOnlyList<ChampionSO> champions)
    {
        return champions[Random.Range(0, champions.Count)];
    }
}
