using TMPro;
using UnityEngine;

public class MatchUI_Controller : MonoBehaviour
{
    [SerializeField] ChampionSelector_UI championSelector;
    [SerializeField] ChampionDrawer championDrawer;
    [SerializeField] TraitUseView traitUseView;
    [SerializeField] BanPickView banPickView;
    [SerializeField] ScoreView scoreView;
    [SerializeField] SwapController swapController;

    public void Init(GameBanPickStorage storage, PhaseManager phaseManager, StorageConverter factory, PhaseEventDispatcher eventDispatcher)
    {
        banPickView.ViewMastery();
        swapController.Inject(phaseManager, storage);
        championSelector.Init(new ChampionSelectPresenter(storage), phaseManager);
        championDrawer.DrawChampionButtons(championSelector.SelectChampion);

        storage.OnBan += banPickView.UpdateBanView;
        storage.OnPick += banPickView.UpdatePickView;

        eventDispatcher.OnPhaseSwap += swapController.Init;
        eventDispatcher.OnPhaseSwap += _ => banPickView.HideBan();
        eventDispatcher.OnPhaseSwap += _ => championDrawer.HideView();

        traitUseView.gameObject.SetActive(false);

        storage.OnPick += (team, id) => scoreView.UpdateTeamScore(factory.CreateStatusStorage(storage.PickIds), team);
    }

    public void TraitUI_Init(Team team, PhaseEventDispatcher eventDispatcher, TraitUseFacade traitController, SlotStorage<Champion> champions, SlotStorage<ChampionStatus> status)
    {
        banPickView.BindStatChangeEvent(status);
        var presenter = new TraitUsePresenter(traitController, champions);
        traitUseView.Init(presenter);
        eventDispatcher.OnPhaseTrait += traitUseView.UpdateTrait;
        traitUseView.UpdateTrait(team);

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
