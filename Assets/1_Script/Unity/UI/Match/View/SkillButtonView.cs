using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonView : MonoBehaviour
{
    [SerializeField] Button[] blueSkills;
    [SerializeField] Button[] redSkills;
    SlotStorage<Button> buttonSlots = new();
    Team team;
    public void Init(Team team)
    {
        gameObject.SetActive(true);
        this.team = team;

        buttonSlots.AddSlots(Team.Blue, blueSkills);
        buttonSlots.AddSlots(Team.Red, redSkills);
    }

    public void InActiveAllBtns()
    {
        foreach (Button btn in buttonSlots.GetAll())
            ButtonUtil.InActiveButton(btn);
    }

    public void ActiveTargets(Team team, SkillTargetFilter skillTargetFilter, Skill skill, IEnumerable<SlotData> currentTargets)
    {
        InActiveAllBtns();
        var slots = skillTargetFilter.FilteringTargetSlots(team, skill.Sides).Except(currentTargets);
        foreach (var slot in slots)
            ButtonUtil.ActiveButton(buttonSlots.GetSlot(slot));
    }
}
