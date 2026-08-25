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
        store = new DeckBuildStore(MatchContext.CurrentDeck);
        store.OnStateChanged += Change;
        FindAnyObjectByType<UI_DeckBuilder>().Init(store, id => MatchContext.Storage.CanSelected(id) ? Color.white : Color.gray);
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
        bool isAllCardsValid = state.SelectedCards.All(id => MatchContext.Storage.CanSelected(id));
        return isDeckFull && isAllCardsValid;
    }
}
