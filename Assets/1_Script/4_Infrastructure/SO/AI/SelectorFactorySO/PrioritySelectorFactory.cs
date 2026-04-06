using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildPrioritySO", menuName = "AI/Selector/BuildPriority")]
public class BuildPrioritySO : AI_SelectorSO
{
    [SerializeField] ChampionSO[] bans;
    [SerializeField] ChampionSO[] picks;

    IEnumerable<int> Bans => bans.Select(x => x.Id);
    IEnumerable<int> Picks => picks.Select(x => x.Id);

    public override IChampionSelector CreateBanSelector() => BanSelector();
    public override IChampionSelector CreatePickSelector() => PickSelector();

    public PrioritySelector BanSelector() => new PrioritySelector(Bans);
    public PrioritySelector PickSelector() => new PrioritySelector(Picks);
}