using TMPro;
using UnityEngine;

public class MatchResultView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI blueScoreText;
    [SerializeField] TextMeshProUGUI redScoreText;
    [SerializeField] TextMeshProUGUI winnerText;
    public void ShowResult(MatchResult result)
    {
        blueScoreText.text = new ScoreTextBuilder().BuildText(result.BlueInfo);
        redScoreText.text = new ScoreTextBuilder().BuildText(result.RedInfo);
        winnerText.text = result.Winner.ToString();
    }
}