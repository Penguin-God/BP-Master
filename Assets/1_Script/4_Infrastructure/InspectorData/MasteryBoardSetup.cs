using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public struct MasteryBoardData
{
    [MinValue(0)] public int Id;
    [MinValue(0)] [SerializeField] int AttackLevel;
    [MinValue(0)] [SerializeField] int DefenseLevel;
    [MinValue(0)] [SerializeField] int SpeedLevel;

    public MasteryBoard CreateBoard() => new MasteryBoard(AttackLevel, DefenseLevel, SpeedLevel);
}

[Serializable]
public class MasteryBoardSetup
{
    [TableList(ShowIndexLabels = true, AlwaysExpanded = true)] [SerializeField] MasteryBoardData[] _masteryBoardDatas;

    public IReadOnlyDictionary<int, MasteryBoard> CreateBoards() => _masteryBoardDatas.ToDictionary(x => x.Id, x => x.CreateBoard());
}