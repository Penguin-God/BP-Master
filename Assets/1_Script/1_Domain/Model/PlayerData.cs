using System.Collections.Generic;

public class PlayerData
{
    public string Name { get; }
    public MasteryCollection Mastery { get; }
    public IReadOnlyDictionary<int, MasteryBoard> MasteryBoards;
    public PlayerData(string name, MasteryCollection mastery)
    {
        Name = name;
        Mastery = mastery;
    }

    public PlayerData(string name, IReadOnlyDictionary<int, MasteryBoard> masteryBoards)
    {
        Name = name;
        MasteryBoards = masteryBoards;
    }
}

public class ParticipantRepository
{
    readonly Dictionary<Participant, PlayerData> _datas = new();

    public void Save(Participant p, PlayerData data) => _datas[p] = data;

    public PlayerData Get(Participant p) => _datas[p];
}