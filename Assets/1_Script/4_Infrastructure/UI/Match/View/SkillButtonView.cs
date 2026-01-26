using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonView : MonoBehaviour
{
    [SerializeField] Transform blueParent;
    [SerializeField] Transform redParent;
    SlotStorage<Button> buttonSlots = new();
    Team team;
    public void Init(Team team)
    {
        gameObject.SetActive(true);
        this.team = team;

        buttonSlots.AddSlots(Team.Blue, blueParent.GetComponentsInChildren<Button>());
        buttonSlots.AddSlots(Team.Red, redParent.GetComponentsInChildren<Button>());
    }

    public void InActiveAllBtns()
    {
        foreach (Button btn in buttonSlots.GetAll())
            ButtonUtil.InActiveButton(btn);
    }

    public void ActiveTargets(SkillTargetFilter skillTargetFilter, Skill skill, IEnumerable<SlotData> currentTargets)
    {
        InActiveAllBtns();
        var slots = skillTargetFilter.FilteringTargetSlots(team, skill.Sides).Except(currentTargets);
        foreach (var slot in slots)
            ButtonUtil.ActiveButton(buttonSlots.GetSlot(slot));
    }
}
