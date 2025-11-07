using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckPrioritySO", menuName = "BP Master/DeckPrioritySO")]
public class DeckPrioritySO : ScriptableObject
{
    [SerializeField] ChampionSO[] bans;
    [SerializeField] ChampionSO[] picks;

    public IEnumerable<int> Bans => bans.Select(x => x.Id);
    public IEnumerable<int> Picks => picks.Select(x => x.Id);
}
