using UnityEngine;
using UnityEngine.UI;

public class ChampionSelectUI_Controller : MonoBehaviour
{
    BanPickView view;
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionDrawer buttonDrawer;
    [SerializeField] ChampionView championFocusView;

    ChampionSelectPresenter championSelectPresenter = null;
    PhaseManager phaseManager;
    public void Init(ChampionSelectPresenter presenter, PhaseManager pm) // 팀을 아직 안받는 이유는 얘가 팀을 2개를 담당할 때가 있어서
    {
        gameObject.SetActive(true);
        view = GetComponentInChildren<BanPickView>();

        championSelectPresenter = presenter;
        phaseManager = pm;

        nailDownBtn.onClick.AddListener(NailDownChampion);
        buttonDrawer.DrawChampionButtons(SelectChampion);
        swapDoneBtn.gameObject.SetActive(false);
    }

    Button selectBtn;
    void SelectChampion(ChampionSO champion, Button button)
    {
        championSelectPresenter.SelectChamp(champion.Id);
        championFocusView.UpdateDisplay(champion);
        selectBtn = button;
    }

    void InActiveSelectButton()
    {
        selectBtn.enabled = false;
        var colors = selectBtn.colors;
        colors.normalColor = new Color(0.5f, 0.5f, 0.5f); // 회색 톤
        selectBtn.colors = colors;
        selectBtn = null;
    }

    [SerializeField] ChampionRepository championManager;
    void NailDownChampion() // 챔프 확정
    {
        championSelectPresenter.NailDownChampion(phaseManager.CurrentFlow);
        // view.UpdateSelectView(phaseManager.CurrentFlow.Phase, phaseManager.CurrentTurn, championManager.GetChampionData(selectId));
        phaseManager.SubmitAction(phaseManager.CurrentTurn);
        championFocusView.ClearDisplay();
        InActiveSelectButton();
    }

    // 나중에 따로 빠짐
    [SerializeField] Button swapDoneBtn;
    public void OnSwap(Team team)
    {
        buttonDrawer.gameObject.SetActive(false);
        swapDoneBtn.gameObject.SetActive(true);
        swapDoneBtn.onClick.AddListener(() => SwapDone(team));
        view.HideBan();
        nailDownBtn.gameObject.SetActive(false);
    }

    void SwapDone(Team team)
    {
        if (phaseManager.CurrentFlow.Phase == GamePhase.Swap)
        {
            phaseManager.SubmitAction(team);
            swapDoneBtn.gameObject.SetActive(false);
        }
    }
}