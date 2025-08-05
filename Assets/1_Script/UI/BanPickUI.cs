using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BanPcikPhase { Ban = 0, Pick = 1, Swap = 2, Done = 3 }

public class BanPickUI : MonoBehaviour, IBanPickAgent
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

    public ChampionSO SelectChampion() => currentSelectChampion;
}
