using System;
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
    [SerializeField] Transform changeablePanel;
    [SerializeField] Transform selectedPanel;

    [Header("Prefabs & Dependencies")]
    [SerializeField] UI_DeckCard cardPrefab;

    DeckBuildStore _store;
    CardIdentity _focusedCard;
    List<UI_DeckCard> _spawnedCards = new List<UI_DeckCard>();

    Func<int, Color> _cardColorProvider;

    void Awake()
    {
        addButton.onClick.AddListener(OnAddClicked);
        removeButton.onClick.AddListener(OnRemoveClicked);
    }

    public void Init(DeckBuildStore store, Func<int, Color> cardColorProvider = null)
    {
        _store = store;
        _cardColorProvider = cardColorProvider;
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
        RefreshUIVisuals();
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
            _focusedCard = null;
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
    void UpdateView(DeckBuildState state)
    {
        _spawnedCards.Clear();

        DrawCards(changeablePanel, state.ChangeableCards, CardPoolType.Available);
        DrawCards(selectedPanel, state.SelectedCards, CardPoolType.Selected);

        RefreshUIVisuals();
    }

    void DrawCards(Transform panel, HashSet<int> cardIds, CardPoolType poolType)
    {
        foreach (Transform child in panel) Destroy(child.gameObject);

        foreach (var id in cardIds)
        {
            var cardObj = Instantiate(cardPrefab, panel);
            // 외부 함수로 색깔 결정
            Color cardColor = _cardColorProvider != null ? _cardColorProvider(id) : Color.white;

            cardObj.Init(new CardIdentity(poolType, id), ChampionDataLoder.NameCatalog[id], cardColor, OnCardClicked, OnCardDoubleClicked);
            _spawnedCards.Add(cardObj);
        }
    }

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