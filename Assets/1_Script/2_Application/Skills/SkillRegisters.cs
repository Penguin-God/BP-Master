using System;

public class ImmediateTrigger : ISkillRegister
{
    public void Register(Action<ChampionStatus> execute) => execute(null);
}

public class OnPhaseDoneTrigger : ISkillRegister
{
    readonly PhaseEventDispatcher dispatcher;
    public OnPhaseDoneTrigger(PhaseEventDispatcher dispatcher) => this.dispatcher = dispatcher;
    public void Register(Action<ChampionStatus> execute) => dispatcher.OnPhaseDone += () => execute(null);
}

public class OnAllyPickChampionTrigger : ISkillRegister
{
    readonly BanPickEventDispatcher eventDispatcher;
    readonly Team team;

    public OnAllyPickChampionTrigger(BanPickEventDispatcher eventDispatcher, Team team)
    {
        this.eventDispatcher = eventDispatcher;
        this.team = team;
    }

    public void Register(Action<ChampionStatus> execute)
    {
        eventDispatcher.OnTeamChampionPick += (champion, pickedTeam) =>
        {
            if (pickedTeam == team)
                execute(champion.Status);
        };
    }
}

public class OnOpponentPickChampionTrigger : ISkillRegister
{
    readonly BanPickEventDispatcher eventDispatcher;
    readonly Team team;

    public OnOpponentPickChampionTrigger(BanPickEventDispatcher eventDispatcher, Team team)
    {
        this.eventDispatcher = eventDispatcher;
        this.team = team;
    }

    public void Register(Action<ChampionStatus> execute)
    {
        eventDispatcher.OnTeamChampionPick += (champion, pickedTeam) =>
        {
            if (pickedTeam != team)
                execute(champion.Status);
        };
    }
}