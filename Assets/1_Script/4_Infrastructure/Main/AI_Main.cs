using System.Collections;
using UnityEngine;

public class AI_Main : MonoBehaviour, IPhaseEntry
{
    Team Team;
    [SerializeField] AI_SelectorFactory selectorsCreatetor;

    AI_BanPickAgent banPickAgent;
    AI_SkillExecutionUseCase skillUseCase;

    public void EnterBan() => StartCoroutine(CoBan());
    public void EnterPick() => banPickAgent.Pick(Team);

    public void Init(Team team, BanPickStorage storage, SkillUsecase skillUseController, ChampionCatalog championCatalog, MasteryRegistry masteryRegistry, BanPickHandler banPickHandler)
    {
        Team = team;

        selectorsCreatetor.Init(Team, championCatalog, masteryRegistry.GetTeamMasteryManager(Team), banPickHandler.PickSlotFacade.StatusSlots);
        banPickAgent = new AI_BanPickAgent(Team, storage, selectorsCreatetor.CreateBanSelector(), selectorsCreatetor.CreatePickSelector(), banPickHandler);
        banPickHandler.BanPickEventDispatcher.OnPick += OnPick;
        skillUseCase = new AI_SkillExecutionUseCase(banPickHandler.PickSlotFacade.SkillSlots, skillUseController, new SkillTargetService(new RandomSkillTargetSelector()));
    }

    IEnumerator CoBan()
    {
        yield return new WaitForSeconds(0.5f);
        banPickAgent.Ban(Team);
    }

    void OnPick(SlotData slotData, int id)
    {
        if (slotData.Team != Team) return;
        StartCoroutine(Co_UseSkill(slotData));
    }

    IEnumerator Co_UseSkill(SlotData slot)
    {
        yield return new WaitForSeconds(1.5f);
        skillUseCase.UseSkill(slot);
    }
}