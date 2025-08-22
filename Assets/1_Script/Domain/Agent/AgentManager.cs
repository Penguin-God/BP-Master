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
    Team currentTeam;
    readonly HashSet<Team> swapSubmitted = new();

    public void ChangePhase(GamePhase phase, Team turn)
    {
        currentPhase = phase;
        currentTeam = turn;
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

        if (currentPhase != phase || selectInfo.Team != currentTeam)
        {
            UnityEngine.Debug.Log($"옳지 않은 접근{phase}, {selectInfo.Team}. 실제 {currentPhase}, {currentTeam}");
            return;
        }

        gameBanPickStorage.SaveSelect(selectInfo);
        OnActionDone?.Invoke();
    }
}
