using Match;
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
    [SerializeField] Transform fearlessPanel;
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
        // Draw FearLess Cards
        DrawCards(fearlessPanel, MatchContext.FearlessLockedCards);
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
        if (ForcusCardContainDeck() == false) CardToUesd();
        else CardToNotUesd();
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

    // --- View Render --- //
    void UpdateView(DeckBuildState state)
    {
        _spawnedCards.Clear();

        DrawCards(changeablePanel, state.ChangeableCards);
        DrawCards(selectedPanel, state.SelectedCards);

        RefreshUIVisuals();
    }

    void DrawCards(Transform panel, HashSet<int> cardIds)
    {
        foreach (Transform child in panel) Destroy(child.gameObject);

        foreach (var id in cardIds)
        {
            var cardObj = Instantiate(cardPrefab, panel);
            cardObj.Init(new CardIdentity(id), ChampionDataLoder.NameCatalog[id], Color.white, OnCardClicked, OnCardDoubleClicked);
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

        foreach (var card in _spawnedCards)
            card.SetFocus(_focusedCard != null && card.Identity == _focusedCard);
    }

    bool ForcusCardContainDeck()
    {
        if (_focusedCard == null) return false;
        return _store.State.SelectedCards.Contains(_focusedCard.Id);
    }
}