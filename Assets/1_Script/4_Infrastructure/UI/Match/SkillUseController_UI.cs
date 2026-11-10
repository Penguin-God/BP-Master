using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillUseController_UI : MonoBehaviour
{
    SkillButtonView skillButtonView;
    SkillUsecase skillUseController;
    TargetSelection currentSelection;
    Action<SlotData> slotClickHandler;
    Action onSkillCompleted;
    Action<IEnumerable<SlotData>> onTargetsConfirmed;

    public SkillButtonView SkillButtonView => skillButtonView;
    public SkillUsecase SkillUseController => skillUseController;

    public void Init(SkillUsecase skillUseController)
    {
        gameObject.SetActive(true);
        this.skillUseController = skillUseController;

        skillButtonView = GetComponent<SkillButtonView>();
        skillButtonView.Init();

        foreach (var slot in skillButtonView.ButtonSlots.GetAllSlotDatas())
            skillButtonView.ButtonSlots.GetSlot(slot).onClick.AddListener(() => OnClickSlot(slot));
    }

    public void BeginTargetSelection(IEnumerable<SlotData> selectableSlots, Action<SlotData> onSelected)
    {
        slotClickHandler = onSelected;
        skillButtonView.ActiveTargets(selectableSlots);
    }

    public void SelectSkillTargets(SlotStorage<Champion> championSlot, SlotData useSlot, Action<IEnumerable<SlotData>> onConfirmed)
    {
        slotClickHandler = null;
        onSkillCompleted = null;
        onTargetsConfirmed = onConfirmed;

        var skill = championSlot.GetSlot(useSlot).Skill;
        if (TurnActionFlowHelper.ShouldSkipSkill(skill))
        {
            CompleteTargetConfirmation(Array.Empty<SlotData>());
            return;
        }

        var context = Factorys.CreateFindContext(useSlot.Team, skill, championSlot);
        currentSelection = TargetSelection.Create(useSlot, context);

        if (currentSelection.IsFull) CompleteTargetConfirmation(Array.Empty<SlotData>());
        else ActiveValidTargets();
    }

    public void UseSkill(SlotStorage<Champion> championSlot, SlotData useSlot) => UseSkill(championSlot, useSlot, null);

    public void UseSkill(SlotStorage<Champion> championSlot, SlotData useSlot, Action onCompleted)
    {
        slotClickHandler = null;
        onTargetsConfirmed = null;
        onSkillCompleted = onCompleted;
        var skill = championSlot.GetSlot(useSlot).Skill;
        if (TurnActionFlowHelper.ShouldSkipSkill(skill))
        {
            ExecuteSkill(useSlot, Array.Empty<SlotData>());
            return;
        }

        var context = Factorys.CreateFindContext(useSlot.Team, skill, championSlot);
        currentSelection = TargetSelection.Create(useSlot, context);

        if (currentSelection.IsFull) ExecuteSkill(currentSelection.Caster, Array.Empty<SlotData>());
        else ActiveValidTargets();
    }

    void OnClickSlot(SlotData clickSlot)
    {
        if (slotClickHandler != null)
        {
            var handler = slotClickHandler;
            slotClickHandler = null;
            skillButtonView.InActiveAllBtns();
            handler.Invoke(clickSlot);
            return;
        }

        OnClickSkillSlot(clickSlot);
    }

    void OnClickSkillSlot(SlotData clickSlot)
    {
        if (currentSelection == null) return;

        currentSelection = TargetSelectionRules.Select(currentSelection, clickSlot);
        ActiveValidTargets();

        if (currentSelection.IsFull)
        {
            if (onTargetsConfirmed != null)
            {
                CompleteTargetConfirmation(currentSelection.Selected);
                return;
            }

            SoundPlayer.Play("UseSkill");
            ExecuteSkill(currentSelection.Caster, currentSelection.Selected);
        }
    }

    void CompleteTargetConfirmation(IEnumerable<SlotData> selectedTargets)
    {
        var callback = onTargetsConfirmed;
        onTargetsConfirmed = null;
        skillButtonView.InActiveAllBtns();
        currentSelection = null;
        callback?.Invoke(selectedTargets);
    }

    void ExecuteSkill(SlotData caster, IEnumerable<SlotData> targets)
    {
        var callback = onSkillCompleted;
        onSkillCompleted = null;
        skillUseController.UseSkill(caster, targets);
        skillButtonView.InActiveAllBtns();
        currentSelection = null;
        callback?.Invoke();
    }

    void ActiveValidTargets() => skillButtonView.ActiveTargets(currentSelection.GetSelectableTargets());
}
