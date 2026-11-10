using System;
using System.Linq;

public class PickTurnActionCoordinator
{
    readonly PickSlotFacade pickSlotFacade;
    readonly SkillUseController_UI skillUseView;

    public event Action<SlotData> OnTurnActionCompleted;

    public PickTurnActionCoordinator(PickSlotFacade pickSlotFacade, SkillUseController_UI skillUseView)
    {
        this.pickSlotFacade = pickSlotFacade;
        this.skillUseView = skillUseView;
    }

    public void StartAction(SlotData slot, Action onDone = null)
    {
        StartSkillStep(slot, () => SelectAttackTarget(slot, () => CompleteTurnAction(slot, onDone)));
    }

    void StartSkillStep(SlotData casterSlot, Action onSkillDone)
    {
        skillUseView.UseSkill(pickSlotFacade.ChampionSlots, casterSlot, onSkillDone);
    }

    void SelectAttackTarget(SlotData casterSlot, Action onAttackDone)
    {
        Team opponentTeam = EnumCaster.GetOppoentTeam(casterSlot.Team);
        var statusSlots = pickSlotFacade.StatusSlots;

        if (TurnActionFlowHelper.ShouldSkipAttack(opponentTeam, statusSlots))
        {
            onAttackDone?.Invoke();
            return;
        }

        var aliveTargets = CardAttackService.GetAliveTargetSlots(opponentTeam, statusSlots).ToList();
        skillUseView.BeginTargetSelection(aliveTargets, targetSlot =>
        {
            ExecuteAttack(casterSlot, targetSlot);
            onAttackDone?.Invoke();
        });
    }

    void ExecuteAttack(SlotData attackerSlot, SlotData targetSlot)
    {
        var statusSlots = pickSlotFacade.StatusSlots;
        CardAttackService.ExecuteAttack(statusSlots.GetSlot(attackerSlot), statusSlots.GetSlot(targetSlot));
        SoundPlayer.Play("UseSkill");
    }

    void CompleteTurnAction(SlotData slot, Action onDone)
    {
        skillUseView.SkillButtonView.InActiveAllBtns();
        OnTurnActionCompleted?.Invoke(slot);
        onDone?.Invoke();
    }
}
