public readonly struct SelectData
{
    public readonly int Id;
    public readonly TurnInfo CurrentTurn;
    public readonly int Count;

    public SelectData(int id, TurnInfo currentTurn, int count)
    {
        Id = id;
        CurrentTurn = currentTurn;
        Count = count;
    }
}

public readonly struct TurnInfo
{
    public readonly Team Team;
    public readonly BanPcikPhase Phase;

    public TurnInfo(Team team, BanPcikPhase phase)
    {
        Team = team;
        Phase = phase;
    }
}

public readonly struct SelectInfo
{
    public readonly Team Team;
    public readonly SelectType Select;
    public readonly int Id;

    public SelectInfo(Team team, SelectType select, int id)
    {
        Team = team;
        Select = select;
        Id = id;
    }
}
