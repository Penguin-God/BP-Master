using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchResultView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI blueScoreText;
    [SerializeField] TextMeshProUGUI redScoreText;
    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] Button newGameButton;

    void Awake()
    {
        newGameButton.gameObject.SetActive(false);
    }

    public void ShowResult(MatchResult result, MatchManager matchManager)
    {
        blueScoreText.text = new ScoreTextBuilder().BuildText(result.BlueInfo);
        redScoreText.text = new ScoreTextBuilder().BuildText(result.RedInfo);
        winnerText.text = $"승리 : {result.Winner.ToString()}";

        if (matchManager.Record.IsMatchFinished) return;
        newGameButton.gameObject.SetActive(true);
        newGameButton.onClick.AddListener(() => matchManager.HandleRoundEnd(result.Winner));
    }
}