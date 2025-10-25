using System;
using System.Collections.Generic;

public class IdStorageConverter
{
    readonly ChampionCatalog championCatalog;
    public IdStorageConverter(ChampionCatalog championCatalog) => this.championCatalog = championCatalog;

    public SlotStorage<ChampionStatus> IdToStatus(SlotStorage<int> idStorage)
        => StorageConverter.ConvertStorage(idStorage, id => new ChampionStatus(championCatalog.GetChampion(id).StatData, TraitType.Charge));

    public SlotStorage<Champion> IdToChampion(SlotStorage<int> idStorage)
        => StorageConverter.ConvertStorage(idStorage, id => championCatalog.GetChampion(id));
}

public static class ChampionStorageConverter
{
    public static SlotStorage<IEnumerable<SkillData>> ChamptionToSkill(SlotStorage<Champion> champions)
        => StorageConverter.ConvertStorage(champions, champ => champ.TraitDatas);
}

public static class StorageConverter
{
    // TIn을 TOut으로 변환하는 Func를 받아 모든 Slot에 적용 후 리턴
    public static SlotStorage<TOut> ConvertStorage<TIn, TOut>(SlotStorage<TIn> source, Func<TIn, TOut> selector)
    {
        var result = new SlotStorage<TOut>();

        foreach (var slot in source.GetAllSlotDatas())
        {
            var converted = selector(source.GetSlot(slot));
            result.AddSlot(slot.Team, converted);
        }

        return result;
    }
}