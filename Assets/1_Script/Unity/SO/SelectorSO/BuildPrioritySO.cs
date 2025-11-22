using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildPrioritySO", menuName = "BP Master/BuildPrioritySO")]
public class BuildPrioritySO : SelectorsCreatetorSO
{
    [SerializeField] ChampionSO[] bans;
    [SerializeField] ChampionSO[] picks;

    IEnumerable<int> Bans => bans.Select(x => x.Id);
    IEnumerable<int> Picks => picks.Select(x => x.Id);

    public override IBanSelector CreateBanSelector() => CreateSelector();
    public override IPickSelector CreatePickSelector() => CreateSelector();

    public PrioritySelector CreateSelector() => new PrioritySelector(Bans, Picks);
}