using UnityEngine;
using UnityEngine.UI;

public class BanPickUI : MonoBehaviour, IActionHandler
{
    BanPickView view;
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionDrawer buttonDrawer;
    [SerializeField] Button swapDoneBtn;
    
    ChampionSelectPresenter championSelectPresenter = null;
    PhaseManager phaseManager;
    public void Init(GameBanPickStorage storage, PhaseManager pm) // 팀을 아직 안받는 이유는 얘가 팀을 2개를 담당할 때가 있어서
    {
        gameObject.SetActive(true);
        view = GetComponentInChildren<BanPickView>();

        championSelectPresenter = new ChampionSelectPresenter(storage);
        phaseManager = pm;

        nailDownBtn.onClick.AddListener(NailDownChampion);
        buttonDrawer.DrawChampionButtons(SelectChampion);
    }

    void SelectChampion(ChampionSO champion)
    {
        championSelectPresenter.SelectChamp(champion.Id);
        view.UpdateSelectChampion(champion);
    }

    [SerializeField] ChampionManagerMono championManager;
    void NailDownChampion() // 챔프 확정
    {
        int selectId = championSelectPresenter.NailDownChampion(phaseManager.CurrentFlow);
        if (selectId == -1) return;

        view.UpdateSelectView(phaseManager.CurrentFlow.Phase, phaseManager.CurrentTurn, championManager.GetChampionData(selectId));
        phaseManager.SubmitAction(phaseManager.CurrentTurn);
    }

    public void OnRequestSwap(Team team) => swapDoneBtn.onClick.AddListener(() => SwapDone(team));
    void SwapDone(Team team)
    {
        if (phaseManager.CurrentFlow.Phase == GamePhase.Swap)
            phaseManager.SubmitAction(team);
    }
}