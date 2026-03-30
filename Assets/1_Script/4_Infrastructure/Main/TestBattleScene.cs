using Match;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBattleScene : MonoBehaviour
{
    [SerializeField] int userId;
    [SerializeField] int ai_id;

    void Start()
    {
        MatchContext.MatchInit(new MatchData(userId, ai_id), 2, ChampionDataLoder.AllId);
        SceneManager.LoadScene("Battle");
    }
}
