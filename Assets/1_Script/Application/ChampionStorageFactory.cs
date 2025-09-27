
public class ChampionStorageFactory
{
    readonly ChampionCatalog championCatalog;

    public ChampionStorageFactory(ChampionCatalog championCatalog)
    {
        this.championCatalog = championCatalog;
    }

    public SlotStorage<ChampionStatus> CreateStatusStorage(SlotStorage<int> idStorage)
    {
        var result = new SlotStorage<ChampionStatus>();

        foreach (var slot in idStorage.GetAllSlotDatas())
        {
            var status = new ChampionStatus(championCatalog.GetChampion(idStorage.GetSlot(slot)).StatData);
            result.AddSlot(slot.Team, status);
        }

        return result;
    }

    public SlotStorage<Champion> CreateChampionStorage(SlotStorage<int> idStorage)
    {
        var result = new SlotStorage<Champion>();

        foreach (var slot in idStorage.GetAllSlotDatas())
        {
            var champ = championCatalog.GetChampion(idStorage.GetSlot(slot));
            result.AddSlot(slot.Team, champ);
        }

        return result;
    }
}
