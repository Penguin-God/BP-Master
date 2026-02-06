using System;
using System.Collections.Generic;
using System.Linq;

public enum Team { Blue, Red, All }

public class BanPickStorage
{
    readonly Dictionary<Team, HashSet<int>> banStorage = new();
    public SlotStorage<int> PickIds { get; private set; } = new();

    public event Action<Team, int> OnBan;
    public event Action<SlotData, int> OnPick;

    public readonly HashSet<int> SelectableIds = new();

    public BanPickStorage(IEnumerable<int> allIds)
    {
        SelectableIds = new HashSet<int>(allIds);
        banStorage.Add(Team.Red, new());
        banStorage.Add(Team.Blue, new());
    }

    bool CanSelected(int id) => SelectableIds.Contains(id);

    readonly IEnumerable<GamePhase> VaildPhases = new GamePhase[] { GamePhase.Ban, GamePhase.Pick };
    public bool SaveSelect(GameFlowData flow, int selectedId)
    {
        if (CanSelected(selectedId) == false || VaildPhases.Contains(flow.Phase) == false) return false;

        SelectableIds.Remove(selectedId);
        if (flow.Phase == GamePhase.Ban) Ban(flow.Turn, selectedId);
        else Pick(flow.Turn, selectedId);
        return true;
    }

    void Ban(Team team, int id)
    {
        banStorage[team].Add(id);
        OnBan?.Invoke(team, id);
    }

    public void Pick(Team team, int id)
    {
        PickIds.AddSlot(team, id);
        OnPick?.Invoke(new SlotData(team, PickIds.GetTeamCount(team) - 1), id);
    }
}
