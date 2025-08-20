using System;

public class ActionAgent
{
    readonly TeamBanPickStorage storage;
    public event Action OnActionDone;

    public ActionAgent(TeamBanPickStorage storage)
    {
        this.storage = storage;
    }

    public void Ban(int id)
    {
        storage.SaveSelect(SelectType.Ban, id);
        NotifyDone();
    }

    public void Pick(int id)
    {
        storage.SaveSelect(SelectType.Pick, id);
        NotifyDone();
    }

    public void Swap(int index1, int index2)
    {
        storage.Swap(index1, index2);
    }

    public void SwapDone()
    {
        NotifyDone();
    }

    void NotifyDone() => OnActionDone?.Invoke();
}
