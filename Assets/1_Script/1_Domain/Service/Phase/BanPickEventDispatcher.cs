using System;

public class BanPickEventDispatcher
{
    public event Action<Champion, Team> OnTeamChampionPick;
    public event Action<SlotChampion> OnChampionPick;
    public event Action<SlotData, int> OnPick;

    public void RaisePick(Champion champion, SlotData slotData)
    {
        OnTeamChampionPick?.Invoke(champion, slotData.Team);
        OnChampionPick?.Invoke(new SlotChampion(champion, slotData));
        OnPick?.Invoke(slotData, champion.Id);
    }
}
