using TMPro;
using UnityEngine;

public class MatchUI_Controller : MonoBehaviour
{
    [SerializeField] ChampionSelector_UI championSelector;
    [SerializeField] ChampionButtonView championDrawer;
    [SerializeField] TraitUseView traitUseView;
    [SerializeField] BanPickView banPickView;
    [SerializeField] ScoreView scoreView;
    [SerializeField] SwapController swapController;
    [SerializeField] GameFlowView gameFlowView;
    [SerializeField] TraitButtonView traitButtonView;

    public void Init(GameBanPickStorage storage, PhaseManager phaseManager, IdStorageConverter factory, PhaseEventDispatcher eventDispatcher)
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

        storage.OnPick += (team, id) => scoreView.UpdateTeamScore(factory.IdToStatus(storage.PickIds), team);

        eventDispatcher.OnGameProgress += gameFlowView.ViewGameFlow;
    }

    public void TraitUI_Init(Team playerTeam, PhaseEventDispatcher eventDispatcher, TraitUseFacade traitUseFacade, SlotStorage<Champion> champions, SlotStorage<ChampionStatus> status, TraitSlotFilter filter)
    {
        banPickView.BindStatChangeEvent(status);
        var presenter = new TraitUsePresenter(playerTeam);

        traitButtonView.Init(filter);
        traitUseView.Init(presenter, traitUseFacade, ChampionStorageConverter.ChamptionToTrait(champions));
        eventDispatcher.OnPhaseTrait += traitUseView.UpdateTrait;
        traitUseView.Set(playerTeam);

        gameFlowView.Init(champions);
        traitUseFacade.OnTraitUsed += gameFlowView.ViewTraitUseLog;

        eventDispatcher.OnPhaseTrait += (team) => scoreView.UpdateTeamScore(status, team);
    }

    [SerializeField] GameObject scores;
    [SerializeField] TextMeshProUGUI textBlue;
    [SerializeField] TextMeshProUGUI textRed;
    public void ShowResult(MatchResult result)
    {
        scores.SetActive(true);
        textBlue.text = new ScorePresenter().BuildText(result.BlueInfo);
        textRed.text = new ScorePresenter().BuildText(result.RedInfo);
        print($"승자 : {result.Winner}");
    }
}
