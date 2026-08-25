using Match;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParticipantView : MonoBehaviour
{
    [SerializeField] PlayerDataProviderFactorySO playerDataProviderFactorySO;
    [SerializeField] TextMeshProUGUI blueText;
    [SerializeField] TextMeshProUGUI redText;
    
    public void ViewParticipant(Dictionary<Team, int> playerIds)
    {
        UpdateText(Team.Blue, playerIds[Team.Blue]);
        UpdateText(Team.Red, playerIds[Team.Red]);
    }

    void UpdateText(Team team, int id) => GetText(team).text = $"{playerDataProviderFactorySO.CreatePlayerDataProvider().LoadPlayer(id).Name} : {MatchContext.MatchState.GetWin(id)}";
    TextMeshProUGUI GetText(Team team) => team == Team.Blue ? blueText : redText;
}
