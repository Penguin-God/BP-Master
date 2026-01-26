using UnityEngine;

public class MasteryTooltipTrigger : TooltipTrigger
{
    [SerializeField] ChampionRepository championRepository;
    [SerializeField] MasteryRegistory masteryData;
    [SerializeField] Team team;

    protected override string BuildText() => new MasteryTextBuilder(championRepository.NameCatalog).BuildMasteriesText(masteryData.GetTeamMasteries(team));
}
