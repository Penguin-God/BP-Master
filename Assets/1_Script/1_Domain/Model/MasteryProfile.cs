using System;
using System.Collections.Generic;
using System.Linq;

public class MasteryProfile
{
    public int AvailablePoints { get; protected set; }
    public MasteryBoardCollection BoardCollection { get; }

    public MasteryProfile(IEnumerable<int> championIds, int startPoints = 0)
        : this(startPoints, new MasteryBoardCollection(championIds.ToDictionary(x => x, x => new MasteryBoard()))) {}

    public MasteryProfile(int point, MasteryBoardCollection boards)
    {
        AvailablePoints = point;
        BoardCollection = boards;
    }

    public void Upgrade(int championId, StatType statType)
    {
        if (AvailablePoints <= 0)
            throw new InvalidOperationException("포인트가 부족합니다.");

        var board = BoardCollection.GetBoard(championId);
        board.Upgrade(statType);
        AvailablePoints--;
    }

    public MasteryBoard GetBoard(int championId) => BoardCollection.GetBoard(championId);
}