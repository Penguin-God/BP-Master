using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TraitUseView : MonoBehaviour
{
    [SerializeField] Button[] blueChamps;
    [SerializeField] Button[] redChamps;
    Dictionary<Team, Button[]> buttons = new();

    TraitUsePresenter presenter;
    TraitUseFacade traitUseFacade;
    SlotStorage<IEnumerable<TraitData>> traits;
    TraitSlotFilter traitSlotFilter;
    public void Init(TraitUsePresenter presenter, TraitUseFacade traitUseFacade, SlotStorage<IEnumerable<TraitData>> traits, TraitSlotFilter filter)
    {
        gameObject.SetActive(true);
        this.presenter = presenter;
        this.traitUseFacade = traitUseFacade;
        this.traits = traits;
        this.traitSlotFilter = filter;

        buttons.Add(Team.Blue, blueChamps);
        buttons.Add(Team.Red, redChamps);

        SetupChampionButtons(blueChamps, Team.Blue);
        SetupChampionButtons(redChamps, Team.Red);
        InActiveAllBtns();
    }

    void SetupChampionButtons(Button[] btns, Team buttonTeam)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i; // 클로저 캡처 방지
            btns[i].onClick.AddListener(() => OnButtonClicked(buttonTeam, index));
        }
    }

    void InActiveAllBtns()
    {
        foreach (Button btn in buttons.Values.SelectMany(x => x))
            ButtonUtil.InActiveButton(btn);
    }

    public void Set(Team team)
    {
        presenter.ChangeTeam(team);
        ActiveButtons(false);
    }

    public void UpdateTrait(Team team)
    {
        if (team == presenter.Team) ActiveButtons(false);
        else InActiveAllBtns();
    }

    void ActiveButtons(bool isUse)
    {
        InActiveAllBtns();
        var targetSides = traits.GetSlot(presenter.selectionState.UseSlot).Select(x => x.TargetRule.TargetSide);
        foreach (var slot in traitSlotFilter.GetSlots(presenter.selectionState.UseTurn, presenter.Team, targetSides))
            ButtonUtil.ActiveButton(buttons[slot.Team][slot.Index]);
    }

    void OnButtonClicked(Team buttonTeam, int index)
    {
        var result = presenter.ClickChampion(new SlotData(buttonTeam, index), out var useData);
        if (result)
            traitUseFacade.UseTrait(useData.UseSlot, useData.TargetSlot, traits.GetSlot(useData.UseSlot));
        else
            ActiveButtons(true);
    }
}
