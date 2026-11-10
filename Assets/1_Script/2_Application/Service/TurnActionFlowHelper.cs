using System.Linq;

public static class TurnActionFlowHelper
{
    public static bool ShouldSkipAttack(Team opponentTeam, SlotStorage<ChampionStatus> statusSlots)
        => !CardAttackService.GetAliveTargetSlots(opponentTeam, statusSlots).Any();

    public static bool ShouldSkipSkill(Skill skill)
        => skill.IsEmpty;
}
