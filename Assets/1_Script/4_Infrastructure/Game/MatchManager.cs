using UnityEngine;

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
}