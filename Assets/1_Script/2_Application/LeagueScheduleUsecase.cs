
public class LeagueScheduleUsecase
{
    readonly ScheduleFlow _flow;
    readonly int _playerId;
    readonly IBattleResolver _userBattleResolver;
    readonly IBattleResolver _aiResolver;

    public LeagueScheduleUsecase(ScheduleFlow flow, int playerId, IBattleResolver sceneLoader, IBattleResolver aiResolver)
    {
        _flow = flow;
        _playerId = playerId;
        _userBattleResolver = sceneLoader;
        _aiResolver = aiResolver;
    }

    public void ProcessNextMatch()
    {
        if (_flow.IsFinished) return;

        var currentMatch = _flow.Advance();
        
        if (IsPlayerMatch(currentMatch))
            _userBattleResolver.Resolve(currentMatch);
        else
            _aiResolver.Resolve(currentMatch);
    }

    bool IsPlayerMatch(MatchData match) => match.Id1 == _playerId || match.Id2 == _playerId;
}