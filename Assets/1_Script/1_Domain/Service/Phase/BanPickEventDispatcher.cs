using System;

public class BanPickEventDispatcher
{
    public event Action<Champion, Team> OnTeamChampionPick;
    public event Action<PickChampion> OnChampionPick;
    public event Action<int> OnPick;

    public void RaisePick(Champion champion, Team team)
    {
        OnTeamChampionPick?.Invoke(champion, team);
        OnChampionPick?.Invoke(new PickChampion(champion.Id, champion.Skill, champion.Status, team));
        OnPick?.Invoke(champion.Id);
    }
}
