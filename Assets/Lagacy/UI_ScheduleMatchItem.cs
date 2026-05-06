using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScheduleMatchItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI matchNumberText;
    [SerializeField] TextMeshProUGUI matchText;
    [SerializeField] Image backgroundImage;

    [Header("State Colors")]
    [SerializeField] Color pastColor = Color.gray;
    [SerializeField] Color currentColor = Color.green;
    [SerializeField] Color playerColor = Color.yellow;
    [SerializeField] Color normalColor = Color.white;

    public void Bind(MatchDisplayModel model)
    {
        matchNumberText.text = $"No.{model.MatchIndex + 1}";
        matchText.text = $"{model.Match.Id1} vs {model.Match.Id2}";
        backgroundImage.color = GetColor(model.State);
    }

    Color GetColor(MatchState state)
    {
        return state switch
        {
            MatchState.Past => pastColor,
            MatchState.Current => currentColor,
            MatchState.Player => playerColor,
            _ => normalColor
        };
    }
}