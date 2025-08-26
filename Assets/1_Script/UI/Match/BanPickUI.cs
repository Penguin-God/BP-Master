using UnityEngine;
using UnityEngine.UI;

public class BanPickUI : MonoBehaviour, IActionHandler
{
    BanPickView view;
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionDrawer buttonDrawer;

    ChampionSO currentSelectChampion = null;

    public void Init(GameBanPickStorage storage)
    {
        gameObject.SetActive(true);
        view = GetComponentInChildren<BanPickView>();
        _storage = storage;

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
                bus.ActionDone(team);
            }
        }
        else if (currentPhase == GamePhase.Pick)
        {
            if (_storage.SaveSelect(new SelectInfo(team, SelectType.Pick, currentSelectChampion.Id)))
            {
                view.UpdatePickView(prevTeam, currentSelectChampion.Id);
                bus.ActionDone(team);
            }
        }
    }

    //public void OnRequestBan(Team team, DraftActionController draftAction)
    //{
    //    this.team = team;
    //    currentPhase = GamePhase.Ban;
    //    this.draftAction = draftAction;
    //}

    //public void OnRequestPick(Team team, DraftActionController draftAction)
    //{
    //    this.team = team;
    //    currentPhase = GamePhase.Pick;
    //    this.draftAction = draftAction;
    //}

    //public void OnRequestSwap(Team team, DraftActionController draftAction)
    //{
    //    swapDoneBtn.onClick.AddListener(() => draftAction.SwapDone(team));
    //}
    ActionEventBus bus;
    public void OnRequestBan(Team team, ActionEventBus draftAction)
    {
        this.team = team;
        bus = draftAction;
        currentPhase = GamePhase.Ban;
    }

    public void OnRequestPick(Team team, ActionEventBus draftAction)
    {
        this.team = team;
        bus = draftAction;
        currentPhase = GamePhase.Pick;
    }

    public void OnRequestSwap(Team team, ActionEventBus draftAction)
    {
        bus = draftAction;
        swapDoneBtn.onClick.AddListener(() => bus.ActionDone(team));
    }

    [SerializeField] Button swapDoneBtn;
}
    