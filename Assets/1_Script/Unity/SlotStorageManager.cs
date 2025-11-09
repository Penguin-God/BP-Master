
public class SlotStorageManager
{
    public SlotStorage<int> IdSlots { get; private set; }
    public SlotStorage<ChampionStatus> StatusSlots { get; private set; }
    public SlotStorage<Skill> SkillSlots { get; private set; }
    public SlotStorage<bool> SkillUseFlagSlot { get; private set; }
    public SlotStorage<ChampionSO> ChampionDataSlots { get; private set; }

    public SlotStorageManager(GameBanPickStorage storage, ChampionRepository champRegistory)
    {
        IdSlots = storage.PickIds;
        ChampionDataSlots = StorageConverter.ConvertStorage(IdSlots, id => champRegistory.GetChampionData(id));
        StatusSlots = StorageConverter.ConvertStorage(ChampionDataSlots, data => data.CreateStatus());

        SkillSlots = StorageConverter.ConvertStorage(ChampionDataSlots, champ => champ.Skill);
        SkillUseFlagSlot = StorageConverter.ConvertStorage(IdSlots, _ => false);
    }
}
