using System;
using System.Collections.Generic;

public class IdStorageConverter
{
    readonly ChampionCatalog championCatalog;
    public IdStorageConverter(ChampionCatalog championCatalog) => this.championCatalog = championCatalog;

    public SlotStorage<ChampionStatus> IdToStatus(SlotStorage<int> idStorage)
        => StorageConverter.ConvertStorage(idStorage, id => new ChampionStatus(championCatalog.GetChampion(id).StatData));

    public SlotStorage<Champion> IdToChampion(SlotStorage<int> idStorage)
        => StorageConverter.ConvertStorage(idStorage, id => championCatalog.GetChampion(id));
}

public static class ChampionStorageConverter
{
    public static SlotStorage<IEnumerable<TraitData>> ChamptionToTrait(SlotStorage<Champion> champions)
        => StorageConverter.ConvertStorage(champions, champ => champ.TraitDatas);

    public static SlotStorage<TraitApplier> StatusToTraitAppiler(SlotStorage<ChampionStatus> statuses)
        => StorageConverter.ConvertStorage(statuses, (status, slot) => new TraitApplier(statuses, slot));
}

public static class StorageConverter
{
    public static SlotStorage<TOut> ConvertStorage<TIn, TOut>(SlotStorage<TIn> source, Func<TIn, SlotData, TOut> selector)
    {
        var result = new SlotStorage<TOut>();

        foreach (var slot in source.GetAllSlotDatas())
        {
            var converted = selector(source.GetSlot(slot), slot);
            result.AddSlot(slot.Team, converted);
        }

        return result;
    }

    public static SlotStorage<TOut> ConvertStorage<TIn, TOut>(SlotStorage<TIn> source, Func<TIn, TOut> selector)
        => ConvertStorage(source, (tin, _) => selector(tin));
}