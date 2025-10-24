using System.Collections.Generic;

public class SlotStorageManager
{
    public SlotStorage<int> IdSlots { get; private set; }
    public SlotStorage<Champion> ChampionSlots { get; private set; }
    public SlotStorage<ChampionStatus> StatusSlots { get; private set; }
    public SlotStorage<IEnumerable<SkillData>> SkillSlots { get; private set; }
    public SlotStorage<bool> SkillUseFlagSlot { get; private set; }

    public SlotStorageManager(GameBanPickStorage storage, IdStorageConverter idStorageConverter)
    {
        IdSlots = storage.PickIds;
        ChampionSlots = idStorageConverter.IdToChampion(IdSlots);
        StatusSlots = idStorageConverter.IdToStatus(IdSlots);
        SkillSlots = ChampionStorageConverter.ChamptionToSkill(ChampionSlots);
        SkillUseFlagSlot = StorageConverter.ConvertStorage(IdSlots, _ => false);
    }
}
