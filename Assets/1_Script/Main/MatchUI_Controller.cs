using TMPro;
using UnityEngine;

public class MatchUI_Controller : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfig;
    [SerializeField] ChampionSelector_UI championSelector;
    [SerializeField] ChampionButtonView championDrawer;
    [SerializeField] SkillUseController_UI traitUseView;
    [SerializeField] SlotViewOrchestrator banPickView;
    [SerializeField] ScoreView scoreView;
    
    [SerializeField] GameFlowView gameFlowView;
    [SerializeField] SkillButtonView skillButtonView;
    [SerializeField] ChampionRepository championRepository;
    [SerializeField] BanView banView;
    [SerializeField] MasteryView masteryView;

    public void Init(GameBanPickStorage storage, PhaseManager phaseManager, PhaseEventDispatcher eventDispatcher)
    {
        banPickView.InitSlotView();
        masteryView.ViewMastery(championRepository);

        championSelector.Init(new ChampionSelectPresenter(storage), phaseManager);

        storage.OnBan += banView.UpdateBanList;
        storage.OnPick += banPickView.PickChampion;

        storage.OnBan += (team, id) => championDrawer.InActiveButton(id);
        storage.OnPick += (slot, id) => championDrawer.InActiveButton(id);

        eventDispatcher.OnPhaseSkill += _ => banView.HideBan();
        eventDispatcher.OnPhaseSkill += _ => championDrawer.HideView();

        traitUseView.gameObject.SetActive(false);

        storage.OnPick += (slot, id) => scoreView.UpdateTeamScore(IdToStatus(storage.PickIds), slot.Team);

        eventDispatcher.OnGameProgress += gameFlowView.ViewGameFlow;
    }

    SlotStorage<ChampionStatus> IdToStatus(SlotStorage<int> idStorage) 
        => StorageConverter.ConvertStorage(idStorage, id => new ChampionStatus(championRepository.GetChampionData(id).StatData, TraitType.None));

    public void TraitUI_Init(Team playerTeam, PhaseEventDispatcher eventDispatcher, SkillUseController skillController, SlotStorageManager slotStorageManager, SkillSlotFilter filter)
    {
        banPickView.InitTrackerViewSlots(slotStorageManager.StatusSlots);
        
        skillButtonView.Init(filter, playerTeam);
        traitUseView.Init(new SkillUsePersenter(matchConfig.TeamSize), slotStorageManager.SkillDataSlots, skillController);
        eventDispatcher.OnPhaseSkill += skillButtonView.RefreshButtonsByTurn;
        skillButtonView.RefreshButtonsByTurn(Team.Blue);

        gameFlowView.Init(slotStorageManager.ChampionDataSlots);
        skillController.OnUseSkill += gameFlowView.ViewTraitUseLog;

        eventDispatcher.OnPhaseSkill += (team) => scoreView.UpdateTeamScore(slotStorageManager.StatusSlots, team);
    }

    [SerializeField] GameObject scores;
    [SerializeField] TextMeshProUGUI textBlue;
    [SerializeField] TextMeshProUGUI textRed;
    public void ShowResult(MatchResult result)
    {
        scores.SetActive(true);
        textBlue.text = new ScoreTextBuilder().BuildText(result.BlueInfo);
        textRed.text = new ScoreTextBuilder().BuildText(result.RedInfo);
        print($"승자 : {result.Winner}");
    }
}
