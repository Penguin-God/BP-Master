using System;

public class ActionAgent
{
    readonly TeamBanPickStorage storage;
    public event Action OnActionDone;

    public ActionAgent(TeamBanPickStorage storage)
    {
        this.storage = storage;
    }

    GamePhase phase;
    public void OnRequestAction(GamePhase phase)
    {
        this.phase = phase;
    }

    public void Done()
    {
        switch (phase)
        {
            case GamePhase.Ban: storage.SaveSelect(SelectType.Ban, 0); break;
            case GamePhase.Pick: storage.SaveSelect(SelectType.Pick, 0); break;
        }

        OnActionDone?.Invoke();
    }
}
