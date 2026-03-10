using System.Collections.Generic;
using System.Linq;

public record MatchData(int Id1, int Id2)
{
    public IEnumerable<int> All_Ids => new int[] { Id1, Id2 };
    public int GetOpponentId(int id) => All_Ids.Except(new int[] { id }).First();
};

public class ScheduleFlow
{
    readonly Queue<MatchData> matches;

    public ScheduleFlow(IEnumerable<MatchData> matchDatas) => matches = new Queue<MatchData>(matchDatas);

    public MatchData PeekMatch => matches.Peek();
    public bool IsFinished => matches.Count == 0;

    public MatchData Advance() => matches.Dequeue();
}