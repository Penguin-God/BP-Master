using UnityEngine;

public class MatchUI_Controller : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfig;
    [SerializeField] ChampionSelector_UI championSelector;
    [SerializeField] ChampionButtonView championDrawer;
    [SerializeField] SkillUseController_UI skillUseView;
    [SerializeField] SlotViewOrchestrator slotViews;
    [SerializeField] ScoreView scoreView;
    
    [SerializeField] GameFlowView gameFlowView;
    [SerializeField] SkillButtonView skillButtonView;
    [SerializeField] ChampionRepository championRepository;
    [SerializeField] BanView banView;

    MasteryButtonHighlighter masteryHighlighter;
    public void Awake()
    {
        masteryHighlighter = GetComponentInChildren<MasteryButtonHighlighter>(includeInactive: true);
    }

    Team team;
    public void Init(Team playerTeam, GameBanPickStorage storage, PhaseFlowOrchestrator phaseManager, PhaseEventDispatcher eventDispatcher, SlotStorage<ChampionStatus> statusSlots, SlotStorage<Skill> skillSlots, SkillUsecase skillController)
    {
        team = playerTeam;
        slotViews.InitSlotView(statusSlots);
        championSelector.Init(storage, phaseManager);

        masteryHighlighter.Highlight(playerTeam); // championSelector 이후에 시작
        championDrawer.InActiveButtons(storage.SelectableIds);

        storage.OnBan += banView.UpdateBanList;
        storage.OnPick += slotViews.PickChampion;

        storage.OnBan += (_, id) => championDrawer.InActiveButton(id);
        storage.OnPick += (_, id) => championDrawer.InActiveButton(id);

        storage.OnPick += OnPick;

        skillUseView.gameObject.SetActive(false);
        skillButtonView.Init(playerTeam);
        skillUseView.Init(skillSlots, skillController);

        storage.OnPick += (slot, _) => scoreView.UpdateTeamScore(statusSlots, slot.Team);
        skillController.OnUseSkill += (slot) => scoreView.UpdateTeamScore(statusSlots, slot.Team);

        eventDispatcher.OnGameProgress += gameFlowView.ViewGameFlow;
    }

    void OnPick(SlotData slotData, int id)
    {
        if (slotData.Team != team) return;
        skillUseView.UseSkill(slotData);
    }

    [SerializeField] GameObject scores;
    public void Done(MatchResult result, bool isGameEnd)
    {
        championDrawer.gameObject.SetActive(false);
        Instantiate(scores, transform).GetComponent<MatchResultView>().ShowResult(result, isGameEnd);
    }
}
