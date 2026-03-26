using Match;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBattleScene : MonoBehaviour
{
    [SerializeField] PlayerDataInspector playerMastery;
    [SerializeField] PlayerDataInspector aiMastery;

    void Start()
    {
        var playerDatas = new PlayerMatchData(playerMastery.ToData(), aiMastery.ToData());
        MatchContext.MatchInit(playerDatas, 2, ChampionDataLoder.AllId);
        SceneManager.LoadScene("Battle");
    }
}
