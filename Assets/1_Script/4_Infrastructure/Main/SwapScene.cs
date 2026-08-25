using UnityEngine;
using Match;

public class SwapScene : MonoBehaviour
{
    DeckBuildStore store;
    void Awake()
    {
        store = new DeckBuildStore(MatchContext.CurrentDeck);
        store.OnStateChanged += Change;
        FindAnyObjectByType<UI_DeckBuilder>().Init(store, id => MatchContext.Storage.CanSelected(id) ? Color.white : Color.gray);
    }

    void OnDestroy()
    {
        if (store != null)
            store.OnStateChanged -= Change;
    }

    void Change(DeckBuildState state) => MatchContext.CurrentDeck = store.State;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) SceneLoadHelper.LoadScene(SceneType.Battle);
    }
}
