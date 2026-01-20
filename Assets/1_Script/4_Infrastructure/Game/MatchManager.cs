using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    [SerializeField] int targetWin;
    MatchRecord _record;
    public MatchRecord Record => _record;

    void Awake() // 유일객체 구현
    {
        var managers = FindObjectsByType<MatchManager>(FindObjectsSortMode.None);
        if (managers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _record = new MatchRecord(targetWin);
    }

    [ContextMenu("Red ")] void 주작() => HandleRoundEnd(Team.Red);

    public void HandleRoundEnd(Team winner)
    {
        // _record.AddWin(winner);
        if (_record.IsMatchFinished)
        {
            Debug.Log($"최종 승리 팀: {_record.MatchWinner}");
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}