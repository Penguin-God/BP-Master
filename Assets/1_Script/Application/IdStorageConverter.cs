using System.Collections.Generic;

public class IdStorageConverter
{
    readonly ChampionCatalog championCatalog;

    public IdStorageConverter(ChampionCatalog championCatalog)
    {
        this.championCatalog = championCatalog;
    }

    public SlotStorage<ChampionStatus> IdToStatus(SlotStorage<int> idStorage)
    {
        var result = new SlotStorage<ChampionStatus>();

        foreach (var slot in idStorage.GetAllSlotDatas())
        {
            var status = new ChampionStatus(championCatalog.GetChampion(idStorage.GetSlot(slot)).StatData);
            result.AddSlot(slot.Team, status);
        }

        return result;
    }

    public SlotStorage<Champion> IdToChampion(SlotStorage<int> idStorage)
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

public static class ChampionStorageConverter
{
    public static SlotStorage<IEnumerable<TraitData>> ChamptionToTrait(SlotStorage<Champion> champions)
    {
        var result = new SlotStorage<IEnumerable<TraitData>>();

        foreach (var slot in champions.GetAllSlotDatas())
        {
            var trait = champions.GetSlot(slot).TraitDatas;
            result.AddSlot(slot.Team, trait);
        }

        return result;
    }
}
