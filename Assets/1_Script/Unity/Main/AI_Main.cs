using System.Collections;
using UnityEngine;

public class AI_Main : MonoBehaviour, IPhaseEntry
{
    Team Team;

    [SerializeField] MasteryGenerator masteryGenerator;
    [SerializeField] SelectorsCreatetorSO selectorsCreatetor;

    AI_BanPickAgent banPickAgent;
    public void EnterBan() => StartCoroutine(CoBan());
    public void EnterPick() => banPickAgent.Pick(Team);

    IEnumerator CoBan()
    {
        yield return new WaitForSeconds(0.5f);
        banPickAgent.Ban(Team);
    }

    public void Init(Team team, GameBanPickStorage storage, SlotStorage<Skill> skillSlots, SkillUseController skillUseController)
    {
        Team = team;

        selectorsCreatetor.Init(masteryGenerator.GetTeamMasteryManager(Team));
        banPickAgent = new AI_BanPickAgent(Team, storage, selectorsCreatetor.CreateBanSelector(), selectorsCreatetor.CreatePickSelector());
        storage.OnPick += OnPick;
        GetComponent<AI_MonoBehaviourAgent>().Init(new AI_SkillUseAgent(skillSlots, skillUseController));
    }

    void OnPick(SlotData slotData, int id)
    {
        if (slotData.Team != Team) return;
        GetComponent<AI_MonoBehaviourAgent>().UseSkill(slotData);
    }
}