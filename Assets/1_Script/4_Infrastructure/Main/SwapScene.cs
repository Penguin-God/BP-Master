using UnityEngine;
using Match;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class SwapScene : MonoBehaviour
{
    [SerializeField] Button nextBattleBtn;
    DeckBuildStore store;

    void Awake()
    {
        if (MatchContext.CurrentDeck == null)
            MatchContext.CurrentDeck = new DeckBuildState(20, new (ChampionDataLoder.AllId), new());
        var deckState = new DeckBuildState(MatchContext.CurrentDeck.CardCount, ExpectFearLessSet(MatchContext.CurrentDeck.AvailableCards), ExpectFearLessSet(MatchContext.CurrentDeck.SelectedCards));
        store = new DeckBuildStore(deckState);
        Change(deckState);
        store.OnStateChanged += Change;

        FindAnyObjectByType<UI_DeckBuilder>().Init(store, id => MatchContext.FearlessLockedCards.Contains(id) ? Color.gray : Color.white);

        nextBattleBtn.onClick.AddListener(() => SceneLoadHelper.LoadScene(SceneType.Battle));
        nextBattleBtn.interactable = CheckDeckPlayable(MatchContext.CurrentDeck);

        HashSet<int> ExpectFearLessSet(IEnumerable<int> cards) => new (cards.Except(MatchContext.FearlessLockedCards));
    }

    void OnDestroy()
    {
        if (store != null)
            store.OnStateChanged -= Change;
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