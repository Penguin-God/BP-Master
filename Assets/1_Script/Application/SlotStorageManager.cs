using System;
using System.Collections.Generic;

public class SlotStorageManager
{
    public SlotStorage<int> IdSlots { get; private set; }
    public SlotStorage<Champion> ChampionSlots { get; private set; }
    public SlotStorage<ChampionStatus> StatusSlots { get; private set; }
    public SlotStorage<TraitApplier> TraitApplierSlots{ get; private set; }
    public SlotStorage<IEnumerable<TraitData>> TraitSlots { get; private set; }

    public SlotStorageManager(GameBanPickStorage storage, IdStorageConverter idStorageConverter)
    {
        IdSlots = storage.PickIds;
        ChampionSlots = idStorageConverter.IdToChampion(IdSlots);
        StatusSlots = idStorageConverter.IdToStatus(IdSlots);
        TraitApplierSlots = ChampionStorageConverter.StatusToTraitAppiler(StatusSlots);
        TraitSlots = ChampionStorageConverter.ChamptionToTrait(ChampionSlots);
    }

    public void AddTraitUseEvent(Action<SlotData> action)
    {
        foreach (var item in TraitApplierSlots.GetAll())
            item.OnUseTrait += action;
    }
}
