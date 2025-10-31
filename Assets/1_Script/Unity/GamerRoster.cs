using System.Collections.Generic;
using UnityEngine;

public class GamerRoster : MonoBehaviour
{
    [SerializeField] ChampionRepository championRepository;
    
    [SerializeField] int[] masteryLevels;

    public IEnumerable<ChampionMastery> Blues { get; private set; }
    public IEnumerable<ChampionMastery> Reds { get; private set; }

    public IEnumerable<ChampionMastery> GetTeamMasteries(Team team) => team == Team.Blue ? Blues : Reds;

    public void CreateRandomRoster(int teamSize)
    {
        var drawer = new MasteryDrawer(championRepository.AllId);
        Blues = drawer.DrawRandoms(masteryLevels);
        Reds = drawer.DrawRandoms(masteryLevels);
    }
}
