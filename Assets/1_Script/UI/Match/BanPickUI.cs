using UnityEngine;
using UnityEngine.UI;

public class BanPickUI : MonoBehaviour, IActionHandler
{
    BanPickView view;
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionDrawer buttonDrawer;

    ChampionSO currentSelectChampion = null;

    public void Init(GameBanPickStorage storage, ActionEventBus bus) // 팀을 아직 안받는 이유는 얘가 팀을 2개를 담당할 때가 있어서
    {
        gameObject.SetActive(true);
        view = GetComponentInChildren<BanPickView>();
        _storage = storage;
        _bus = bus;

        nailDownBtn.onClick.AddListener(NailDownChampion);
        buttonDrawer.DrawChampionButtons(SelectChampion);
    }

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

        Team prevTeam = team; // 명령시 team변수 갱신되서
        if (currentPhase == GamePhase.Ban)
        {
            if(_storage.SaveSelect(new SelectInfo(team, SelectType.Ban, currentSelectChampion.Id)))
            {
                view.UpdateBanView(prevTeam, currentSelectChampion.Id);
                _bus.ActionDone(team);
            }
        }
        else if (currentPhase == GamePhase.Pick)
        {
            if (_storage.SaveSelect(new SelectInfo(team, SelectType.Pick, currentSelectChampion.Id)))
            {
                view.UpdatePickView(prevTeam, currentSelectChampion.Id);
                _bus.ActionDone(team);
            }
        }
    }

    ActionEventBus _bus;
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
        swapDoneBtn.onClick.AddListener(() => _bus.ActionDone(team));
    }

    public void OnRequestActive(Team team)
    {
        throw new System.NotImplementedException();
    }

    [SerializeField] Button swapDoneBtn;
}
    