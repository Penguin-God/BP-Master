using TMPro;
using UnityEngine;

public class MatchUI_Controller : MonoBehaviour
{
    [SerializeField] ChampionSelector_UI championSelector;
    [SerializeField] ChampionButtonView championDrawer;
    [SerializeField] TraitUseController traitUseView;
    [SerializeField] BanPickView banPickView;
    [SerializeField] ScoreView scoreView;
    [SerializeField] SwapController swapController;
    [SerializeField] GameFlowView gameFlowView;
    [SerializeField] TraitButtonView traitButtonView;
    [SerializeField] ChampionRepository championRepository;

    public void Init(GameBanPickStorage storage, PhaseManager phaseManager, PhaseEventDispatcher eventDispatcher)
    {
        banPickView.ViewMastery();
        swapController.Inject(phaseManager, storage);
        championSelector.Init(new ChampionSelectPresenter(storage), phaseManager);

        storage.OnBan += banPickView.UpdateBanView;
        storage.OnPick += banPickView.UpdatePickView;

        storage.OnBan += (team, id) => championDrawer.InActiveButton(id);
        storage.OnPick += (team, id) => championDrawer.InActiveButton(id);

        eventDispatcher.OnPhaseSwap += swapController.Init;
        eventDispatcher.OnPhaseSwap += _ => banPickView.HideBan();
        eventDispatcher.OnPhaseSwap += _ => championDrawer.HideView();

        traitUseView.gameObject.SetActive(false);

        storage.OnPick += (team, id) => scoreView.UpdateTeamScore(IdToStatus(storage.PickIds), team);

        eventDispatcher.OnGameProgress += gameFlowView.ViewGameFlow;
    }

    SlotStorage<ChampionStatus> IdToStatus(SlotStorage<int> idStorage) 
        => StorageConverter.ConvertStorage(idStorage, id => new ChampionStatus(championRepository.GetChampionData(id).StatData, TraitType.None));

    public void TraitUI_Init(Team playerTeam, PhaseEventDispatcher eventDispatcher, SkillUseController traitUseFacade, SlotStorageManager slotStorageManager, TraitSlotFilter filter)
    {
        banPickView.BindStatChangeEvent(slotStorageManager.StatusSlots);
        
        traitButtonView.Init(filter, playerTeam);
        traitUseView.Init(new TraitUsePersenter(traitUseFacade, 5, slotStorageManager.SkillSlots), slotStorageManager.SkillSlots);
        eventDispatcher.OnPhaseSkill += traitButtonView.RefreshButtonsByTurn;
        traitButtonView.RefreshButtonsByTurn(Team.Blue);

        gameFlowView.Init(slotStorageManager.ChampionDataSlots);
        traitUseFacade.OnUseSkill += gameFlowView.ViewTraitUseLog;

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
