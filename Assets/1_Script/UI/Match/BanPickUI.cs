using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BanPickUI : MonoBehaviour, ISelectWait, ISelector
{
    BanPickView view;
    [SerializeField] BanPickController banPickController;
    [SerializeField] Button championSelectionBtn;
    [SerializeField] ChampionDrawer buttonDrawer;

    ChampionSO currentSelectChampion = null;


    void Start()
    {
        view = GetComponentInChildren<BanPickView>();
        banPickController.OnSelectedChampion += view.UpdatePickChampions;
        championSelectionBtn.onClick.AddListener(NailDownChampion);
        buttonDrawer.DrawChampionButtons(SelectChampion);
    }

    void SelectChampion(ChampionSO champion)
    {
        currentSelectChampion = champion;
        view.UpdateSelectChampion(champion);
    }

    void NailDownChampion() // 챔프 확정
    {
        if (currentSelectChampion != null)
            isSelect = true;
    }

    bool isSelect;
    public IEnumerator WaitSelect()
    {
        yield return new WaitUntil(() =>  isSelect);
        isSelect = false;
    }

    int ISelector.SelectChampion() => currentSelectChampion.Id;
}
