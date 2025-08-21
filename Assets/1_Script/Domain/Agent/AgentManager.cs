using System;
using System.Collections.Generic;

public class AgentManager
{
    readonly GameBanPickStorage gameBanPickStorage;

    public AgentManager(GameBanPickStorage gameBanPickStorage)
    {
        this.gameBanPickStorage = gameBanPickStorage;
    }

    public event Action OnActionDone;
    GamePhase currentPhase = GamePhase.Done;
    readonly HashSet<Team> swapSubmitted = new();

    public void ChangePhase(GamePhase phase)
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
