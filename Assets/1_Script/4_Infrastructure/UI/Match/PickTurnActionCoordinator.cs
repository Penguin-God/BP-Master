using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickTurnActionCoordinator
{
    const float HighlightDuration = 0.35f;

    readonly PickSlotFacade pickSlotFacade;
    readonly SkillUseController_UI skillUseView;
    readonly SlotViewOrchestrator slotViews;

    public event Action<SlotData> OnTurnActionCompleted;

    public PickTurnActionCoordinator(PickSlotFacade pickSlotFacade, SkillUseController_UI skillUseView, SlotViewOrchestrator slotViews = null)
    {
        this.pickSlotFacade = pickSlotFacade;
        this.skillUseView = skillUseView;
        this.slotViews = slotViews;
    }

    public void StartAction(SlotData slot, Action onDone = null)
    {
        if (skillUseView != null && skillUseView.isActiveAndEnabled)
            skillUseView.StartCoroutine(Co_TurnActionPipeline(slot, onDone));
        else
            StartSkillStep(slot, () => SelectAttackTarget(slot, () => CompleteTurnAction(slot, onDone)));
    }

    public IEnumerator Co_TurnActionPipeline(SlotData casterSlot, Action onDone = null)
    {
        yield return Co_ExecuteSkillWithVisual(casterSlot);
        yield return Co_ExecuteAttackWithVisual(casterSlot);
        CompleteTurnAction(casterSlot, onDone);
    }

    public IEnumerator Co_ExecuteSkillWithVisual(SlotData casterSlot)
    {
        var skill = pickSlotFacade.ChampionSlots.GetSlot(casterSlot).Skill;
        if (TurnActionFlowHelper.ShouldSkipSkill(skill))
        {
            if (skillUseView != null && skillUseView.SkillUseController != null)
                skillUseView.SkillUseController.UseSkill(casterSlot, Array.Empty<SlotData>());
            yield break;
        }

        var casterView = GetSlotViewSafe(casterSlot);
        if (casterView != null)
            casterView.SetHighlight(SlotHighlightPalette.SkillCaster);

        bool isSelected = false;
        IEnumerable<SlotData> selectedTargets = Array.Empty<SlotData>();

        if (skillUseView != null)
        {
            skillUseView.SelectSkillTargets(pickSlotFacade.ChampionSlots, casterSlot, targets =>
            {
                selectedTargets = targets;
                isSelected = true;
            });

            while (!isSelected)
                yield return null;
        }

        var targetList = selectedTargets.ToList();
        foreach (var targetSlot in targetList)
        {
            var targetView = GetSlotViewSafe(targetSlot);
            if (targetView != null)
                targetView.SetHighlight(SlotHighlightPalette.SkillTarget);
        }

        SoundPlayer.Play("UseSkill");
        if (skillUseView != null && skillUseView.SkillUseController != null)
            skillUseView.SkillUseController.UseSkill(casterSlot, targetList);

        yield return new WaitForSeconds(HighlightDuration);

        if (casterView != null)
            casterView.ClearHighlight();

        foreach (var targetSlot in targetList)
        {
            var targetView = GetSlotViewSafe(targetSlot);
            if (targetView != null)
                targetView.ClearHighlight();
        }
    }

    public IEnumerator Co_ExecuteAttackWithVisual(SlotData casterSlot)
    {
        Team opponentTeam = EnumCaster.GetOppoentTeam(casterSlot.Team);
        var statusSlots = pickSlotFacade.StatusSlots;

        if (TurnActionFlowHelper.ShouldSkipAttack(opponentTeam, statusSlots))
            yield break;

        var casterView = GetSlotViewSafe(casterSlot);
        if (casterView != null)
            casterView.SetHighlight(SlotHighlightPalette.AttackCaster);

        var aliveTargets = CardAttackService.GetAliveTargetSlots(opponentTeam, statusSlots).ToList();
        SlotData selectedTarget = aliveTargets[0];

        if (skillUseView != null)
        {
            bool isTargetSelected = false;
            skillUseView.BeginTargetSelection(aliveTargets, targetSlot =>
            {
                selectedTarget = targetSlot;
                isTargetSelected = true;
            });

            while (!isTargetSelected)
                yield return null;
        }

        var targetView = GetSlotViewSafe(selectedTarget);
        if (targetView != null)
            targetView.SetHighlight(SlotHighlightPalette.AttackTarget);

        ExecuteAttack(casterSlot, selectedTarget);

        yield return new WaitForSeconds(HighlightDuration);

        if (casterView != null)
            casterView.ClearHighlight();

        if (targetView != null)
            targetView.ClearHighlight();
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

    SlotView GetSlotViewSafe(SlotData slotData)
    {
        if (slotViews == null) return null;
        return slotViews.GetSlotView(slotData);
    }

    void CompleteTurnAction(SlotData slot, Action onDone)
    {
        if (skillUseView != null && skillUseView.SkillButtonView != null)
            skillUseView.SkillButtonView.InActiveAllBtns();
        OnTurnActionCompleted?.Invoke(slot);
        onDone?.Invoke();
    }
}
