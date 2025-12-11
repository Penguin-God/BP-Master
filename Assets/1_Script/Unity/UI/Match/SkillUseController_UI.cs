using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillUseController_UI : MonoBehaviour
{
    [SerializeField] Button[] blueChamps;
    [SerializeField] Button[] redChamps;
    SkillButtonView skillButtonView;

    SlotStorage<Skill> skillSlots;
    SkillUseController skillUseController;

    public void Init(SkillUsePersenter traitUsePresenter, SlotStorage<Skill> skillSlots, SkillUseController skillUseController)
    {
        gameObject.SetActive(true);
        this.skillSlots = skillSlots;
        this.skillUseController = skillUseController;

        SetupChampionButtons(blueChamps, Team.Blue);
        SetupChampionButtons(redChamps, Team.Red);
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
    }

    TraitTargetSelector targetSelector;
    SlotData useSlot;
    public void UseSkill(SlotData useSlot)
    {
        this.useSlot = useSlot;
        var rule = EnumCaster.MergeRule(skillSlots.GetSlot(useSlot).Rules);
        // 전체 타겟, 상대 수보다 많은 타겟 문제
        targetSelector = new TraitTargetSelector(skillSlots.GetTeamCount(EnumCaster.GetTargetTeam(useSlot.Team, rule.TargetSide)), rule);
        StartCoroutine(Co_SelectTargets(useSlot, targetSelector));
        RefeshButton();
    }

    void RefeshButton() => skillButtonView.ActiveTargets(new SkillTargetFilter(skillSlots.GetTeamCounter()), skillSlots.GetSlot(useSlot), targetSelector.Targets);

    IEnumerator Co_SelectTargets(SlotData useSlot, TraitTargetSelector selector)
    {
        yield return new WaitUntil(() => selector.IsFull);
        skillUseController.UseSkill(useSlot, selector.Targets, skillSlots.GetSlot(useSlot));
    }
}
