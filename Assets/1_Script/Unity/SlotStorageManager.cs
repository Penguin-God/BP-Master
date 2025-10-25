using System.Collections.Generic;

public class SlotStorageManager
{
    public SlotStorage<int> IdSlots { get; private set; }
    public SlotStorage<Champion> ChampionSlots { get; private set; }
    public SlotStorage<ChampionStatus> StatusSlots { get; private set; }
    public SlotStorage<IEnumerable<SkillData>> SkillSlots { get; private set; }
    public SlotStorage<bool> SkillUseFlagSlot { get; private set; }

    public SlotStorageManager(GameBanPickStorage storage, ChampionRepository champRegistory)
    {
        IdSlots = storage.PickIds;
        ChampionSlots = StorageConverter.ConvertStorage(IdSlots, id => champRegistory.GetChampionData(id).CreateChampion());
        StatusSlots = StorageConverter.ConvertStorage(IdSlots, id => champRegistory.GetChampionData(id).CreateStatus());

        SkillSlots = ChampionStorageConverter.ChamptionToSkill(ChampionSlots);
        SkillUseFlagSlot = StorageConverter.ConvertStorage(IdSlots, _ => false);
    }
}
