using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TraitUseController : MonoBehaviour
{
    [SerializeField] Button[] blueChamps;
    [SerializeField] Button[] redChamps;

    TraitButtonView traitButtonView;

    SlotSelectionState selectState = new SlotSelectionState();
    TraitUseFacade traitUseFacade;
    SlotStorage<IEnumerable<TraitData>> traits;
    public void Init(TraitUseFacade traitUseFacade, SlotStorage<IEnumerable<TraitData>> traits)
    {
        gameObject.SetActive(true);
        this.traitUseFacade = traitUseFacade;
        this.traits = traits;

        SetupChampionButtons(blueChamps, Team.Blue);
        SetupChampionButtons(redChamps, Team.Red);
        traitButtonView = GetComponent<TraitButtonView>();
    }

    TraitUsePersenter traitUsePresenter;
    public void Init(TraitUsePersenter traitUsePresenter, SlotStorage<IEnumerable<TraitData>> traits)
    {
        gameObject.SetActive(true);
        this.traitUsePresenter = traitUsePresenter;
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
            btns[i].onClick.AddListener(() => OnClickTraitSlot(new SlotData(buttonTeam, index)));
        }
    }

    void OnClickTraitSlot(SlotData clickSlot)
    {
        if (traitUsePresenter.IsUseable)
        {
            traitUsePresenter.SelectTarget(clickSlot);
            if(traitUsePresenter.IsUseable) traitButtonView.ActiveTargets(traits.GetSlot(traitUsePresenter.UseSlot));
        }
        else
        {
            var rules = traits.GetSlot(clickSlot).Select(x => x.TargetRule);
            traitUsePresenter.SelectUseTrait(clickSlot, EnumCaster.MergeRule(rules));
            traitButtonView.ActiveTargets(traits.GetSlot(clickSlot));
        }
    }
}
