using log4net.Appender;
using System.Collections.Generic;

public class PlayerData
{
    public readonly int Id = 0;
    public readonly string Name;
    public readonly MasteryCollection Mastery;
    public MasteryBoardCollection MasteryBoardCollection;

    public PlayerData(string name, MasteryCollection mastery)
    {
        Name = name;
        Mastery = mastery;
    }

    public PlayerData(int id, string name, MasteryBoardCollection masteryBoardCollection)
    {
        Id = id;
        Name = name;
        MasteryBoardCollection = masteryBoardCollection;
    }
}

public class ParticipantRepository
{
    readonly Dictionary<Participant, PlayerData> _datas = new();

    public void Save(Participant p, PlayerData data) => _datas[p] = data;

    public PlayerData Get(Participant p) => _datas[p];
}