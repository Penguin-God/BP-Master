using UnityEngine;
using TMPro;

public class MatchUI_Controller : MonoBehaviour
{
    [SerializeField] ChampionSelectUI_Controller banPickUI;
    [SerializeField] TraitUseView traitUseView;
    [SerializeField] BanPickView banPickView;
    [SerializeField] ScoreView scoreView;
    [SerializeField] GameObject scores;
    [SerializeField] TextMeshProUGUI textBlue;
    [SerializeField] TextMeshProUGUI textRed;

    public void BindPhaseManager(PhaseManager phaseManager)
    {
        phaseManager.OnPhaseSwap += banPickUI.OnSwap;
        phaseManager.OnPhaseTrait += traitUseView.UpdateTrait;
    }

    public void InitTrait(TraitUsePresenter presenter, Team team)
    {
        traitUseView.Init(presenter);
        traitUseView.UpdateTrait(team);
    }

    public void BindTraitController(TraitController traitController, PhaseManager phaseManager)
    {
        traitController.OnTraitApplied += banPickView.ChangeChampionStat;
        traitController.OnTraitApplied += x => scoreView.UpdateTeamScore(x.Slot.Team);

        var presenter = new TraitUsePresenter(traitController);
        presenter.OnTraitUsed += phaseManager.SubmitAction;
        InitTrait(presenter, Team.Blue); // 기본 초기화
    }

    public void InitScoreView(SlotStorage<Champion> pickSlotStorage)
    {
        scoreView.Init(pickSlotStorage);
        scoreView.UpdateTeamScore(Team.Blue);
        scoreView.UpdateTeamScore(Team.Red);
    }

    public void ShowResult(MatchResult result)
    {
        scores.SetActive(true);
        textBlue.text = new ScorePresenter().BuildText(result.BlueInfo);
        textRed.text = new ScorePresenter().BuildText(result.RedInfo);
        Debug.Log($"승자 : {result.Winner}");
    }
}
