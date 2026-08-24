using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DeckBuilder : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] Button addButton;
    [SerializeField] Button removeButton;
    [SerializeField] Transform availablePanel;
    [SerializeField] Transform selectedPanel;

    [Header("Prefabs & Dependencies")]
    [SerializeField] UI_DeckCard cardPrefab;

    private DeckBuildStore _store; // 상태를 들고 있는 스토어를 참조만 함

    public void Init(DeckBuildStore store)
    {
        _store = store;
        _store.OnStateChanged += UpdateView;

        // 최초 1회 화면 그리기
        UpdateView(_store.State);
    }

    private void OnDestroy()
    {
        if (_store != null)
            _store.OnStateChanged -= UpdateView;
    }

    // --- Action Handlers: 직접 상태를 바꾸지 않고 스토어에 '요청(Dispatch)'만 함 --- //
    void OnAvailableCardClicked(int id) => _store.Dispatch(state => DeckBuildUIService.FocusAvailableCard(state, id));
    void OnAvailableCardDoubleClicked(int id) => _store.Dispatch(state => DeckBuildUIService.DoubleClickAvailableCard(state, id));

    void OnSelectedCardClicked(int id) => _store.Dispatch(state => DeckBuildUIService.FocusSelectedCard(state, id));
    void OnSelectedCardDoubleClicked(int id) => _store.Dispatch(state => DeckBuildUIService.DoubleClickSelectedCard(state, id));

    void UpdateView(DeckBuildUIState state)
    {
        var viewModel = DeckBuildUIService.CreateViewModel(state);

        countText.text = viewModel.CountText.Text;
        countText.color = viewModel.CountText.IsWarning ? Color.red : Color.white;

        addButton.interactable = viewModel.AddButton.IsInteractable;
        removeButton.interactable = viewModel.RemoveButton.IsInteractable;

        DrawCards(availablePanel, state.DomainState.AvailableCards, state.FocusedAvailableCardId, OnAvailableCardClicked, OnAvailableCardDoubleClicked);
        DrawCards(selectedPanel, state.DomainState.SelectedCards, state.FocusedSelectedCardId, OnSelectedCardClicked, OnSelectedCardDoubleClicked);
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