using Match;
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
        var playerDatas = new PlayerMatchData(new PlayerData(playerId, "@@", playerMastery.CreateBoardCollection()), new PlayerData(ai_id, "AI", aiMastery.CreateBoardCollection()));
        MatchContext.MatchInit(playerDatas, 2, ChampionDataLoder.AllId);
        SceneManager.LoadScene("Battle");
    }
}
