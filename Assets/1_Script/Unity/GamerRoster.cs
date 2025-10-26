using UnityEngine;

public class GamerRoster : MonoBehaviour
{
    [SerializeField] ProGamerSO[] blueGamers;
    [SerializeField] ProGamerSO[] redGamers;
    [SerializeField] ChampionRepository championRepository;
    public SlotStorage<ProGamer> Rosters = new();

    [SerializeField] int teamCount;
    [SerializeField] int[] masteryLevels;

    public void SetRandomRoster()
    {
        RandomRosterFactory factory = new RandomRosterFactory(new MasteryDrawer(championRepository.AllId));
        Rosters = factory.CreateRoster(teamCount, masteryLevels);
    }
}
