using UnityEngine;
using Match;
using UnityEngine.SceneManagement;

public class SwapScene : MonoBehaviour
{
    DeckBuildStore store;
    void Awake()
    {
        store = new DeckBuildStore(MatchContext.CurrentDeck);
        store.OnStateChanged += Change;
        FindAnyObjectByType<UI_DeckBuilder>().Init(store);
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
