using TMPro;
using UnityEngine;

public class MatchUI_Controller : MonoBehaviour
{
    [SerializeField] ChampionSelectUI_Controller banPickUI;
    [SerializeField] TraitUseView traitUseView;
    [SerializeField] BanPickView banPickView;
    [SerializeField] ScoreView scoreView;

    public void Init(GameBanPickStorage storage, PhaseManager phaseManager, PickStatusChanger pickStatusChanger)
    {
        banPickUI.Init(new ChampionSelectPresenter(storage), phaseManager);
        pickStatusChanger.OnStatChanged += banPickView.ChangeChampionStat;
        storage.OnBan += banPickView.UpdateBanView;
        storage.OnPick += banPickView.UpdatePickView;
        phaseManager.OnPhaseSwap += banPickUI.OnSwap;
        traitUseView.gameObject.SetActive(false);
    }

    public void TraitUI_Init(Team team, PhaseManager phaseManager, TraitController traitController, PickTableRegistry pickFacade)
    {
        var presenter = new TraitUsePresenter(traitController, pickFacade.Champions);
        traitUseView.Init(presenter);
        phaseManager.OnPhaseTrait += traitUseView.UpdateTrait;
        traitUseView.UpdateTrait(Team.Blue);

        scoreView.Init(pickFacade.Statuses);
        scoreView.UpdateTeamScore(Team.Blue);
        scoreView.UpdateTeamScore(Team.Red);
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
