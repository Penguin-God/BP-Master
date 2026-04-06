using Match;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBattleScene : MonoBehaviour
{
    [SerializeField] MatchConfigSO configSO;
    [SerializeField] int ai_id;

    void Start()
    {
        MatchContext.MatchInit(new MatchData(configSO.UserId, ai_id), configSO.TargetWinCount, ChampionDataLoder.AllId);
        SceneManager.LoadScene("Battle");
    }
}
