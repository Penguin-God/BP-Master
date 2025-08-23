using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BanPickUI : MonoBehaviour, ISelectWait, ISelector, IDraftActionHandler
{
    BanPickView view;
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionDrawer buttonDrawer;

    ChampionSO currentSelectChampion = null;

    public void Init()
    {
        gameObject.SetActive(true);
        view = GetComponentInChildren<BanPickView>();
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

        Team prevTeam = team; // 명령시 team변수 갱신되서
        if (currentPhase == GamePhase.Ban)
        {
            if(draftAction.Ban(team, currentSelectChampion.Id))
                view.UpdateBanView(prevTeam, currentSelectChampion.Id);
        }
        else if (currentPhase == GamePhase.Pick)
        {
            if(draftAction.Pick(team, currentSelectChampion.Id))
                view.UpdatePickView(prevTeam, currentSelectChampion.Id);
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


    [SerializeField] Button swapDoneBtn;
    public void OnRequestSwap(Team team, DraftActionController draftAction)
    {
        swapDoneBtn.onClick.AddListener(() => draftAction.SwapDone(team));
    }
}
    