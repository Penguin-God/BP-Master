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

    public void Init(GameBanPickStorage storage, PhaseManager phaseManager, ChampionStorageFactory factory)
    {
        swapController.Inject(phaseManager, storage);
        championSelector.Init(new ChampionSelectPresenter(storage), phaseManager);
        championDrawer.DrawChampionButtons(championSelector.SelectChampion);

        storage.OnBan += banPickView.UpdateBanView;
        storage.OnPick += banPickView.UpdatePickView;

        phaseManager.OnPhaseSwap += swapController.Init;
        phaseManager.OnPhaseSwap += _ => banPickView.HideBan();
        phaseManager.OnPhaseSwap += _ => championDrawer.HideView();

        traitUseView.gameObject.SetActive(false);

        storage.OnPick += (team, id) => scoreView.UpdateTeamScore(factory.CreateStatusStorage(storage.PickIds), team);
    }

    public void TraitUI_Init(Team team, PhaseManager phaseManager, TraitController traitController, SlotStorage<Champion> champions, SlotStorage<ChampionStatus> status)
    {
        banPickView.BindStatChangeEvent(status);
        var presenter = new TraitUsePresenter(traitController, champions);
        traitUseView.Init(presenter);
        phaseManager.OnPhaseTrait += traitUseView.UpdateTrait;
        traitUseView.UpdateTrait(team);

        phaseManager.OnPhaseTrait += (team) => scoreView.UpdateTeamScore(status, team);
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
