using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_BanPickAgent : ISelectWait, ISelector
{
    readonly ChampionSelectStorage storage;
    readonly IEnumerable<int> allChampions;
    public AI_BanPickAgent(ChampionSelectStorage storage, IEnumerable<int> allChampions)
    {
        this.storage = storage;
        this.allChampions = allChampions;
    }

    public IEnumerator WaitSelect()
    {
        yield return new WaitForSeconds(1.5f);
    }

    int ISelector.SelectChampion()
    {
        int[] selectableChams = allChampions.Except(storage.SelectChampions).ToArray();
        return selectableChams[Random.Range(0, selectableChams.Length)];
    }
}
