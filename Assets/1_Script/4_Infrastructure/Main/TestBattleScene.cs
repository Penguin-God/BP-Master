using Match;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBattleScene : MonoBehaviour
{
    [SerializeField] int playerId = 1;
    [SerializeField] int ai_id;
    void Start()
    {
        MatchContext.MatchInit(new MatchData(playerId, ai_id), 2, new int[] { 5, 15, 15 }, ChampionDataLoder.AllId);
        SceneManager.LoadScene("Battle");
    }
}
