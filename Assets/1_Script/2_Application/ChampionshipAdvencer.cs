
public enum MatchType
{
    None,
    League,
    Tournament
}

public class ChampionshipAdvencer
{
    public MatchType CurrentMatchType { get; private set; }
}
