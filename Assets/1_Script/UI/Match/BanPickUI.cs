using UnityEngine;
using UnityEngine.UI;

public class BanPickUI : MonoBehaviour, IActionHandler
{
    BanPickView view;
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionDrawer buttonDrawer;

    ChampionSO currentSelectChampion = null;

    public void Init(GameBanPickStorage storage, PhaseManager pm) // 팀을 아직 안받는 이유는 얘가 팀을 2개를 담당할 때가 있어서
    {
        gameObject.SetActive(true);
        view = GetComponentInChildren<BanPickView>();
        _storage = storage;
        phaseManager = pm;

        nailDownBtn.onClick.AddListener(NailDownChampion);
        buttonDrawer.DrawChampionButtons(SelectChampion);
    }

    public void SetActiveExcutor(ActiveExcuteManager activeExcuteManager) => activeExcutor = activeExcuteManager;

    void SelectChampion(ChampionSO champion)
    {
        currentSelectChampion = champion;
        view.UpdateSelectChampion(champion);
    }

    Team team;
    GamePhase currentPhase;
    GameBanPickStorage _storage;
    void NailDownChampion() // 챔프 확정
    {
        if (currentSelectChampion == null) return;

        if (currentPhase == GamePhase.Ban)
        {
            if(_storage.SaveSelect(new SelectInfo(team, SelectType.Ban, currentSelectChampion.Id)))
            {
                view.UpdateBanView(team, currentSelectChampion.Id);
                phaseManager.SubmitAction(team);
            }
        }
        else if (currentPhase == GamePhase.Pick)
        {
            if (_storage.SaveSelect(new SelectInfo(team, SelectType.Pick, currentSelectChampion.Id)))
            {
                view.UpdatePickView(team, currentSelectChampion.Id);
                phaseManager.SubmitAction(team);
            }
        }
    }

    PhaseManager phaseManager;
    public void OnRequestBan(Team team)
    {
        this.team = team;
        currentPhase = GamePhase.Ban;
    }

    public void OnRequestPick(Team team)
    {
        this.team = team;
        currentPhase = GamePhase.Pick;
    }

    public void OnRequestSwap(Team team)
    {
        swapDoneBtn.onClick.AddListener(() => phaseManager.SubmitAction(team));
    }

    ActiveExcuteManager activeExcutor;
    public void OnRequestActive(Team team)
    {
        this.team = team;
        currentPhase = GamePhase.Active;
    }

    // 버튼 플러그해서 사용
    public void Active(int index)
    {
        if (currentPhase != GamePhase.Active) return;

        // 나중을 위한 것
        // activeExcutor.DoActive(index, team);
        if (activeExcutor.IsTeamDone(team))
            phaseManager.SubmitAction(team);
    }

    [SerializeField] Button swapDoneBtn;
}
    