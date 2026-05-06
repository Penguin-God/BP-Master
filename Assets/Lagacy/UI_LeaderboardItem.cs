using TMPro;
using UnityEngine;

public class UI_LeaderboardItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] TextMeshProUGUI loseText;
    [SerializeField] TextMeshProUGUI scoreText;

    public void Bind(LeaderboardDisplayModel model)
    {
        rankText.text = model.Rank.ToString();
        nameText.text = model.TeamName;
        winText.text = model.WinText;
        loseText.text = model.LoseText;
        scoreText.text = model.ScoreText;
    }
}