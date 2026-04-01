using System.Collections.Generic;
using System.Linq;

public record MatchData(int Id1, int Id2)
{
    public IEnumerable<int> All_Ids => new int[] { Id1, Id2 };
    public int GetOpponentId(int id) => All_Ids.Except(new int[] { id }).First();
};


public class ScheduleFlow
{
    readonly MatchData[] _matches;
    public int CurrentIndex { get; private set; }

    public ScheduleFlow(IEnumerable<MatchData> matchDatas, int startIndex = 0)
    {
        _matches = matchDatas.ToArray();
        CurrentIndex = startIndex;
    }

    public MatchData CurrentMatch => _matches[CurrentIndex];
    public bool IsFinished => CurrentIndex >= _matches.Length;

    public MatchData Advance()
    {
        var match = _matches[CurrentIndex];
        CurrentIndex++;
        return match;
    }
}