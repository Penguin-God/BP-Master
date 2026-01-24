using System.Collections.Generic;

public class ParticipantData
{
    public string Name { get; }
    public MasteryCollection Mastery { get; }

    public ParticipantData(string name, MasteryCollection mastery)
    {
        Name = name;
        Mastery = mastery;
    }
}

public class ParticipantRepository
{
    readonly Dictionary<Participant, ParticipantData> _datas = new();

    public void Save(Participant p, ParticipantData data) => _datas[p] = data;

    public ParticipantData Get(Participant p) => _datas[p];
    public bool HasData => _datas.Count > 0;
}