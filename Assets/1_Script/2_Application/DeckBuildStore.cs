using System;

public class DeckBuildStore
{
    public DeckBuildState State { get; private set; }
    public event Action<DeckBuildState> OnStateChanged;
    public DeckBuildStore(DeckBuildState initialState) => State = initialState;

    public void Dispatch(Func<DeckBuildState, DeckBuildState> pureFunction)
    {
        var nextState = pureFunction(State);
        if (State != nextState)
        {
            State = nextState;
            OnStateChanged?.Invoke(State);
        }
    }
}