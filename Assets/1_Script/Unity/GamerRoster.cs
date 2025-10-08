using System.Linq;
using UnityEngine;

public class GamerRoster : MonoBehaviour
{
    [SerializeField] ProGamerSO[] blueGamers;
    [SerializeField] ProGamerSO[] redGamers;
    public SlotStorage<ProGamer> Rosters = new();

    [SerializeField] int teamCount;
    [SerializeField] int[] masteryLevels;
    //void Start()
    //{
    //    Rosters.AddSlots(Team.Blue, blueGamers.Select(x => x.CreateGamer()));
    //    Rosters.AddSlots(Team.Red, redGamers.Select(x => x.CreateGamer()));
    //}

    public void SetRandomRoster(ChampionCatalog catalog)
    {
        RandomRosterFactory factory = new RandomRosterFactory(new MasteryDrawer(catalog));
        Rosters = factory.CreateRoster(teamCount, masteryLevels);
    }
}
