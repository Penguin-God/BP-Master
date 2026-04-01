using System;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public struct MatchDataConfig
{
    [HorizontalGroup("Match", LabelWidth = 30)]
    public int Id1;

    [HorizontalGroup("Match", LabelWidth = 30)]
    public int Id2;

    public MatchData CreateData() => new MatchData(Id1, Id2);
}

[CreateAssetMenu(fileName = "LeagueScheduleSO", menuName = "BP Master/LeagueScheduleSO")]
public class LeagueScheduleSO : ScriptableObject
{
    [SerializeField] MatchDataConfig[] matches;

    public ScheduleFlow CreateFlow(int startIndex) => new ScheduleFlow(matches.Select(x => x.CreateData()), startIndex);
}