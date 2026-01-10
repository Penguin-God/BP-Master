using UnityEngine;
using UnityEngine.UI;

public class ChampionSelector_UI : MonoBehaviour, IPhaseEntry
{
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionView championFocusView;
    [SerializeField] ChampionRepository championManager;

    GameBanPickStorage storage;
    PhaseFlowOrchestrator phaseManager;
    [SerializeField] ChampionButtonView champBtnView;

    public void Init(GameBanPickStorage storage, PhaseFlowOrchestrator pm)
    {
        gameObject.SetActive(true);

        champBtnView.CreateButtons();
        champBtnView.AddEvent(SelectChampion);

        this.storage = storage;
        phaseManager = pm;

        nailDownBtn.onClick.AddListener(NailDownChampion);
    }

    int selectId = -1;
    void SelectChampion(ChampionIdentify champion)
    {
        selectId = champion.Id;
        championFocusView.UpdateDisplay(championManager.GetChampionData(champion.Id));
    }

    void NailDownChampion()
    {
        if(storage.SaveSelect(phaseManager.CurrentFlow, selectId))
        {
            ButtonUtil.InActiveButton(nailDownBtn);
            championFocusView.ClearDisplay();
        }
    }

    public void EnterBan() => ButtonUtil.ActiveButton(nailDownBtn);
    public void EnterPick() => ButtonUtil.ActiveButton(nailDownBtn);
}