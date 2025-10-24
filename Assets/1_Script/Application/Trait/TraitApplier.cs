
public class TraitApplier
{
    public bool IsUse { get; set; }

    public TraitApplier(SlotStorage<ChampionStatus> statuses, SlotData slotData) {}

    public void Use() => IsUse = true;
}
