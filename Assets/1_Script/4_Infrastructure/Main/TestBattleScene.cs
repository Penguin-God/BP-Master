using Match;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBattleScene : MonoBehaviour
{
    [SerializeField] int playerId = 1;
    [SerializeField] int ai_id;

    [SerializeField] MasteryBoardSetup playerMastery;
    [SerializeField] MasteryBoardSetup aiMastery;

    void Start()
    {
        Dictionary<int, PlayerData> dataByid = new();
        //dataByid.Add(playerId, new PlayerData("Player", playerMastery.CreateBoardCollection()));
        //dataByid.Add(ai_id, new PlayerData("AI", aiMastery.CreateBoardCollection()));

        MatchContext.MatchInit(new MatchData(playerId, ai_id), 2, dataByid, ChampionDataLoder.AllId);
        SceneManager.LoadScene("Battle");
    }
}
