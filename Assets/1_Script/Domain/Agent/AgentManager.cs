using System;
using System.Collections.Generic;

public class AgentManager
{
    private ActionAgent blue;
    private ActionAgent red;
    private GameBanPickStorage gameBanPickStorage;

    public AgentManager(ActionAgent blue, ActionAgent red, GameBanPickStorage gameBanPickStorage)
    {
        this.blue = blue;
        this.red = red;
        this.gameBanPickStorage = gameBanPickStorage;
    }

    public event Action OnActionDone;
    GamePhase currentPhase = GamePhase.Done;
    readonly HashSet<Team> swapSubmitted = new();

    public void PhaseChange(GamePhase phase)
    {
        currentPhase = phase;
    }

    public void SwapDone(Team team)
    {
        if (currentPhase != GamePhase.Swap) return;
        swapSubmitted.Add(team);
        if (swapSubmitted.Contains(Team.Blue) && swapSubmitted.Contains(Team.Red))
        {
            OnActionDone?.Invoke();
            swapSubmitted.Clear();
        }
    }

    public void Ban(Team team, int id) => SaveSelect(new SelectInfo(team, SelectType.Ban, id), GamePhase.Ban);
    public void Pick(Team team, int id) => SaveSelect(new SelectInfo(team, SelectType.Pick, id), GamePhase.Pick);

    void SaveSelect(SelectInfo selectInfo, GamePhase phase)
    {
        if (currentPhase != phase) return;

        gameBanPickStorage.SaveSelect(selectInfo);
        OnActionDone?.Invoke();
    }
}
