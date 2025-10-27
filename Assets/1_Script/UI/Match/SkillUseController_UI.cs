using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillUseController_UI : MonoBehaviour
{
    [SerializeField] Button[] blueChamps;
    [SerializeField] Button[] redChamps;

    SkillButtonView skillButtonView;
    SlotStorage<IEnumerable<SkillData>> traits;
    SkillUsePersenter traitUsePresenter;
    public void Init(SkillUsePersenter traitUsePresenter, SlotStorage<IEnumerable<SkillData>> traits)
    {
        gameObject.SetActive(true);
        this.traitUsePresenter = traitUsePresenter;
        this.traits = traits;

        SetupChampionButtons(blueChamps, Team.Blue);
        SetupChampionButtons(redChamps, Team.Red);
        skillButtonView = GetComponent<SkillButtonView>();
    }

    void SetupChampionButtons(Button[] btns, Team buttonTeam)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i; // 클로저 캡처 방지
            btns[i].onClick.AddListener(() => OnClickSkillSlot(new SlotData(buttonTeam, index)));
        }
    }

    void OnClickSkillSlot(SlotData clickSlot)
    {
        if (traitUsePresenter.IsUseable) traitUsePresenter.SelectTarget(clickSlot);
        else
        {
            var rules = traits.GetSlot(clickSlot).Select(x => x.TargetRule);
            traitUsePresenter.SelectUseSkill(clickSlot, EnumCaster.MergeRule(rules));
        }

        if (traitUsePresenter.IsUseable) skillButtonView.ActiveTargets(traits.GetSlot(traitUsePresenter.UseSlot), traitUsePresenter.CurrentTargets);
    }
}
