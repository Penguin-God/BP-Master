using System;
using System.Collections.Generic;
using System.Linq;

public enum Team { Blue, Red, All }

public class BanPickStorage
{
    readonly Dictionary<Team, HashSet<int>> banStorage = new();
    public SlotStorage<int> PickIds { get; private set; } = new();

    public readonly HashSet<int> SelectableIds = new();

    public BanPickStorage(IEnumerable<int> allIds)
    {
        SelectableIds = new HashSet<int>(allIds);
        banStorage.Add(Team.Red, new());
        banStorage.Add(Team.Blue, new());
    }

    public bool CanSelected(int id) => SelectableIds.Contains(id);
    readonly IEnumerable<GamePhase> VaildPhases = new GamePhase[] { GamePhase.Ban, GamePhase.Pick };
    public bool SaveSelect(GameFlowData flow, int selectedId)
    {
        if (CanSelected(selectedId) == false || VaildPhases.Contains(flow.Phase) == false) return false;

        SelectableIds.Remove(selectedId);
        if (flow.Phase == GamePhase.Ban) Ban(flow.Turn, selectedId);
        else Pick(flow.Turn, selectedId);
        return true;
    }

    public void Ban(Team team, int id)
    {
        SelectableIds.Remove(id);
        banStorage[team].Add(id);
    }

    public SlotData Pick(Team team, int id)
    {
        SelectableIds.Remove(id);
        PickIds.AddSlot(team, id);
        return new SlotData(team, PickIds.GetTeamCount(team) - 1);
    }
}
