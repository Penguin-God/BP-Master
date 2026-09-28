using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DeckBuilder : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] Button cardTranslateBtn;
    [SerializeField] Transform changeablePanel;
    [SerializeField] Transform selectedPanel;
    [SerializeField] ChampionView championView;

    [Header("Prefabs & Dependencies")]
    [SerializeField] UI_DeckCard cardPrefab;

    DeckBuildStore _store;
    CardIdentity _focusedCard;
    List<UI_DeckCard> _spawnedCards = new List<UI_DeckCard>();
    void Awake()
    {
        cardTranslateBtn.onClick.AddListener(TranslateFocusCard);
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
        RefreshUIVisuals();
        championView.UpdateDisplay(target.Id);
    }

    void OnCardDoubleClicked(CardIdentity target)
    {
        _focusedCard = target;
        TranslateFocusCard();
    }

    void TranslateFocusCard()
    {
        if (ForcusCardContainDeck()) CardToNotUesd();
        else CardToUesd();
    }

    void CardToUesd()
    {
        if (ForcusCardContainDeck() == false)
        {
            _store.Dispatch(state => DeckBuildService.AddCard(state, _focusedCard.Id));
            RefreshUIVisuals();
        }
    }

    void CardToNotUesd()
    {
        if (ForcusCardContainDeck())
        {
            _store.Dispatch(state => DeckBuildService.RemoveCard(state, _focusedCard.Id));
            RefreshUIVisuals();
        }
    }

    void UpdateView(DeckBuildState state)
    {
        _spawnedCards.Clear();

        _spawnedCards.AddRange(DeckCardDrawer.DrawCards(
            changeablePanel,
            cardPrefab,
            state.ChangeableCards,
            id => ChampionDataLoder.NameCatalog[id],
            OnCardClicked,
            OnCardDoubleClicked));

        _spawnedCards.AddRange(DeckCardDrawer.DrawCards(
            selectedPanel,
            cardPrefab,
            state.SelectedCards,
            id => ChampionDataLoder.NameCatalog[id],
            OnCardClicked,
            OnCardDoubleClicked));

        RefreshUIVisuals();
    }

    void RefreshUIVisuals()
    {
        if (_store == null) return;
        var state = _store.State;

        bool isFull = state.SelectedCards.Count >= state.CardCount;

        countText.text = $"{state.SelectedCards.Count} / {state.CardCount}";
        countText.color = isFull ? Color.white : Color.red;

        foreach (var card in _spawnedCards)
            card.SetFocus(_focusedCard != null && card.Identity == _focusedCard);
    }

    bool ForcusCardContainDeck()
    {
        if (_focusedCard == null) return false;
        return _store.State.SelectedCards.Contains(_focusedCard.Id);
    }
}


public static class DeckCardDrawer
{
    public static List<UI_DeckCard> DrawCards(
        Transform panel,
        UI_DeckCard cardPrefab,
        IEnumerable<int> cardIds,
        Func<int, string> getCardName,
        Action<CardIdentity> onCardClicked,
        Action<CardIdentity> onCardDoubleClicked = null)
    {
        foreach (Transform child in panel)
            GameObject.Destroy(child.gameObject);

        var cards = new List<UI_DeckCard>();

        foreach (var id in cardIds)
        {
            var card = GameObject.Instantiate(cardPrefab, panel);

            card.Init(
                new CardIdentity(id),
                getCardName(id),
                Color.white,
                onCardClicked,
                onCardDoubleClicked);

            cards.Add(card);
        }

        return cards;
    }
}