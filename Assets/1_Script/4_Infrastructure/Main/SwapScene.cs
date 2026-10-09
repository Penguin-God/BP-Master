using UnityEngine;
using Match;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class SwapScene : MonoBehaviour
{
    [SerializeField] Button nextBattleBtn;
    [SerializeField] Transform fearlessPanel;
    [SerializeField] UI_DeckCard cardPrefab;
    [SerializeField] ChampionView championView;
    [SerializeField] TutorialTriggerSO tutorialTriggerSO;

    DeckBuildStore store;

    void Awake()
    {
        if (MatchContext.CurrentDeck == null)
            MatchContext.CurrentDeck = new DeckBuildState(20, new (ChampionDataLoder.AllId), new());
        var deckState = new DeckBuildState(MatchContext.CurrentDeck.CardCount, ExpectFearLessSet(MatchContext.CurrentDeck.AvailableCards), ExpectFearLessSet(MatchContext.CurrentDeck.SelectedCards));
        store = new DeckBuildStore(deckState);
        Change(deckState);
        store.OnStateChanged += Change;

        FindAnyObjectByType<UI_DeckBuilder>().Init(store);

        nextBattleBtn.onClick.AddListener(() => SceneLoadHelper.LoadScene(SceneType.Battle));
        nextBattleBtn.interactable = CheckDeckPlayable(MatchContext.CurrentDeck);

        HashSet<int> ExpectFearLessSet(IEnumerable<int> cards) => new (cards.Except(MatchContext.FearlessLockedCards));
        DrawFearlessCards();

        tutorialTriggerSO.StartTutorialOneTime(TutorialTriggerType.Swap);
    }

    void OnDestroy()
    {
        if (store != null)
            store.OnStateChanged -= Change;
    }

    void DrawFearlessCards()
    {
        DeckCardDrawer.DrawCards(
            fearlessPanel,
            cardPrefab,
            MatchContext.FearlessLockedCards,
            id => ChampionDataLoder.NameCatalog[id],
            target => championView.UpdateDisplay(target.Id));
    }

    void Change(DeckBuildState state)
    {
        MatchContext.CurrentDeck = store.State;
        nextBattleBtn.interactable = CheckDeckPlayable(state);
    }

    bool CheckDeckPlayable(DeckBuildState state)
    {
        bool isDeckFull = state.SelectedCards.Count == state.CardCount;
        bool isAllCardsValid = state.SelectedCards.All(id => MatchContext.FearlessLockedCards.Contains(id) == false);
        return isDeckFull && isAllCardsValid;
    }
}