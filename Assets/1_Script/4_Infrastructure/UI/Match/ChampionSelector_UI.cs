using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampionSelector_UI : MonoBehaviour, IPhaseEntry
{
    [SerializeField] Button selectBtn;
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

        selectBtn.onClick.AddListener(NailDownChampion);
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
            ButtonUtil.InActiveButton(selectBtn);
            championFocusView.ClearDisplay();
        }
    }

    void SetupSelectButton(string label)
    {
        ButtonUtil.ActiveButton(selectBtn);
        selectBtn.GetComponentInChildren<TextMeshProUGUI>().text = label;
    }

    public void EnterBan() => SetupSelectButton("픽");
    public void EnterPick() => SetupSelectButton("밴");
}