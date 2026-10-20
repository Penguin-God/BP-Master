
public static class Factorys
{
    public static TeamCount CreateTeamCount<T>(SlotStorage<T> slotStorage) => new TeamCount(slotStorage.GetTeamCount(Team.Blue), slotStorage.GetTeamCount(Team.Red));
    public static TargetFindContext CreateFindContext<T>(Team team, Skill skill, SlotStorage<T> slot) => new TargetFindContext(team, EnumCaster.MergeRule(skill.Rules), CreateTeamCount(slot));

}
