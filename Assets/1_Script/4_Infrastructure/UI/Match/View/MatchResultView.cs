using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public record GameEndButtonModel(string Text, UnityAction OnClick);

public class MatchResultView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI blueScoreText;
    [SerializeField] TextMeshProUGUI redScoreText;
    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] Button newGameButton;

    public void DrawResult(MatchResult result, GameEndButtonModel buttonModel)
    {
        FindAnyObjectByType<ChampionButtonView>().gameObject.SetActive(false);
        gameObject.SetActive(true);

        blueScoreText.text = new ScoreTextBuilder().BuildText(result.BlueInfo);
        redScoreText.text = new ScoreTextBuilder().BuildText(result.RedInfo);
        winnerText.text = $"승리 : {result.Winner.ToString()}";

        newGameButton.onClick.RemoveAllListeners();
        newGameButton.onClick.AddListener(buttonModel.OnClick);
        newGameButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonModel.Text;
    }
}