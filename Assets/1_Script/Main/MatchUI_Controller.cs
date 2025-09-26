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

    public void Init(GameBanPickStorage storage, PhaseManager phaseManager, SlotStatusChanger pickStatusChanger, PickTableRegistry pickTableRegistry)
    {
        swapController.Inject(phaseManager);
        championSelector.Init(new ChampionSelectPresenter(storage), phaseManager);
        championDrawer.DrawChampionButtons(championSelector.SelectChampion);

        pickStatusChanger.OnStatChanged += banPickView.ChangeChampionStat;
        storage.OnBan += banPickView.UpdateBanView;
        storage.OnPick += banPickView.UpdatePickView;

        phaseManager.OnPhaseSwap += swapController.Init;
        phaseManager.OnPhaseSwap += _ => banPickView.HideBan();
        phaseManager.OnPhaseSwap += _ => championDrawer.HideView();

        traitUseView.gameObject.SetActive(false);

        scoreView.Init(pickTableRegistry.Statuses);
        storage.OnPick += (team, id) => scoreView.UpdateTeamScore(team);
    }

    public void TraitUI_Init(Team team, PhaseManager phaseManager, TraitController traitController, PickTableRegistry pickFacade)
    {
        var presenter = new TraitUsePresenter(traitController, pickFacade.Champions);
        traitUseView.Init(presenter);
        phaseManager.OnPhaseTrait += traitUseView.UpdateTrait;
        traitUseView.UpdateTrait(team);

        traitController.OnTraitApplied += (x) => scoreView.UpdateTeamScore(x.Slot.Team);
        traitController.OnTraitApplied += banPickView.ChangeChampionStat;
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
