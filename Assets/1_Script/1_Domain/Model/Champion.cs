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


public class PickChampion
{
    public readonly int Id;
    public readonly Skill Skill;
    public readonly ChampionStatus Status;
    public readonly Team Team;

    public PickChampion(int id, Skill skill, ChampionStatus status, Team team)
    {
        Id = id;
        Skill = skill;
        Status = status;
        Team = team;
    }
}