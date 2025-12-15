using TMPro;
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
    [SerializeField] MasteryView masteryView;
    [SerializeField] GameObject skillUseLog;

    MasteryButtonHighlighter masteryHighlighter;
    public void Awake()
    {
        masteryHighlighter = GetComponentInChildren<MasteryButtonHighlighter>(true);
    }

    Team team;
    public void Init(Team playerTeam, GameBanPickStorage storage, PhaseFlowOrchestrator phaseManager, PhaseEventDispatcher eventDispatcher, SlotStorage<ChampionStatus> statusSlots, SlotStorage<Skill> skillSlots, SkillUseController skillController)
    {
        team = playerTeam;
        slotViews.InitSlotView(statusSlots);
        masteryView.ViewMastery(championRepository);
        championSelector.Init(new ChampionSelectPresenter(storage), phaseManager);

        masteryHighlighter.Highlight(playerTeam); // championSelector 이후에 시작

        storage.OnBan += banView.UpdateBanList;
        storage.OnPick += slotViews.PickChampion;

        storage.OnBan += (team, id) => championDrawer.InActiveButton(id);
        storage.OnPick += (slot, id) => championDrawer.InActiveButton(id);

        eventDispatcher.OnPhaseSkill += _ => banView.HideBan();
        eventDispatcher.OnPhaseSkill += _ => championDrawer.HideView();

        storage.OnPick += OnPick;

        skillUseView.gameObject.SetActive(false);
        skillButtonView.Init(playerTeam);
        skillUseView.Init(skillSlots, skillController);

        storage.OnPick += (slot, id) => scoreView.UpdateTeamScore(statusSlots, slot.Team);
        skillController.OnUseSkill += (slot) => scoreView.UpdateTeamScore(statusSlots, slot.Team);

        eventDispatcher.OnGameProgress += gameFlowView.ViewGameFlow;

        eventDispatcher.OnPhaseDone += () => championDrawer.gameObject.SetActive(false);
    }

    void SkillUI_Init(SkillUseController skillController, SlotStorageManager slotStorageManager)
    {
        skillUseLog.SetActive(true);
        masteryView.gameObject.SetActive(false);

        skillUseView.Init(slotStorageManager.SkillSlots, skillController);

        gameFlowView.Init(slotStorageManager.ChampionDataSlots);
        skillController.OnUseSkill += gameFlowView.UpdateUseSkill;
    }

    void OnPick(SlotData slotData, int id)
    {
        if (slotData.Team != team) return;
        skillUseView.UseSkill(slotData);
    }

    [SerializeField] GameObject scores;
    [SerializeField] TextMeshProUGUI textBlue;
    [SerializeField] TextMeshProUGUI textRed;
    public void Done(MatchResult result)
    {
        skillButtonView.gameObject.SetActive(false);
        scores.SetActive(true);
        textBlue.text = new ScoreTextBuilder().BuildText(result.BlueInfo);
        textRed.text = new ScoreTextBuilder().BuildText(result.RedInfo);
        print($"승자 : {result.Winner}");
    }
}
