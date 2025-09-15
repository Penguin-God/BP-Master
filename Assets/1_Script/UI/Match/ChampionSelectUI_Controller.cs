using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampionSelectUI_Controller : MonoBehaviour
{
    BanPickView view;
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionDrawer buttonDrawer;
    [SerializeField] TextMeshProUGUI selectChampionTxt;
    [SerializeField] ChampionView championFocusView;

    ChampionSelectPresenter championSelectPresenter = null;
    PhaseManager phaseManager;
    public void Init(GameBanPickStorage storage, PhaseManager pm) // 팀을 아직 안받는 이유는 얘가 팀을 2개를 담당할 때가 있어서
    {
        gameObject.SetActive(true);
        view = GetComponentInChildren<BanPickView>();

        championSelectPresenter = new ChampionSelectPresenter(storage);
        phaseManager = pm;

        nailDownBtn.onClick.AddListener(NailDownChampion);
        buttonDrawer.DrawChampionButtons(SelectChampion, FocusChamp, FocusExitChamp);
        swapDoneBtn.gameObject.SetActive(false);
    }

    void FocusChamp(ChampionSO championSO) => championFocusView.UpdateDisplay(championSO);
    void FocusExitChamp(ChampionSO championSO) => championFocusView.ClearDisplay();

    void SelectChampion(ChampionSO champion)
    {
        championSelectPresenter.SelectChamp(champion.Id);
        selectChampionTxt.text = champion.ChampionName;
    }

    [SerializeField] ChampionManagerMono championManager;
    void NailDownChampion() // 챔프 확정
    {
        int selectId = championSelectPresenter.NailDownChampion(phaseManager.CurrentFlow);
        if (selectId == -1) return;

        view.UpdateSelectView(phaseManager.CurrentFlow.Phase, phaseManager.CurrentTurn, championManager.GetChampionData(selectId));
        phaseManager.SubmitAction(phaseManager.CurrentTurn);
        selectChampionTxt.text = string.Empty;
    }

    // 나중에 따로 빠짐
    [SerializeField] Button swapDoneBtn;
    public void OnSwap(Team team)
    {
        swapDoneBtn.gameObject.SetActive(true);
        swapDoneBtn.onClick.AddListener(() => SwapDone(team));
    }

    void SwapDone(Team team)
    {
        if (phaseManager.CurrentFlow.Phase == GamePhase.Swap)
            phaseManager.SubmitAction(team);
    }
}