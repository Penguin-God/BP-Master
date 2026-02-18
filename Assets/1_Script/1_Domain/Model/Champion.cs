public class Champion
{
    public readonly int Id;
    public readonly Skill Skill;
    public readonly ChampionStatus Status;

    public Champion(int id, Skill skill, ChampionStatus status)
    {
        Id = id;
        Skill = skill;
        Status = status;
    }
}


public class SlotChampion
{
    public Champion Champion;
    public int Id => Champion.Id;
    public Skill Skill => Champion.Skill;
    public ChampionStatus Status => Champion.Status;
    public readonly SlotData SlotData;

    public SlotChampion(Champion champion, SlotData slotData)
    {
        Champion = champion;
        SlotData = slotData;
    }
}