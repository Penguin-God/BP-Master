using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public void ShowResult(MatchResult result, bool isGameEnd)
    {
        blueScoreText.text = new ScoreTextBuilder().BuildText(result.BlueInfo);
        redScoreText.text = new ScoreTextBuilder().BuildText(result.RedInfo);
        winnerText.text = $"승리 : {result.Winner.ToString()}";

        if (isGameEnd) return;
        newGameButton.gameObject.SetActive(true);
        newGameButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }
}