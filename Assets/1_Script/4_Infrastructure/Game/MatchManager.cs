using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [SerializeField] ChampionRepository champManager;
    [SerializeField] int targetWin;

    public GameBanPickStorage Storage { get; private set; }
    MatchRecord _record;
    public MatchRecord Record => _record;
    IEnumerable<int> selectableIds;

    void Awake() // 유일객체 구현
    {
        var managers = FindObjectsByType<MatchManager>(FindObjectsSortMode.None);
        if (managers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _record = new MatchRecord(targetWin);
        selectableIds = new List<int>(champManager.AllId);
        Storage = new GameBanPickStorage(selectableIds);
    }

    public void EndMatch()
    {
        selectableIds = selectableIds.Except(Storage.PickIds.GetAll());
        Storage = new GameBanPickStorage(selectableIds);
    }
}