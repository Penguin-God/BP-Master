using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillUseController_UI : MonoBehaviour
{
    [SerializeField] Transform blueParent;
    [SerializeField] Transform redParent;
    SkillButtonView skillButtonView;

    SlotStorage<Skill> skillSlots;
    SkillUsecase skillUseController;

    public void Init(SlotStorage<Skill> skillSlots, SkillUsecase skillUseController)
    {
        gameObject.SetActive(true);
        this.skillSlots = skillSlots;
        this.skillUseController = skillUseController;

        SetupChampionButtons(blueParent.GetComponentsInChildren<Button>(), Team.Blue);
        SetupChampionButtons(redParent.GetComponentsInChildren<Button>(), Team.Red);
        skillButtonView = GetComponent<SkillButtonView>();
    }

    void SetupChampionButtons(Button[] btns, Team buttonTeam)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i; // 클로저 캡처 방지
            btns[i].onClick.AddListener(() => OnClickSkillSlot(new SlotData(buttonTeam, index)));
        }
    }

    void OnClickSkillSlot(SlotData clickSlot)
    {
        targetSelector.Select(clickSlot);
        RefeshButton();
        if (targetSelector.IsFull)
        {
            skillUseController.UseSkill(useSlot, targetSelector.Targets, skillSlots.GetSlot(useSlot));
            skillButtonView.InActiveAllBtns();
        }
    }

    SkillTargetSelector targetSelector;
    SlotData useSlot;
    public void UseSkill(SlotData useSlot)
    {
        if (skillSlots.GetSlot(useSlot).IsEmpty)
        {
            skillUseController.UseSkill(useSlot, new SlotData[] { }, skillSlots.GetSlot(useSlot));
            return;
        }

        this.useSlot = useSlot;
        var rule = EnumCaster.MergeRule(skillSlots.GetSlot(useSlot).Rules);
        targetSelector = new SkillTargetSelector(useSlot.Team, skillSlots.GetTeamCounter(), rule);
        RefeshButton();
        // 타겟이 없는 경우
        if (targetSelector.IsFull)
        {
            skillUseController.UseSkill(useSlot, new SlotData[] { }, skillSlots.GetSlot(useSlot));

        }
    }

    void RefeshButton() => skillButtonView.ActiveTargets(new SkillTargetFilter(skillSlots.GetTeamCounter()), skillSlots.GetSlot(useSlot), targetSelector.Targets);
}
