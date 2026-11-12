using System;
using System.Collections.Generic;

public enum SlotUIState
{
    Default,
    Dead,
    Untargetable,
    Targetable,
    AttackCasting,
    SkillCasting,
    AttackHit,
    SkillImpact,
}

public enum SlotPhaseMode
{
    Observation,
    AttackCasting,
    SkillCasting,
}

public static class SlotStateResolver
{
    public static SlotUIState ResolveObservationState(bool isDead) => isDead ? SlotUIState.Dead : SlotUIState.Default;

    public static SlotUIState ResolvePostImpactState(bool isDead) => isDead ? SlotUIState.Dead : SlotUIState.Default;

    public static SlotUIState ResolveCastingState(SlotData slot, SlotData casterSlot, HashSet<SlotData> validTargets, bool isDead, bool isAttack)
    {
        if (slot.Equals(casterSlot)) return isAttack ? SlotUIState.AttackCasting : SlotUIState.SkillCasting;
        if (isDead) return SlotUIState.Untargetable;
        if (validTargets.Contains(slot)) return SlotUIState.Targetable;
        return SlotUIState.Untargetable;
    }

    public static SlotUIState ResolveSlotState(SlotPhaseMode phaseMode, SlotData slot, SlotData casterSlot, HashSet<SlotData> validTargets, bool isDead) => phaseMode switch
    {
        SlotPhaseMode.Observation => ResolveObservationState(isDead),
        SlotPhaseMode.AttackCasting => ResolveCastingState(slot, casterSlot, validTargets, isDead, true),
        SlotPhaseMode.SkillCasting => ResolveCastingState(slot, casterSlot, validTargets, isDead, false),
        _ => ResolveObservationState(isDead)
    };

    public static Dictionary<SlotData, SlotUIState> ResolveBoardStates(SlotPhaseMode phaseMode, SlotStorage<ChampionStatus> statusSlots) => ResolveBoardStates(phaseMode, statusSlots, default, Array.Empty<SlotData>());

    public static Dictionary<SlotData, SlotUIState> ResolveBoardStates(SlotPhaseMode phaseMode, SlotStorage<ChampionStatus> statusSlots, SlotData casterSlot, IEnumerable<SlotData> validTargets)
    {
        var result = new Dictionary<SlotData, SlotUIState>();
        var targetSet = new HashSet<SlotData>(validTargets);

        foreach (var slot in statusSlots.GetAllSlotDatas())
        {
            bool isDead = statusSlots.GetSlot(slot).IsDead;
            result[slot] = ResolveSlotState(phaseMode, slot, casterSlot, targetSet, isDead);
        }

        return result;
    }

    public static Dictionary<SlotData, SlotUIState> ResolveBoardByGamePhase(GamePhase phase, bool isPlayerCasting, bool isAttack, SlotStorage<ChampionStatus> statusSlots) => ResolveBoardByGamePhase(phase, isPlayerCasting, isAttack, statusSlots, default, Array.Empty<SlotData>());

    public static Dictionary<SlotData, SlotUIState> ResolveBoardByGamePhase(GamePhase phase, bool isPlayerCasting, bool isAttack, SlotStorage<ChampionStatus> statusSlots, SlotData casterSlot, IEnumerable<SlotData> validTargets)
    {
        if (!isPlayerCasting || phase == GamePhase.Ban || phase == GamePhase.Done)
            return ResolveBoardStates(SlotPhaseMode.Observation, statusSlots);

        SlotPhaseMode mode = isAttack ? SlotPhaseMode.AttackCasting : SlotPhaseMode.SkillCasting;
        return ResolveBoardStates(mode, statusSlots, casterSlot, validTargets);
    }
}
