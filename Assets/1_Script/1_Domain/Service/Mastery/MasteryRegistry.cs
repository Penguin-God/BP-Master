public class MasteryRegistry
{
    MasteryCollection _blueMasteryCollection;
    MasteryCollection _redMasteryCollection;

    public MasteryCollection GetTeamMasteryCollection(Team team) => team == Team.Blue ? _blueMasteryCollection : _redMasteryCollection;

    public void InitTeamMastery(Team team, MasteryCollection collection)
    {
        if (team == Team.Blue)
            _blueMasteryCollection = collection;
        else
            _redMasteryCollection = collection;
    }
}
