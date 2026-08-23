using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DeckBuilder : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] Button addButton;       // '>' 방향 버튼 (사용 가능 -> 내 덱)
    [SerializeField] Button removeButton;    // '<' 방향 버튼 (내 덱 -> 사용 가능)
    [SerializeField] Transform availablePanel;
    [SerializeField] Transform selectedPanel;

    [Header("Prefabs & Dependencies")]
    [SerializeField] UI_DeckCard cardPrefab;

    // 유일한 '가변' 필드 (단방향 업데이트 됨)
    DeckBuildUIState _state;

    void Awake()
    {
        // 이동 버튼 이벤트 바인딩
        addButton.onClick.AddListener(OnAddClicked);
        removeButton.onClick.AddListener(OnRemoveClicked);

        var domainState = new DeckBuildState(
            RequiredCount: 25,
            AvailableCards: new HashSet<int>(ChampionDataLoder.AllId),
            SelectedCards: new HashSet<int>()
        );

        _state = new DeckBuildUIState(domainState);
        UpdateView();
    }

    // 외부(예: 로비나 매니저)에서 덱 빌딩 화면을 열 때 호출
    public void Init(int requiredCount, IEnumerable<int> initialAvailableCards)
    {
        var domainState = new DeckBuildState(
            RequiredCount: 25,
            AvailableCards: new HashSet<int>(ChampionDataLoder.AllId),
            SelectedCards: new HashSet<int>()
        );

        _state = new DeckBuildUIState(domainState);
        UpdateView();
    }


    void SetState(DeckBuildUIState newState)
    {
        _state = newState;
        UpdateView();
    }

    // --- Action Handlers --- //
    void OnAvailableCardClicked(int id) => SetState(DeckBuildUIService.FocusAvailableCard(_state, id));
    void OnAvailableCardDoubleClicked(int id) => SetState(DeckBuildUIService.DoubleClickAvailableCard(_state, id));

    void OnSelectedCardClicked(int id) => SetState(DeckBuildUIService.FocusSelectedCard(_state, id));
    void OnSelectedCardDoubleClicked(int id) => SetState(DeckBuildUIService.DoubleClickSelectedCard(_state, id));

    void OnAddClicked() => SetState(DeckBuildUIService.MoveFocusedToSelected(_state));
    void OnRemoveClicked() => SetState(DeckBuildUIService.MoveFocusedToAvailable(_state));

    // --- View Render --- //

    void UpdateView()
    {
        // 1. 뷰 모델 생성 (색상 및 텍스트 데이터 로직 처리)
        var viewModel = DeckBuildUIService.CreateViewModel(_state);

        // 2. 텍스트 및 버튼 상호작용 업데이트
        countText.text = viewModel.CountText.Text;
        countText.color = viewModel.CountText.IsWarning ? Color.red : Color.white;

        addButton.interactable = viewModel.AddButton.IsInteractable;
        removeButton.interactable = viewModel.RemoveButton.IsInteractable;

        // 3. 패널에 카드 렌더링
        DrawCards(availablePanel, _state.DomainState.AvailableCards, _state.FocusedAvailableCardId, OnAvailableCardClicked, OnAvailableCardDoubleClicked);
        DrawCards(selectedPanel, _state.DomainState.SelectedCards, _state.FocusedSelectedCardId, OnSelectedCardClicked, OnSelectedCardDoubleClicked);
    }

    void DrawCards(Transform panel, HashSet<int> cardIds, int focusedId, System.Action<int> onClick, System.Action<int> onDoubleClick)
    {
        foreach (Transform child in panel) Destroy(child.gameObject);

        foreach (var id in cardIds)
        {
            var cardObj = Instantiate(cardPrefab, panel);
            string cardName = ChampionDataLoder.NameCatalog[id];

            cardObj.Init(id, cardName, onClick, onDoubleClick);
            cardObj.SetFocus(id == focusedId); // 포커스된 카드면 하이라이트 ON
        }
    }
}