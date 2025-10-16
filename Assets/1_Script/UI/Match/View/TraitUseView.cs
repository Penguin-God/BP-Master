using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitUseView : MonoBehaviour
{
    [SerializeField] Button[] blueChamps;
    [SerializeField] Button[] redChamps;

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

        SetupChampionButtons(blueChamps, Team.Blue);
        SetupChampionButtons(redChamps, Team.Red);
        traitButtonView = GetComponent<TraitButtonView>();
    }

    void SetupChampionButtons(Button[] btns, Team buttonTeam)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i; // 클로저 캡처 방지
            btns[i].onClick.AddListener(() => OnButtonClicked(new SlotData(buttonTeam, index)));
        }
    }

    void OnButtonClicked(SlotData clickSlot)
    {
        var result = presenter.ClickChampion(clickSlot, out var useData);
        if (result)
            traitUseFacade.UseTrait(useData.UseSlot, useData.TargetSlot, traits.GetSlot(useData.UseSlot));
        else
            traitButtonView.ActiveTargets(traits.GetSlot(presenter.selectionState.UseSlot));
    }
}
