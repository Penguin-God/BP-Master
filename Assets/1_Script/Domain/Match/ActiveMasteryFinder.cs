
public class ActiveMasteryFinder
{
    readonly SlotStorage<ProGamer> gamers;
    readonly SlotStorage<int> ids;

    public ActiveMasteryFinder(SlotStorage<ProGamer> gamers, SlotStorage<int> ids)
    {
        this.gamers = gamers;
        this.ids = ids;
    }

    public int GetActiveLevel(SlotData slot)
    {
        var gamer = gamers.GetSlot(slot);
        var championId = ids.GetSlot(slot);
        return gamer.GetMastery(championId);
    }
}
