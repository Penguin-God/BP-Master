public class MasteryRegistry
{
    MasteryStatCollection _blueMasteryCollection;
    MasteryStatCollection _redMasteryCollection;

    public MasteryStatCollection GetTeamMasteryCollection(Team team) => team == Team.Blue ? _blueMasteryCollection : _redMasteryCollection;

    public void InitTeamMastery(Team team, MasteryStatCollection collection)
    {
        if (team == Team.Blue)
            _blueMasteryCollection = collection;
        else
            _redMasteryCollection = collection;
    }
}
