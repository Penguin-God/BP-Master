using TMPro;
using UnityEngine;

public class ParticipantView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI blueText;
    [SerializeField] TextMeshProUGUI redText;

    public void ViewParticipant(MatchRecord matchRecord, Team playerTeam)
    {
        UpdateText(playerTeam, "Player", matchRecord.PlayerWins);
        UpdateText(EnumCaster.GetOppoentTeam(playerTeam), "AI", matchRecord.AiWins);
    }

    void UpdateText(Team team, string name, int win) => GetText(team).text = $"{name} : {win}";
    TextMeshProUGUI GetText(Team team) => team == Team.Blue ? blueText : redText;
}
