using System;
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
public class PlayerDataInspector
{
    public int Id;
    public string Name;
    [TableList(ShowIndexLabels = true, AlwaysExpanded = true)] [SerializeField] MasteryBoardData[] _masteryBoardDatas;

    public MasteryBoardCollection CreateBoardCollection() => new MasteryBoardCollection(_masteryBoardDatas.ToDictionary(x => x.Id, x => x.CreateBoard()));

    public PlayerData ToData() => new PlayerData(Id, Name, CreateBoardCollection());
}