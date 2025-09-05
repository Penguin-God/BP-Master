using UnityEngine;
using UnityEngine.UI;

public class BanPickUI : MonoBehaviour, IActionHandler
{
    BanPickView view;
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionDrawer buttonDrawer;

    ChampionSelectPresenter presenter = null;
    PhaseManager phaseManager;
    public void Init(GameBanPickStorage storage, PhaseManager pm) // 팀을 아직 안받는 이유는 얘가 팀을 2개를 담당할 때가 있어서
    {
        gameObject.SetActive(true);
        view = GetComponentInChildren<BanPickView>();

        presenter = new ChampionSelectPresenter(storage);
        phaseManager = pm;

        nailDownBtn.onClick.AddListener(NailDownChampion);
        buttonDrawer.DrawChampionButtons(SelectChampion);
    }

    public void SetActiveExcutor(ActiveExcuteManager activeExcuteManager) => activeExcutor = activeExcuteManager;

    void SelectChampion(ChampionSO champion)
    {
        presenter.SelectChamp(champion.Id);
        view.UpdateSelectChampion(champion);
    }

    void NailDownChampion() // 챔프 확정
    {
        int selectId = presenter.NailDownChampion(phaseManager.CurrentFlow);
        if (selectId == -1) return;

        view.UpdateSelectView(BanPickEnumCaster.PhaseToSelect(phaseManager.CurrentFlow.Phase), phaseManager.CurrentTurn, selectId);
        phaseManager.SubmitAction(phaseManager.CurrentTurn);
    }

    public void OnRequestBan(Team team) {}
    public void OnRequestPick(Team team) {}
    public void OnRequestSwap(Team team) => swapDoneBtn.onClick.AddListener(() => SwapDone(team));
    void SwapDone(Team team)
    {
        if (phaseManager.CurrentFlow.Phase == GamePhase.Swap)
            phaseManager.SubmitAction(team);
    }
    public void OnRequestActive(Team team) {}

    
    ActiveExcuteManager activeExcutor;
    // 버튼 플러그해서 사용
    public void Active(int index)
    {
        if (phaseManager.CurrentFlow.Phase != GamePhase.Active) return;

        // 나중을 위한 것
        // activeExcutor.DoActive(index, team);
        if (activeExcutor.IsTeamDone(phaseManager.CurrentTurn))
            phaseManager.SubmitAction(phaseManager.CurrentTurn);
    }

    [SerializeField] Button swapDoneBtn;
}
    