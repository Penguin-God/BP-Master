using System.Collections.Generic;
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

    //public void ActiveTargets(TeamCount teamCount, Skill skill, IEnumerable<SlotData> currentTargets)
    //{
    //    InActiveAllBtns();
    //    var slots = TargetCountCalculator.CreateTargetingableSlots(team, skill.Sides, teamCount).Except(currentTargets);
    //    foreach (var slot in slots)
    //        ButtonUtil.ActiveButton(buttonSlots.GetSlot(slot));
    //}

    public void ActiveTargets(IEnumerable<SlotData> targets)
    {
        foreach (var slot in targets)
            ButtonUtil.ActiveButton(buttonSlots.GetSlot(slot));
    }
}
