using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TraitButtonView : MonoBehaviour
{
    [SerializeField] Button[] blueTraits;
    [SerializeField] Button[] redTraits;
    SlotStorage<Button> buttonSlots = new();

    TraitSlotFilter traitSlotFilter;
    Team team;
    public void Init(TraitSlotFilter filter, Team team)
    {
        gameObject.SetActive(true);
        this.traitSlotFilter = filter;
        this.team = team;

        buttonSlots.AddSlots(Team.Blue, blueTraits);
        buttonSlots.AddSlots(Team.Red, redTraits);
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

    public void ActiveTargets(IEnumerable<SkillData> traitDatas, IEnumerable<SlotData> currentTargets)
    {
        InActiveAllBtns();
        var slots = traitSlotFilter.FilteringTargetSlots(team, traitDatas.Select(x => x.TargetRule.TargetSide)).Except(currentTargets);
        foreach (var slot in slots)
            ButtonUtil.ActiveButton(buttonSlots.GetSlot(slot));
    }
}
