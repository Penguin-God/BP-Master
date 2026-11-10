using System.Collections.Generic;
using System.Linq;

public static class CardAttackService
{
    public static IEnumerable<SlotData> GetAliveTargetSlots(Team opponentTeam, SlotStorage<ChampionStatus> statusSlots)
    {
        return statusSlots.GetAllSlotDatas()
            .Where(slot => slot.Team == opponentTeam && !statusSlots.GetSlot(slot).IsDead);
    }

    public static void ExecuteAttack(ChampionStatus attacker, ChampionStatus target)
    {
        if (attacker == null || target == null || attacker.IsDead || target.IsDead) return;
        target.TakeDamage(attacker.Stat.Attack);
    }

    public static void AttackFirstAliveTarget(SlotData attackerSlot, SlotStorage<ChampionStatus> statusSlots)
    {
        Team opponentTeam = EnumCaster.GetOppoentTeam(attackerSlot.Team);
        var aliveTargetSlots = GetAliveTargetSlots(opponentTeam, statusSlots).OrderBy(slot => slot.Index).ToList();
        if (aliveTargetSlots.Count == 0) return;

        ExecuteAttack(statusSlots.GetSlot(attackerSlot), statusSlots.GetSlot(aliveTargetSlots[0]));
    }
}
