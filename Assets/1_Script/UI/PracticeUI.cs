using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PracticeUI : MonoBehaviour
{
    [SerializeField] ProGamerLoder playerManager;
    ChampionButtonView championSelectionUI;
    PlayerSectionUI playerSectionUI;
    int currentPracticePlayer = -1;

    void Start()
    {
        championSelectionUI = GetComponentInChildren<ChampionButtonView>(true);
        playerSectionUI = GetComponentInChildren<PlayerSectionUI>(true);
        championSelectionUI.gameObject.SetActive(false);
        playerSectionUI.gameObject.SetActive(true);

        playerSectionUI.DrawPlayerButton(DrawChampions, playerManager.Players.ToArray());
    }

    void DrawChampions(ProGamerDel player)
    {
        playerSectionUI.gameObject.SetActive(false);
        currentPracticePlayer = player.Id;

        championSelectionUI.gameObject.SetActive(true);
        championSelectionUI.CreateButtons();
    }


    void IncreasedMastery(ChampionSO championSO, Button btn)
    {
        playerSectionUI.gameObject.SetActive(true);
        playerManager.IncreasedMastery(currentPracticePlayer, championSO);
        currentPracticePlayer = -1;
        championSelectionUI.gameObject.SetActive(false);
        playerSectionUI.DrawPlayerButton(DrawChampions, playerManager.Players.ToArray());
    }
}
