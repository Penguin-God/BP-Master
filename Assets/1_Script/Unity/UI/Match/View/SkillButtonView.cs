using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonView : MonoBehaviour
{
    [SerializeField] Button[] blueSkills;
    [SerializeField] Button[] redSkills;
    SlotStorage<Button> buttonSlots = new();

    SkillSlotFilter traitSlotFilter;
    Team team;
    public void Init(SkillSlotFilter filter, Team team)
    {
        gameObject.SetActive(true);
        this.traitSlotFilter = filter;
        this.team = team;

        buttonSlots.AddSlots(Team.Blue, blueSkills);
        buttonSlots.AddSlots(Team.Red, redSkills);
    }

    void InActiveAllBtns()
    {
        foreach (Button btn in buttonSlots.GetAll())
            ButtonUtil.InActiveButton(btn);
    }

    void ActiveUseableButtons()
    {
        InActiveAllBtns();
        var slots = traitSlotFilter.FilteringUseableSlots(team);
        foreach (var slot in slots)
            ButtonUtil.ActiveButton(buttonSlots.GetSlot(slot));
    }

    public void RefreshButtonsByTurn(Team team)
    {
        if (team == this.team) ActiveUseableButtons();
        else InActiveAllBtns();
    }

    public void ActiveTargets(Skill skill, IEnumerable<SlotData> currentTargets)
    {
        InActiveAllBtns();
        var slots = traitSlotFilter.FilteringTargetSlots(team, skill.Sides).Except(currentTargets);
        foreach (var slot in slots)
            ButtonUtil.ActiveButton(buttonSlots.GetSlot(slot));
    }
}
