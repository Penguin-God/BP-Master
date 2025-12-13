using UnityEngine;

public class AI_Main : MonoBehaviour
{
    Team Team;
    PhaseEventDispatcher phaseEventDispatcher;

    [SerializeField] MasteryGenerator masteryGenerator;
    [SerializeField] SelectorsCreatetorSO selectorsCreatetor;

    public void Init(Team team, PhaseEventDispatcher phaseEventDispatcher)
    {
        Team = team;
        this.phaseEventDispatcher = phaseEventDispatcher;
    }

    public void InitAI_BanPick(GameBanPickStorage storage, SlotStorage<Skill> skillSlots, SkillUseController skillUseController)
    {
        selectorsCreatetor.Init(masteryGenerator.GetTeamMasteryManager(Team));
        var ai = new AI_BanPickAgent(Team, storage, selectorsCreatetor.CreateBanSelector(), selectorsCreatetor.CreatePickSelector());
        phaseEventDispatcher.OnPhaseBan += ai.Ban;
        phaseEventDispatcher.OnPhasePick += ai.Pick;
        storage.OnPick += OnPick;
        GetComponent<AI_MonoBehaviourAgent>().Init(new AI_SkillUseAgent(skillSlots, skillUseController));
    }

    void OnPick(SlotData slotData, int id)
    {
        if (slotData.Team != Team) return;
        GetComponent<AI_MonoBehaviourAgent>().UseSkill(slotData);
    }
}