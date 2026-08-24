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

    public void ShowResult(MatchResult result, bool isGameEnd)
    {
        blueScoreText.text = new ScoreTextBuilder().BuildText(result.BlueInfo);
        redScoreText.text = new ScoreTextBuilder().BuildText(result.RedInfo);
        winnerText.text = $"승리 : {result.Winner.ToString()}";

        newGameButton.gameObject.SetActive(true);
        newGameButton.onClick.RemoveAllListeners();

        if (isGameEnd)
        {
            newGameButton.onClick.AddListener(() => SceneLoadHelper.LoadScene(SceneType.Lobby));
            newGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "로비로";
        }
        else
        {
            newGameButton.onClick.AddListener(() => SceneLoadHelper.LoadScene(SceneType.Swap));
            newGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "스왑";
        }
    }
}