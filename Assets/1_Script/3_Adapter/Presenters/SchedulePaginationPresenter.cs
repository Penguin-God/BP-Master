using System;
using System.Collections.Generic;

public enum MatchState
{
    Past,
    Current,
    Player,
    Normal
}

public struct MatchDisplayModel
{
    public readonly int MatchIndex;
    public readonly MatchData Match;
    public readonly MatchState State;

    public MatchDisplayModel(int matchIndex, MatchData match, MatchState state)
    {
        MatchIndex = matchIndex;
        Match = match;
        State = state;
    }
}

public class SchedulePaginationPresenter
{
    readonly ScheduleFlow _flow;
    readonly int _playerId;
    const int ItemsPerPage = 10;

    public int CurrentPage { get; private set; }

    int MaxPage => Math.Max(0, (_flow.Matches.Count - 1) / ItemsPerPage);

    public SchedulePaginationPresenter(ScheduleFlow flow, int playerId)
    {
        _flow = flow;
        _playerId = playerId;
        ResetToCurrentPage();
    }

    public void ResetToCurrentPage() => CurrentPage = _flow.CurrentIndex / ItemsPerPage;

    public void NextPage()
    {
        if (CurrentPage < MaxPage) 
            CurrentPage++;
    }

    public void PrevPage()
    {
        if (CurrentPage > 0) 
            CurrentPage--;
    }

    public IReadOnlyList<MatchDisplayModel> GetCurrentPageData()
    {
        var result = new List<MatchDisplayModel>();

        int start = CurrentPage * ItemsPerPage;
        int end = Math.Min(start + ItemsPerPage, _flow.Matches.Count);

        for (int i = start; i < end; i++)
        {
            MatchState state = DetermineState(i, _flow.Matches[i]);
            result.Add(new MatchDisplayModel(matchIndex: i, match: _flow.Matches[i], state));
        }

        return result;
    }

    MatchState DetermineState(int index, MatchData match)
    {
        if (index < _flow.CurrentIndex) return MatchState.Past;
        if (index == _flow.CurrentIndex) return MatchState.Current;
        if (match.Id1 == _playerId || match.Id2 == _playerId) return MatchState.Player;

        return MatchState.Normal;
    }
}