using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BanPcikPhase { Ban = 0, Pick = 1, Swap = 2, Done = 3 }

public class BanPickUI : MonoBehaviour
{
    [SerializeField] DraftTurnSO banTurnSO;
    [SerializeField] DraftTurnSO pickTurnSO;
    [SerializeField] Button championSelectionBtn;

    [SerializeField] BanPcikPhase currentPhase = BanPcikPhase.Ban;

    [SerializeField] TextMeshProUGUI[] bluePicks;
    [SerializeField] TextMeshProUGUI[] redPicks;
    readonly Dictionary<Team, TextMeshProUGUI[]> pickDict = new();

    [SerializeField] ChampionSO currentSelectChampion = null;
    [SerializeField] TextMeshProUGUI selectChampionTxt;
    [SerializeField] ChampionSelectionUI championSelectionUI;

    BanPickManager BanPickManager;
    void Start()
    {
        BanPickManager = new BanPickManager(banTurnSO, pickTurnSO);

        pickDict.Add(Team.Blue, bluePicks);
        pickDict.Add(Team.Red, redPicks);
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
        if (currentSelectChampion == null) return;

        if(BanPickManager.TrySelect(currentSelectChampion, out SelectData data))
        {
            if(data.BanPcikPhase == BanPcikPhase.Pick)
                pickDict[data.Team][data.Count - 1].text = data.Champion.ChampionName;
        }
    }
}
