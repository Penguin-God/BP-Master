using System.Linq;
using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfig;
    [SerializeField] ChampionRepository champManager;

    [SerializeField] MatchUI_Controller matchUI_Controller;
    [SerializeField] MasteryGenerator masteryGenerator;
    [SerializeField] AI_Main ai_main;

    SlotStorage<Champion> championSlots = new();
    SlotStorage<ChampionStatus> statusSlots = new();
    SlotStorage<Skill> skillSlots = new();

    [SerializeField] ChampionSelector_UI championSelector;

    public void GameStart(Team playerTeam)
    {
        masteryGenerator.CreateRandomRoster(matchConfig.TeamSize);

        var storage = new GameBanPickStorage(champManager.AllId);

        // 이런 생성 로직들을 처리해주는 어댑터
        IPhaseEntry blue = playerTeam == Team.Blue ? championSelector : ai_main;
        IPhaseEntry red = playerTeam == Team.Red ? championSelector : ai_main;
        var phaseEventDispatcher = new PhaseEventDispatcher();
        PhaseFlowOrchestrator phaseManager = new(GetComponent<GamePhaseLoder>().LoadPhase(), phaseEventDispatcher, new TeamPhaseEntryDispatcher(blue, red));

        phaseEventDispatcher.OnPhaseDone += OnDone;
        storage.OnPick += OnPick;

        skillController = new SkillUseController(statusSlots);
        skillController.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);
        storage.OnBan += (team, id) => phaseManager.SubmitAction(team);

        matchUI_Controller.Init(playerTeam, storage, phaseManager, phaseEventDispatcher, statusSlots, skillSlots, skillController); // start보다 먼저

        ai_main.Init(EnumCaster.GetOppoentTeam(playerTeam), storage, skillSlots, skillController);

        phaseManager.Start();
    }

    SkillUseController skillController;
    void OnPick(SlotData slotData, int id)
    {
        var champion = champManager.GetChampionData(id).CreateChampion();
        championSlots.AddSlot(slotData.Team, champion);
        statusSlots.AddSlot(slotData.Team, champion.Status);
        skillSlots.AddSlot(slotData.Team, champion.Skill);
        new TraitFactory(matchConfig.TraitConfig, statusSlots).Create(slotData.Team, champion.Status.TraitType).Do();
        if (masteryGenerator.GetTeamMasteries(Team.Blue).Select(x => x.ChampionId).Contains(id))
            new TeamMasteryApplier().ApplyStatChange(champion.Status, masteryGenerator.GetTeamMasteries(Team.Blue).First(x => x.ChampionId == id).Level);
    }

    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(statusSlots);
        matchUI_Controller.Done(result);
    }
}
