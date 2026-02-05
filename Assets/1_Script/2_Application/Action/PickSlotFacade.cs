public class PickSlotFacade
{
    public readonly SlotStorage<int> IdSlots = new();
    public readonly SlotStorage<Champion> ChampionSlots = new();
    public readonly SlotStorage<ChampionStatus> StatusSlots = new();
    public readonly SlotStorage<Skill> SkillSlots = new();
    public readonly SlotStorage<PickChampion> PickChampions = new();

    public void Add(Team team, Champion champion)
    {
        IdSlots.AddSlot(team, champion.Id);
        ChampionSlots.AddSlot(team, champion);
        StatusSlots.AddSlot(team, champion.Status);
        SkillSlots.AddSlot(team, champion.Skill);
        PickChampions.AddSlot(team, new PickChampion(champion.Id, champion.Skill, champion.Status, team));
    }
}
