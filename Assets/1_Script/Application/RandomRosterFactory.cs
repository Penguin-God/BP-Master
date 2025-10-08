using System.Collections.Generic;
using System.Linq;

public class RandomRosterFactory
{
    private readonly MasteryDrawer masteryDrawer;

    public RandomRosterFactory(MasteryDrawer masteryDrawer)
    {
        this.masteryDrawer = masteryDrawer;
    }

    public SlotStorage<ProGamer> CreateRoster(int rosterCount, int[] levels)
    {
        var storage = new SlotStorage<ProGamer>();
        storage.AddSlots(Team.Blue, DrawRandomTeam(rosterCount, levels));
        storage.AddSlots(Team.Red, DrawRandomTeam(rosterCount, levels));
        return storage;
    }

    IEnumerable<ProGamer> DrawRandomTeam(int teamCount, int[] levels) 
        => Enumerable.Range(0, teamCount).Select(_ => new ProGamer(masteryDrawer.DrawRandoms(levels)));
}
