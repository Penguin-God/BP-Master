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
    public void Init(TraitSlotFilter filter)
    {
        gameObject.SetActive(true);
        this.traitSlotFilter = filter;

        buttonSlots.AddSlots(Team.Blue, blueTraits);
        buttonSlots.AddSlots(Team.Red, redTraits);

        InActiveAllBtns();
    }

    public void InActiveAllBtns()
    {
        foreach (Button btn in buttonSlots.GetAll())
            ButtonUtil.InActiveButton(btn);
    }

    public void ActiveUseableButtons(Team team)
    {
        InActiveAllBtns();
        var slots = traitSlotFilter.FilteringUseableSlots(team);
        foreach (var slot in slots)
            ButtonUtil.ActiveButton(buttonSlots.GetSlot(slot));
    }

    public void ActiveTargets(Team team, IEnumerable<TraitData> traitDatas)
    {
        InActiveAllBtns();
        var slots = traitSlotFilter.FilteringTargetSlots(team, traitDatas.Select(x => x.TargetRule.TargetSide));
        foreach (var slot in slots)
            ButtonUtil.ActiveButton(buttonSlots.GetSlot(slot));
    }
}
