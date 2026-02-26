using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [SerializeField] ChampionRepository champManager;
    [SerializeField] int targetWin;
    [SerializeField] int[] masteryLevels;

    public BanPickStorage Storage { get; private set; }
    MatchRecord _record;
    public MatchRecord Record => _record;
    IEnumerable<int> selectableIds;
    public ParticipantRepository participantRepository = new ParticipantRepository();

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
        Storage = new BanPickStorage(selectableIds);

        var drawer = new MasteryDrawer(champManager.AllId);
        participantRepository.Save(Participant.Player, new ParticipantData("Player", new MasteryCollection(drawer.DrawRandoms(masteryLevels))));
        participantRepository.Save(Participant.AI, new ParticipantData("AI", new MasteryCollection(drawer.DrawRandoms(masteryLevels))));
    }

    public void EndMatch()
    {
        selectableIds = selectableIds.Except(Storage.PickIds.GetAll());
        Storage = new BanPickStorage(selectableIds);
    }

    public void Clear()
    {
        selectableIds = new int[0];
        Storage = null;
    }
}