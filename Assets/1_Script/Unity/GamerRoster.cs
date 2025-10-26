using UnityEngine;

public class GamerRoster : MonoBehaviour
{
    [SerializeField] ProGamerSO[] blueGamers;
    [SerializeField] ProGamerSO[] redGamers;
    [SerializeField] ChampionRepository championRepository;
    public SlotStorage<ProGamer> Rosters = new();

    [SerializeField] int[] masteryLevels;

    public void CreateRandomRoster(int teamSize)
    {
        RandomRosterFactory factory = new RandomRosterFactory(new MasteryDrawer(championRepository.AllId));
        Rosters = factory.CreateRoster(teamSize, masteryLevels);
    }
}
