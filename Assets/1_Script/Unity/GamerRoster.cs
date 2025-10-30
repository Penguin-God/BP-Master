using System.Collections.Generic;
using UnityEngine;

public class GamerRoster : MonoBehaviour
{
    [SerializeField] ProGamerSO[] blueGamers;
    [SerializeField] ProGamerSO[] redGamers;
    [SerializeField] ChampionRepository championRepository;
    public SlotStorage<ProGamer> Rosters = new();

    [SerializeField] int[] masteryLevels;

    public IEnumerable<ChampionMastery> blues;
    public IEnumerable<ChampionMastery> reds;

    public void CreateRandomRoster(int teamSize)
    {
        var drawer = new MasteryDrawer(championRepository.AllId);

        RandomRosterFactory factory = new RandomRosterFactory(new MasteryDrawer(championRepository.AllId));
        Rosters = factory.CreateRoster(teamSize, masteryLevels);

        blues = drawer.DrawRandoms(masteryLevels);
        reds = drawer.DrawRandoms(masteryLevels);
    }
}
