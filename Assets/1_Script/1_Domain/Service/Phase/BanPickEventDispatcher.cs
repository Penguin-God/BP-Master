using System;

public class BanPickEventDispatcher
{
    public event Action<Champion, Team> OnChampionPick;

    public void RaisePick(Champion champion, Team team)
    {
        OnChampionPick?.Invoke(champion, team);
    }
}
