using System.Linq;
using UnityEngine;

public class MasteryButtonHighlighter : MonoBehaviour
{
    [SerializeField] MasteryRegistory masteryGenerator;
    [SerializeField] ChampionButtonView championButtonView;
    
    public void Highlight(Team team)
    {
        var masteredIds = masteryGenerator.GetTeamMasteries(team).Select(x => x.ChampionId);

        championButtonView
            .Buttons
            .Where(btn => masteredIds.Contains(btn.GetComponent<ChampionIdentify>().Id))
            .ToList()
            .ForEach(button => ButtonUtil.ChangeButtonColor(button, new Color32(111, 233, 65, 255)));
    }
}
