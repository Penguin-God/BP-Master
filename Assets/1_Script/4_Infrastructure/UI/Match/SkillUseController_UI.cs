using System.Collections.Generic;
using UnityEngine;

public class SkillUseController_UI : MonoBehaviour
{
    SkillButtonView skillButtonView;
    SkillUsecase skillUseController;
    SkillTargetSelector targetSelector;
    SlotData useSlot;

    public void Init(SkillUsecase skillUseController)
    {
        gameObject.SetActive(true);
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

    public void UseSkill(SlotStorage<Champion> championSlot, SlotData useSlot)
    {
        var skill = championSlot.GetSlot(useSlot).Skill;
        if (skill.IsEmpty)
        {
            ExecuteSkill(new SlotData[] { });
            return;
        }

        this.useSlot = useSlot;
        targetSelector = new SkillTargetSelector(Factorys.CreateFindContext(useSlot.Team, skill, championSlot));

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
