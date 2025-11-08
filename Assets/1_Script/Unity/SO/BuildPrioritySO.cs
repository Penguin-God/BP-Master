using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildPrioritySO", menuName = "BP Master/BuildPrioritySO")]
public class BuildPrioritySO : ScriptableObject
{
    [SerializeField] ChampionSO[] bans;
    [SerializeField] ChampionSO[] picks;

    public IEnumerable<int> Bans => bans.Select(x => x.Id);
    public IEnumerable<int> Picks => picks.Select(x => x.Id);
}
