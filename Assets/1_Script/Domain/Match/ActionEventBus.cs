using System;
using System.Collections.Generic;

public sealed class ActionEventBus
{
    Team _currentTeam;
    readonly HashSet<Team> teamSubitted = new();
    public event Action OnActionDone;

    public void ChangeTeam(Team team)
    {
        _currentTeam = team;
        teamSubitted.Clear();
    }

    public bool ActionDone(Team actingTeam)
    {
        if (_currentTeam == Team.All)
        {
            teamSubitted.Add(actingTeam);
            if(teamSubitted.Contains(Team.Blue) && teamSubitted.Contains(Team.Red))
            {
                OnActionDone?.Invoke();
                teamSubitted.Clear();
                return true;
            }
            else return false;
        }

        // 단일 팀일 경우
        if (actingTeam == _currentTeam)
        {
            OnActionDone?.Invoke();
            return true;
        }
        else return false;
    }
}
