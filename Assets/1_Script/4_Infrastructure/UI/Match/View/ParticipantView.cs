using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParticipantView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI blueText;
    [SerializeField] TextMeshProUGUI redText;

    public void ViewParticipant(Dictionary<Team, int> playerIds)
    {
        //UpdateText(Team.Blue, playerIds[Team.Blue], matchRecord.PlayerWinCount);
        //UpdateText(EnumCaster.GetOppoentTeam(playerTeam), "AI", matchRecord.AiWinCount);
    }

    void UpdateText(Team team, string name, int win) => GetText(team).text = $"{name} : {win}";
    TextMeshProUGUI GetText(Team team) => team == Team.Blue ? blueText : redText;
}
