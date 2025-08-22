using System.Linq;
using UnityEngine;

public class PracticeUI : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    ChampionSelectionUI championSelectionUI;
    PlayerSectionUI playerSectionUI;
    int currentPracticePlayer = -1;

    void Start()
    {
        championSelectionUI = GetComponentInChildren<ChampionSelectionUI>(true);
        playerSectionUI = GetComponentInChildren<PlayerSectionUI>(true);
        championSelectionUI.gameObject.SetActive(false);
        playerSectionUI.gameObject.SetActive(true);

        playerSectionUI.DrawPlayerButton(DrawChampions, playerManager.Players.ToArray());
    }

    void DrawChampions(Player player)
    {
        playerSectionUI.gameObject.SetActive(false);
        currentPracticePlayer = player.Id;

        championSelectionUI.gameObject.SetActive(true);
        championSelectionUI.DrawChampionsButton(IncreasedMastery);
    }


    void IncreasedMastery(ChampionSO championSO)
    {
        playerSectionUI.gameObject.SetActive(true);
        playerManager.IncreasedMastery(currentPracticePlayer, championSO);
        currentPracticePlayer = -1;
        championSelectionUI.gameObject.SetActive(false);
        playerSectionUI.DrawPlayerButton(DrawChampions, playerManager.Players.ToArray());
    }
}
