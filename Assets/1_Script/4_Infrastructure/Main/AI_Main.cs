using System.Collections;
using UnityEngine;

public class AI_Main : MonoBehaviour, IPhaseEntry
{
    Team Team;
    [SerializeField] AI_SelectorFactory selectorsCreatetor;

    AI_BanPickAgent banPickAgent;
    public void EnterBan() => StartCoroutine(CoBan());
    public void EnterPick() => banPickAgent.Pick(Team);

    IEnumerator CoBan()
    {
        yield return new WaitForSeconds(0.5f);
        banPickAgent.Ban(Team);
    }

    public void Init(Team team, BanPickStorage storage, SlotStorage<Skill> skillSlots, SkillUsecase skillUseController, SlotStorage<ChampionStatus> statusSlots, ChampionCatalog championCatalog, MasteryRegistry masteryRegistry)
    {
        Team = team;

        selectorsCreatetor.Init(Team, championCatalog, masteryRegistry.GetTeamMasteryManager(Team), statusSlots);
        banPickAgent = new AI_BanPickAgent(Team, storage, selectorsCreatetor.CreateBanSelector(), selectorsCreatetor.CreatePickSelector());
        storage.OnPick += OnPick;
        GetComponent<AI_MonoBehaviourAgent>().Init(new AI_SkillExecutionUseCase(skillSlots, skillUseController, new RandomSkillTargetSelector()));
    }

    void OnPick(SlotData slotData, int id)
    {
        if (slotData.Team != Team) return;
        GetComponent<AI_MonoBehaviourAgent>().UseSkill(slotData);
    }
}