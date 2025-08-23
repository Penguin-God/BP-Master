using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BanPickUI : MonoBehaviour, ISelectWait, ISelector, IActionHandler
{
    BanPickView view;
    [SerializeField] BanPickController banPickController;
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionDrawer buttonDrawer;

    ChampionSO currentSelectChampion = null;

    void Start()
    {
        view = GetComponentInChildren<BanPickView>();
        banPickController.OnSelectedChampion += view.UpdatePickChampions;
        nailDownBtn.onClick.AddListener(NailDownChampion);
        buttonDrawer.DrawChampionButtons(SelectChampion);
    }

    void SelectChampion(ChampionSO champion)
    {
        currentSelectChampion = champion;
        view.UpdateSelectChampion(champion);
    }

    Team team;
    void NailDownChampion() // 챔프 확정
    {
        if (currentSelectChampion == null) return;

        if (currentPhase == GamePhase.Ban)
        {
            draftAction.Ban(team, currentSelectChampion.Id);
            view.UpdateBanView(team, currentSelectChampion.Id);
        }
        else if(currentPhase == GamePhase.Pick) draftAction.Pick(team, currentSelectChampion.Id);
    }

    bool isSelect;
    public IEnumerator WaitSelect()
    {
        yield return new WaitUntil(() =>  isSelect);
        isSelect = false;
    }

    int ISelector.SelectChampion() => currentSelectChampion.Id;

    public void OnRequestAction(DraftActionController draftAction, GamePhase phase)
    {
        this.draftAction = draftAction;
        currentPhase = phase;
        
        switch (phase)
        {
            case GamePhase.Ban:
            case GamePhase.Pick:
                nailDownBtn.gameObject.SetActive(true);
                break;
            case GamePhase.Swap:  break;
        }
    }

    GamePhase currentPhase;
    DraftActionController draftAction;
}
