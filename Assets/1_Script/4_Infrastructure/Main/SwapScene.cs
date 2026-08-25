using UnityEngine;
using Match;
using UnityEngine.UI;
using System.Linq;

public class SwapScene : MonoBehaviour
{
    [SerializeField] Button nextBattleBtn;
    DeckBuildStore store;

    void Awake()
    {
        var deckState = new DeckBuildState(MatchContext.CurrentDeck.CardCount, new (MatchContext.CurrentDeck.AvailableCards.Except(MatchContext.FearlessLockedCards)), new (MatchContext.CurrentDeck.SelectedCards.Except(MatchContext.FearlessLockedCards)));
        store = new DeckBuildStore(deckState);
        Change(deckState);
        store.OnStateChanged += Change;

        FindAnyObjectByType<UI_DeckBuilder>().Init(store, id => MatchContext.FearlessLockedCards.Contains(id) ? Color.gray : Color.white);

        nextBattleBtn.onClick.AddListener(() => SceneLoadHelper.LoadScene(SceneType.Battle));
        nextBattleBtn.interactable = CheckDeckPlayable(MatchContext.CurrentDeck);
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