using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BanPickUI : MonoBehaviour, ISelectWait, ISelector
{
    BanPickView view;
    [SerializeField] BanPickController banPickController;
    [SerializeField] Button championSelectionBtn;

    [SerializeField] ChampionSO currentSelectChampion = null;
    [SerializeField] ChampionSelectionUI championSelectionUI;
    [SerializeField] GameObject teamChoiceParent;
    [SerializeField] Button blueButton;
    [SerializeField] Button redButton;

    void Start()
    {
        view = GetComponentInChildren<BanPickView>();
        banPickController.OnSelectedChampion += view.UpdatePickChampions;
        championSelectionBtn.onClick.AddListener(NailDownChampion);
        championSelectionUI.DrawChampionsButton(SelectChampion);

        blueButton.onClick.AddListener(() => ChoiceTeam(Team.Blue));
        redButton.onClick.AddListener(() => ChoiceTeam(Team.Red));
    }

    void ChoiceTeam(Team team)
    {
        teamChoiceParent.SetActive(false);
        banPickController.ChioceTeam(team);
    }

    void SelectChampion(ChampionSO champion)
    {
        currentSelectChampion = champion;
        view.UpdateSelectChampion(champion);
    }

    void NailDownChampion()
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
