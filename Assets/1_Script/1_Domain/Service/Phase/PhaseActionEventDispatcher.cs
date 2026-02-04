using System;

public class PhaseActionEventDispatcher
{
    public event Action<Champion, Team> OnChampionPick;

    public void RaisePick(Champion champion, Team team)
    {
        OnChampionPick?.Invoke(champion, team);
    }
}
