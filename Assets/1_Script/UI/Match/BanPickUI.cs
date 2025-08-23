using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BanPickUI : MonoBehaviour, ISelectWait, ISelector, IDraftActionHandler
{
    BanPickView view;
    [SerializeField] BanPickController banPickController;
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionDrawer buttonDrawer;

    ChampionSO currentSelectChampion = null;

    void Start()
    {
        view = GetComponentInChildren<BanPickView>();
        // banPickController.OnSelectedChampion += view.UpdatePickChampions;
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
    DraftActionController draftAction;
    void NailDownChampion() // 챔프 확정
    {
        if (currentSelectChampion == null) return;

        if (currentPhase == GamePhase.Ban)
        {
            view.UpdateBanView(team, currentSelectChampion.Id); // 순서 커플링
            draftAction.Ban(team, currentSelectChampion.Id);
        }
        else if (currentPhase == GamePhase.Pick)
        {
            view.UpdatePickView(team, currentSelectChampion.Id);
            draftAction.Pick(team, currentSelectChampion.Id);
        }
    }

    bool isSelect;
    public IEnumerator WaitSelect()
    {
        yield return new WaitUntil(() =>  isSelect);
        isSelect = false;
    }

    int ISelector.SelectChampion() => currentSelectChampion.Id;

    public void OnRequestBan(Team team, DraftActionController draftAction)
    {
        this.team = team;
        currentPhase = GamePhase.Ban;
        this.draftAction = draftAction;
    }

    public void OnRequestPick(Team team, DraftActionController draftAction)
    {
        this.team = team;
        currentPhase = GamePhase.Pick;
        this.draftAction = draftAction;
    }

    public void OnRequestSwap(Team team, DraftActionController draftAction)
    {
        this.team = team;
        currentPhase = GamePhase.Swap;
        this.draftAction = draftAction;
    }
}
