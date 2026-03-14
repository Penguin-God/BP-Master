using UnityEngine;

public class MasteryTooltipTrigger : TooltipTrigger
{
    [SerializeField] ChampionRepository championRepository;
    [SerializeField] Team team;

    MasteryRegistry masteryRegistry;
    public void Inject(MasteryRegistry registry) => this.masteryRegistry = registry;
    protected override string BuildText()
    {
        if (masteryRegistry == null) return "";
        return new MasteryTextBuilder(championRepository.NameCatalog).BuildMasteriesText(masteryRegistry.GetTeamMasteryCollection(team).AllMasteries);
    }
}
