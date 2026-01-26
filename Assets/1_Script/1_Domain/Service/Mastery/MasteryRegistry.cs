public class MasteryRegistry
{
    MasteryCollection _blueMasteryCollection;
    MasteryCollection _redMasteryCollection;

    public MasteryCollection GetTeamMasteryManager(Team team) => team == Team.Blue ? _blueMasteryCollection : _redMasteryCollection;

    public void SetMastery(Team team, MasteryCollection collection)
    {
        if (team == Team.Blue)
            _blueMasteryCollection = collection;
        else
            _redMasteryCollection = collection;
    }
}
