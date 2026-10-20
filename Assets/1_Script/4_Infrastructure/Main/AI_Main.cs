using System.Collections;
using UnityEngine;

public class AI_Main : MonoBehaviour, IPhaseEntry
{
    Team Team;
    [SerializeField] AISelectorFactory aiFactory;

    AI_BanPickAgent banPickAgent;
    
    public void EnterBan() => StartCoroutine(CoBan());
    public void EnterPick() => banPickAgent.Pick(Team);

    public void Init(int ai_id, Team team, BanPickStorage storage, SkillUsecase skillUseController, ChampionCatalog championCatalog, MasteryRegistry masteryRegistry, BanPickHandler banPickHandler, PhaseAdvancer phaseAdvancer)
    {
        Team = team;

        AI_SelectorSO selectorFactory = aiFactory.CreateAI(ai_id, Team, championCatalog, masteryRegistry, banPickHandler, phaseAdvancer);

        banPickAgent = new AI_BanPickAgent(Team, storage, selectorFactory.CreateBanSelector(), selectorFactory.CreatePickSelector(), banPickHandler);
        banPickHandler.BanPickEventDispatcher.OnPick += UseSkill;

        void UseSkill(SlotData slotData, int id)
        {
            if (slotData.Team != Team) return;
            StartCoroutine(Co_UseSkill(slotData));
        }

        IEnumerator Co_UseSkill(SlotData slot)
        {
            yield return new WaitForSeconds(1.5f);
            skillUseController.AutoUseSkill(slot, TargetOrders.StatOrder(banPickHandler.PickSlotFacade.SkillSlots.GetSlot(slot)));
        }
    }

    IEnumerator CoBan()
    {
        yield return new WaitForSeconds(0.5f);
        banPickAgent.Ban(Team);
    }
}