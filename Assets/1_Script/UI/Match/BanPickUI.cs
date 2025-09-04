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
        int selectId = presenter.NailDownChampion();
        if (selectId == -1) return;

        view.UpdateSelectView(BanPickEnumCaster.PhaseToSelect(presenter.Phase), presenter.Turn, selectId);
        phaseManager.SubmitAction(presenter.Turn);
    }

    void ChangeFlow(Team team, GamePhase phase) => presenter.ChangeFlow(new GameFlowData(phase, team));
    public void OnRequestBan(Team team) => ChangeFlow(team, GamePhase.Ban);
    public void OnRequestPick(Team team) => ChangeFlow(team, GamePhase.Pick);
    public void OnRequestSwap(Team team)
    {
        swapDoneBtn.onClick.AddListener(() => phaseManager.SubmitAction(team));
    }
    public void OnRequestActive(Team team) => ChangeFlow(team, GamePhase.Active);

    
    ActiveExcuteManager activeExcutor;
    // 버튼 플러그해서 사용
    public void Active(int index)
    {
        if (presenter.Phase != GamePhase.Active) return;

        // 나중을 위한 것
        // activeExcutor.DoActive(index, team);
        if (activeExcutor.IsTeamDone(presenter.Turn))
            phaseManager.SubmitAction(presenter.Turn);
    }

    [SerializeField] Button swapDoneBtn;
}
    