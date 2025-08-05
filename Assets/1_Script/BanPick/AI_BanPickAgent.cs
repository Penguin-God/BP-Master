using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_BanPickAgent : IBanPickAgent
{
    readonly ChampionSelectStorage storage;
    public AI_BanPickAgent(ChampionSelectStorage storage) => this.storage = storage;

    public ChampionSO SelectChampion() => storage.SelectableChampions[Random.Range(0, storage.SelectableChampions.Count)];

    public IEnumerator WaitSelect()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
