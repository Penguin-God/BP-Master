using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillUseController_UI : MonoBehaviour
{
    [SerializeField] Button[] blueChamps;
    [SerializeField] Button[] redChamps;
    SkillButtonView skillButtonView;

    SlotStorage<Skill> skillSlots;
    SkillUsePersenter traitUsePresenter;
    SkillUseController skillUseController;

    public void Init(SkillUsePersenter traitUsePresenter, SlotStorage<Skill> skillSlots, SkillUseController skillUseController)
    {
        gameObject.SetActive(true);
        this.traitUsePresenter = traitUsePresenter;
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

    //void OnClickSkillSlot(SlotData clickSlot)
    //{
    //    if (traitUsePresenter.IsUseable)
    //    {
    //        if (traitUsePresenter.SelectTarget(clickSlot, out var useSlot))
    //            skillUseController.UseSkill(useSlot, traitUsePresenter.CurrentTargets, skillSlots.GetSlot(useSlot));
    //    }
    //    else
    //    {
    //        var rules = skillSlots.GetSlot(clickSlot).Rules;
    //        traitUsePresenter.SelectUseSkill(clickSlot, EnumCaster.MergeRule(rules));
    //    }

    //    if (traitUsePresenter.IsUseable) skillButtonView.ActiveTargets(skillSlots.GetSlot(traitUsePresenter.UseSlot), traitUsePresenter.CurrentTargets);
    //}

    void OnClickSkillSlot(SlotData clickSlot)
    {
        targetSelector.Select(clickSlot);
        skillButtonView.ActiveTargets(skillSlots.GetSlot(useSlot), traitUsePresenter.CurrentTargets);
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
    }

    IEnumerator Co_SelectTargets(SlotData useSlot, TraitTargetSelector selector)
    {
        yield return new WaitUntil(() => selector.IsFull);
        skillUseController.UseSkill(useSlot, selector.Targets, skillSlots.GetSlot(useSlot));
    }
}
