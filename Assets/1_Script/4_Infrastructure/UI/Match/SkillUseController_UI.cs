using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillUseController_UI : MonoBehaviour
{
    SkillButtonView skillButtonView;
    SkillUsecase skillUseController;
    TargetSelection currentSelection;

    public void Init(SkillUsecase skillUseController)
    {
        gameObject.SetActive(true);
        this.skillUseController = skillUseController;

        skillButtonView = GetComponent<SkillButtonView>();
        skillButtonView.Init();

        foreach (var slot in skillButtonView.ButtonSlots.GetAllSlotDatas())
            skillButtonView.ButtonSlots.GetSlot(slot).onClick.AddListener(() => OnClickSkillSlot(slot));
    }

    public void UseSkill(SlotStorage<Champion> championSlot, SlotData useSlot)
    {
        var skill = championSlot.GetSlot(useSlot).Skill;
        if (skill.IsEmpty)
        {
            ExecuteSkill(useSlot, Array.Empty<SlotData>());
            return;
        }

        var context = Factorys.CreateFindContext(useSlot.Team, skill, championSlot);
        currentSelection = TargetSelection.Create(useSlot, context);

        if (currentSelection.IsFull) ExecuteSkill(currentSelection.Caster, Array.Empty<SlotData>());
        else ActiveValidTargets();
    }

    void OnClickSkillSlot(SlotData clickSlot)
    {
        if (currentSelection == null) return;

        currentSelection = TargetSelectionRules.Select(currentSelection, clickSlot);
        ActiveValidTargets();

        if (currentSelection.IsFull)
            ExecuteSkill(currentSelection.Caster, currentSelection.Selected);
    }

    void ExecuteSkill(SlotData caster, IEnumerable<SlotData> targets)
    {
        skillUseController.UseSkill(caster, targets);
        skillButtonView.InActiveAllBtns();
        currentSelection = null;
    }

    void ActiveValidTargets() => skillButtonView.ActiveTargets(currentSelection.GetSelectableTargets());
}