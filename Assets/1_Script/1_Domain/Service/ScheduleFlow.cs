using System.Collections.Generic;

public record MatchData(int Id1, int Id2);

public class ScheduleFlow
{
    readonly Queue<MatchData> matches;

    public ScheduleFlow(IEnumerable<MatchData> matchDatas) => matches = new Queue<MatchData>(matchDatas);

    public MatchData PeekMatch => matches.Peek();
    public bool IsFinished => matches.Count == 0;

    public MatchData Advance() => matches.Dequeue();
}