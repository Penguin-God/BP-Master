using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampionSelector_UI : MonoBehaviour, IPhaseEntry
{
    [SerializeField] Button selectBtn;
    [SerializeField] ChampionView championFocusView;
    [SerializeField] ChampionRepository championManager;

    BanPickHandler banpickHandler;
    PhaseFlowOrchestrator phaseManager;
    [SerializeField] ChampionButtonView champBtnView;

    public void Init(BanPickHandler storage, PhaseFlowOrchestrator pm)
    {
        gameObject.SetActive(true);

        champBtnView.AddEvent(SelectChampion);

        this.banpickHandler = storage;
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
        ButtonUtil.InActiveButton(selectBtn);
        // 시간 커플링. 빈 스킬일 경우 SetupSelectButton이 실행되서 버튼 비활성화가 늦으면 평생 그 상태임
        banpickHandler.SaveSelect(phaseManager.CurrentFlow, selectId); // 근데 이게 먼저 돼야 픽 에라 방지 가능
        championFocusView.ClearDisplay();
    }

    void SetupSelectButton(string label)
    {
        ButtonUtil.ActiveButton(selectBtn);
        selectBtn.GetComponentInChildren<TextMeshProUGUI>().text = label;
    }

    public void EnterBan() => SetupSelectButton("밴");
    public void EnterPick() => SetupSelectButton("픽");
}