using UnityEngine;
using UnityEngine.UI;

public class ChampionSelector_UI : MonoBehaviour
{
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionView championFocusView;
    [SerializeField] ChampionRepository championManager;

    ChampionSelectPresenter championSelector = null;
    PhaseFlowOrchestrator phaseManager;
    [SerializeField] ChampionButtonView champBtnView;

    public void Init(ChampionSelectPresenter presenter, PhaseFlowOrchestrator pm)
    {
        gameObject.SetActive(true);

        champBtnView.CreateButtons();
        champBtnView.AddEvent(SelectChampion);

        championSelector = presenter;
        phaseManager = pm;

        nailDownBtn.onClick.AddListener(NailDownChampion);
    }

    void SelectChampion(ChampionIdentify champion)
    {
        championSelector.SelectChamp(champion.Id);
        championFocusView.UpdateDisplay(championManager.GetChampionData(champion.Id));
    }

    void NailDownChampion()
    {
        championSelector.NailDownChampion(phaseManager.CurrentFlow);
        championFocusView.ClearDisplay();
    }
}