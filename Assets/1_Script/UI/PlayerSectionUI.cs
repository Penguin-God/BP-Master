using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerSectionUI : MonoBehaviour
{
    [SerializeField] GameObject championBtn;

    public void DrawPlayerButton(UnityAction<Player> onclick, Player[] players)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach (var player in players)
        {
            var btn = Instantiate(championBtn, transform).GetComponent<Button>();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = BuildMasteryText(player);
            btn.onClick.AddListener(() => onclick(player));
        }
    }

    string BuildMasteryText(Player player)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Player: {player.PlayerName}");

        var mastered = player
                       .AllMasterys
                       .OrderByDescending(m => m.level);
        foreach (var mastery in mastered)
        {
            string champLabel = mastery.Champion.ChampionName;
            sb.AppendLine($"{champLabel}  —  Lv {mastery.level}");
        }

        return sb.ToString();
    }
}
