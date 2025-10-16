using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitUseView : MonoBehaviour
{
    [SerializeField] Button[] blueChamps;
    [SerializeField] Button[] redChamps;
    Dictionary<Team, Button[]> buttons = new();

    TraitButtonView traitButtonView;

    TraitUsePresenter presenter;
    TraitUseFacade traitUseFacade;
    SlotStorage<IEnumerable<TraitData>> traits;
    public void Init(TraitUsePresenter presenter, TraitUseFacade traitUseFacade, SlotStorage<IEnumerable<TraitData>> traits)
    {
        gameObject.SetActive(true);
        this.presenter = presenter;
        this.traitUseFacade = traitUseFacade;
        this.traits = traits;

        buttons.Add(Team.Blue, blueChamps);
        buttons.Add(Team.Red, redChamps);

        SetupChampionButtons(blueChamps, Team.Blue);
        SetupChampionButtons(redChamps, Team.Red);
        traitButtonView = GetComponent<TraitButtonView>();
    }

    void SetupChampionButtons(Button[] btns, Team buttonTeam)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i; // 클로저 캡처 방지
            btns[i].onClick.AddListener(() => OnButtonClicked(buttonTeam, index));
        }
    }

    public void Set(Team team)
    {
        presenter.ChangeTeam(team);
        traitButtonView.ActiveUseableButtons(presenter.Team);
    }

    public void UpdateTrait(Team team)
    {
        if (team == presenter.Team) traitButtonView.ActiveUseableButtons(presenter.Team);
        else traitButtonView.InActiveAllBtns();
    }

    void OnButtonClicked(Team buttonTeam, int index)
    {
        var result = presenter.ClickChampion(new SlotData(buttonTeam, index), out var useData);
        if (result)
            traitUseFacade.UseTrait(useData.UseSlot, useData.TargetSlot, traits.GetSlot(useData.UseSlot));
        else
            traitButtonView.ActiveTargets(presenter.Team, traits.GetSlot(presenter.selectionState.UseSlot));
    }
}
