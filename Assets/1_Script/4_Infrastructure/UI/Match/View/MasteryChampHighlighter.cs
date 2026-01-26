using System.Linq;
using UnityEngine;

public class MasteryButtonHighlighter : MonoBehaviour
{
    [SerializeField] ChampionButtonView championButtonView;
    
    public void Highlight(Team team, MasteryRegistry masteryRegistry)
    {
        var masteredIds = masteryRegistry.GetTeamMasteryManager(team).AllMasteries.Select(x => x.ChampionId);

        championButtonView
            .Buttons
            .Where(btn => masteredIds.Contains(btn.GetComponent<ChampionIdentify>().Id))
            .ToList()
            .ForEach(button => ButtonUtil.ChangeButtonColor(button, new Color32(111, 233, 65, 255)));
    }
}
