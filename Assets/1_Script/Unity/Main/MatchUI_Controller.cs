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
    public void Init(Team playerTeam, GameBanPickStorage storage, PhaseManager phaseManager, PhaseEventDispatcher eventDispatcher, SlotStorage<ChampionStatus> statusSlots, SlotStorage<Skill> skillSlots, SkillUseController skillController)
    {
        team = playerTeam;
        slotViews.InitSlotView();
        masteryView.ViewMastery(championRepository);
        championSelector.Init(new ChampionSelectPresenter(storage), phaseManager);

        masteryHighlighter.Highlight(playerTeam); // championSelector 이후에 시작

        storage.OnBan += banView.UpdateBanList;
        storage.OnPick += slotViews.PickChampion;

        storage.OnBan += (team, id) => championDrawer.InActiveButton(id);
        storage.OnPick += (slot, id) => championDrawer.InActiveButton(id);

        eventDispatcher.OnPhaseSkill += _ => banView.HideBan();
        eventDispatcher.OnPhaseSkill += _ => championDrawer.HideView();

        storage.OnPick += (slot, id) => slotViews.InitTrackerViewSlots(statusSlots);
        storage.OnPick += OnPick;

        skillUseView.gameObject.SetActive(false);
        skillButtonView.Init(playerTeam);
        skillUseView.Init(new SkillUsePersenter(matchConfig.TeamSize), skillSlots, skillController);

        storage.OnPick += (slot, id) => scoreView.UpdateTeamScore(IdToStatus(storage.PickIds), slot.Team);

        eventDispatcher.OnGameProgress += gameFlowView.ViewGameFlow;
    }

    void OnPick(SlotData slotData, int id)
    {
        if (slotData.Team != team) return;
        skillUseView.UseSkill(slotData);
    }

    SlotStorage<ChampionStatus> IdToStatus(SlotStorage<int> idStorage) 
        => StorageConverter.ConvertStorage(idStorage, id => new ChampionStatus(championRepository.GetChampionData(id).StatData, TraitType.None));

    public void SkillUI_Init(Team playerTeam, PhaseEventDispatcher eventDispatcher, SkillUseController skillController, SlotStorageManager slotStorageManager, SkillSlotFilter filter)
    {
        skillUseLog.SetActive(true);
        masteryView.gameObject.SetActive(false);
        
        skillUseView.Init(new SkillUsePersenter(matchConfig.TeamSize), slotStorageManager.SkillSlots, skillController);
        
        gameFlowView.Init(slotStorageManager.ChampionDataSlots);
        skillController.OnUseSkill += gameFlowView.UpdateUseSkill;

        eventDispatcher.OnPhaseSkill += (team) => scoreView.UpdateTeamScore(slotStorageManager.StatusSlots, team);
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
