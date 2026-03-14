using System.Collections;
using UnityEngine;

public class AI_Main : MonoBehaviour, IPhaseEntry
{
    Team Team;
    int defaultId;
    [SerializeField] AIFactorySO aiFactory;

    AI_BanPickAgent banPickAgent;
    AI_SkillExecutionUseCase skillUseCase;

    public void EnterBan() => StartCoroutine(CoBan());
    public void EnterPick() => banPickAgent.Pick(Team);

    public void Init(int ai_id, Team team, BanPickStorage storage, SkillUsecase skillUseController, ChampionCatalog championCatalog, MasteryRegistry masteryRegistry, BanPickHandler banPickHandler, PhaseAdvancer phaseAdvancer)
    {
        Team = team;

        AI_SelectorFactory selectorFactory = aiFactory.CreateAI(ai_id, Team, storage, championCatalog, masteryRegistry, banPickHandler, phaseAdvancer);

        banPickAgent = new AI_BanPickAgent(Team, storage, selectorFactory.CreateBanSelector(), selectorFactory.CreatePickSelector(), banPickHandler);
        banPickHandler.BanPickEventDispatcher.OnPick += UseSkill;
        skillUseCase = new AI_SkillExecutionUseCase(banPickHandler.PickSlotFacade.SkillSlots, skillUseController, new SkillTargetService(new HighStatTargetSelector(banPickHandler.PickSlotFacade.StatusSlots)));
    }

    IEnumerator CoBan()
    {
        yield return new WaitForSeconds(0.5f);
        banPickAgent.Ban(Team);
    }

    void UseSkill(SlotData slotData, int id)
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