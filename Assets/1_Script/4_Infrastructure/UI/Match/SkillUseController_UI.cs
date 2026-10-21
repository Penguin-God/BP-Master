using System.Collections.Generic;
using UnityEngine;

public class SkillUseController_UI : MonoBehaviour
{
    SkillButtonView skillButtonView;
    SlotStorage<Skill> skillSlots;
    SkillUsecase skillUseController;
    SkillTargetSelector targetSelector;
    SlotData useSlot;

    public void Init(SlotStorage<Skill> skillSlots, SkillUsecase skillUseController)
    {
        gameObject.SetActive(true);
        this.skillSlots = skillSlots;
        this.skillUseController = skillUseController;

        skillButtonView = GetComponent<SkillButtonView>();
        skillButtonView.Init();

        foreach (var slot in skillButtonView.ButtonSlots.GetAllSlotDatas())
            skillButtonView.ButtonSlots.GetSlot(slot).onClick.AddListener(() => OnClickSkillSlot(slot));
    }

    void OnClickSkillSlot(SlotData clickSlot)
    {
        targetSelector.Select(clickSlot);
        ActiveValidTargets();
        if (targetSelector.IsFull) ExecuteSkill(targetSelector.Targets);
    }

    public void UseSkill(SlotData useSlot)
    {
        if (skillSlots.GetSlot(useSlot).IsEmpty)
        {
            ExecuteSkill(new SlotData[] { });
            return;
        }

        this.useSlot = useSlot;
        targetSelector = new SkillTargetSelector(Factorys.CreateFindContext(useSlot.Team, skillSlots.GetSlot(useSlot), skillSlots));

        ActiveValidTargets();
        // 타겟이 없는 경우
        if (targetSelector.IsFull) ExecuteSkill(new SlotData[] { });
    }
    void ExecuteSkill(IEnumerable<SlotData> targets)
    {
        skillUseController.UseSkill(useSlot, targets);
        skillButtonView.InActiveAllBtns();
    }

    void ActiveValidTargets() => skillButtonView.ActiveTargets(targetSelector.GetSelectableTarget());
}
