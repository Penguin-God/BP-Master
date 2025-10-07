using System.Linq;
using UnityEngine;

public class PlayerRoster : MonoBehaviour
{
    [SerializeField] ProGamerSO[] blueGamers;
    [SerializeField] ProGamerSO[] redGamers;
    public SlotStorage<ProGamer> Rosters = new();
    
    void Start()
    {
        Rosters.AddSlots(Team.Blue, blueGamers.Select(x => x.CreateGamer()));
        Rosters.AddSlots(Team.Red, redGamers.Select(x => x.CreateGamer()));
    }
}
