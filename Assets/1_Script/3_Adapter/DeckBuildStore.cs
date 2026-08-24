using System;
public class DeckBuildStore
{
    public DeckBuildUIState State { get; private set; }

    public event Action<DeckBuildUIState> OnStateChanged;

    public DeckBuildStore(DeckBuildUIState initialState)
    {
        State = initialState;
    }

    // 순수 함수(Func)를 통째로 넘겨받아서 기존 상태를 집어넣고 새 상태를 뽑아냅니다.
    public void Dispatch(Func<DeckBuildUIState, DeckBuildUIState> pureFunction)
    {
        var nextState = pureFunction(State);

        if (State != nextState)
        {
            State = nextState;
            OnStateChanged?.Invoke(State);
        }
    }
}