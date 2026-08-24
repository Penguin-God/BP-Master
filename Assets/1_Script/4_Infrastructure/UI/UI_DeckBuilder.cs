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

    DeckBuildStore _store;
    CardIdentity _focusedCard;
    List<UI_DeckCard> _spawnedCards = new List<UI_DeckCard>();

    void Awake()
    {
        addButton.onClick.AddListener(OnAddClicked);
        removeButton.onClick.AddListener(OnRemoveClicked);
    }

    public void Init(DeckBuildStore store)
    {
        _store = store;
        _store.OnStateChanged += UpdateView;
        UpdateView(_store.State);
    }

    void OnDestroy()
    {
        if (_store != null)
            _store.OnStateChanged -= UpdateView;
    }

    // --- Action Handlers --- //

    void OnCardClicked(CardIdentity target)
    {
        _focusedCard = target;
        RefreshUIVisuals(); // 스토어 갱신 없이(카드 재생성 없이) UI 포커스/버튼 상태만 즉시 갱신
    }

    void OnCardDoubleClicked(CardIdentity target)
    {
        _focusedCard = target;
        if (target.Pool == CardPoolType.Available) OnAddClicked();
        else OnRemoveClicked();
    }

    void OnAddClicked()
    {
        if (_focusedCard?.Pool == CardPoolType.Available)
        {
            _store.Dispatch(state => DeckBuildService.AddCard(state, _focusedCard.Id));
            _focusedCard = null; // 이동 후 포커스 초기화
            RefreshUIVisuals();
        }
    }

    void OnRemoveClicked()
    {
        if (_focusedCard?.Pool == CardPoolType.Selected)
        {
            _store.Dispatch(state => DeckBuildService.RemoveCard(state, _focusedCard.Id));
            _focusedCard = null;
            RefreshUIVisuals();
        }
    }

    // --- View Render --- //

    // 스토어 상태가 변했을 때(카드가 넘어갔을 때)만 실행됨
    void UpdateView(DeckBuildState state)
    {
        _spawnedCards.Clear();

        DrawCards(availablePanel, state.AvailableCards, CardPoolType.Available);
        DrawCards(selectedPanel, state.SelectedCards, CardPoolType.Selected);

        RefreshUIVisuals();
    }

    void DrawCards(Transform panel, HashSet<int> cardIds, CardPoolType poolType)
    {
        foreach (Transform child in panel) Destroy(child.gameObject);

        foreach (var id in cardIds)
        {
            var cardObj = Instantiate(cardPrefab, panel);
            string cardName = ChampionDataLoder.NameCatalog[id];

            cardObj.Init(new CardIdentity(poolType, id), cardName, OnCardClicked, OnCardDoubleClicked);
            _spawnedCards.Add(cardObj);
        }
    }

    // 텍스트, 버튼, 하이라이트를 즉시 갱신 (전체 카드를 다시 그리지 않음)
    void RefreshUIVisuals()
    {
        if (_store == null) return;
        var state = _store.State;

        bool isFull = state.SelectedCards.Count >= state.CardCount;

        countText.text = $"{state.SelectedCards.Count} / {state.CardCount}";
        countText.color = isFull ? Color.white : Color.red;

        addButton.interactable = (_focusedCard?.Pool == CardPoolType.Available) && !isFull;
        removeButton.interactable = (_focusedCard?.Pool == CardPoolType.Selected);

        foreach (var card in _spawnedCards)
            card.SetFocus(_focusedCard != null && card.Identity == _focusedCard);
    }
}