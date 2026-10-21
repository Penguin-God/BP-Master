using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonView : MonoBehaviour
{
    [SerializeField] Transform blueParent;
    [SerializeField] Transform redParent;
    public readonly SlotStorage<Button> ButtonSlots = new();

    public void Init()
    {
        ButtonSlots.AddSlots(Team.Blue, blueParent.GetComponentsInChildren<Button>());
        ButtonSlots.AddSlots(Team.Red, redParent.GetComponentsInChildren<Button>());
    }

    public void InActiveAllBtns()
    {
        foreach (Button btn in ButtonSlots.GetAll())
            ButtonUtil.InActiveButton(btn);
    }

    public void ActiveTargets(IEnumerable<SlotData> targets)
    {
        InActiveAllBtns();
        foreach (var slot in targets)
            ButtonUtil.ActiveButton(ButtonSlots.GetSlot(slot));
    }
}
