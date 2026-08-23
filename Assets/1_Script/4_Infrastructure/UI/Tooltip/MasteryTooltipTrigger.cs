using System.Linq;
using UnityEngine;

public class MasteryTooltipTrigger : TooltipTrigger
{
    [SerializeField] Team team;

    MasteryRegistry masteryRegistry;
    public void Inject(MasteryRegistry registry) => this.masteryRegistry = registry;

    protected override string BuildText()
    {
        if (masteryRegistry == null) return "";
        return new MasteryTextBuilder(ChampionDataLoder.NameCatalog.ToDictionary(k => k.Key, v => v.Value)).BuildMasteriesText(masteryRegistry.GetTeamMasteryCollection(team).AllMasteries);
    }
}