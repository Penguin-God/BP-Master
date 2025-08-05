using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BanPcikPhase { Ban = 0, Pick = 1, Swap = 2, Done = 3 }

// 세팅, view까지 얘 역할이 많다
public class BanPickUI : MonoBehaviour, IBanPickAgent
{
    [SerializeField] BanPickController banPickController;
    [SerializeField] Button championSelectionBtn;

    [SerializeField] TextMeshProUGUI[] bluePicks;
    [SerializeField] TextMeshProUGUI[] redPicks;
    readonly Dictionary<Team, TextMeshProUGUI[]> pickTextDict = new();

    [SerializeField] ChampionSO currentSelectChampion = null;
    [SerializeField] TextMeshProUGUI selectChampionTxt;
    [SerializeField] ChampionSelectionUI championSelectionUI;

    void Start()
    {
        pickTextDict.Add(Team.Blue, bluePicks);
        pickTextDict.Add(Team.Red, redPicks);
        banPickController.OnSelectedChampion += DrawUI;
        championSelectionBtn.onClick.AddListener(NailDownChampion);
        championSelectionUI.DrawChampionsButton(SelectChampion);
    }

    void SelectChampion(ChampionSO champion)
    {
        selectChampionTxt.text = champion.ChampionName;
        currentSelectChampion = champion;
    }

    void NailDownChampion()
    {
        if (currentSelectChampion != null)
            isSelect = true;
    }

    void DrawUI(SelectData data)
    {
        if (data.BanPcikPhase == BanPcikPhase.Pick)
            pickTextDict[data.Team][data.Count - 1].text = data.Champion.ChampionName;
    }

    bool isSelect;
    public IEnumerator WaitSelect()
    {
        yield return new WaitUntil(() =>  isSelect);
        isSelect = false;
    }

    public ChampionSO SelectChampion() => currentSelectChampion;
}
