using System;

public class PhaseActionEventDispatcher
{
    public event Action<int> OnPick;
    public event Action<Champion> OnChampionPick;
    public void RaisePick(Champion champion)
    {
        OnPick?.Invoke(champion.Id);
        OnChampionPick?.Invoke(champion);
    }
}
