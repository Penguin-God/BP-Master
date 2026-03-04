using System;
using System.Collections.Generic;

public class MasteryInventory
{
    public int AvailablePoints { get; protected set; }
    readonly Dictionary<int, MasteryBoard> boards = new Dictionary<int, MasteryBoard>();

    public MasteryInventory(IEnumerable<int> championIds, int startPoints = 0)
    {
        AvailablePoints = startPoints;
        foreach (var id in championIds)
            boards[id] = new MasteryBoard();
    }

    public MasteryInventory(int savedPoints, Dictionary<int, MasteryBoard> savedBoards)
    {
        AvailablePoints = savedPoints;
        boards = savedBoards;
    }

    public MasteryBoard GetBoard(int championId)
    {
        if (boards.TryGetValue(championId, out var board))
            return board;

        throw new KeyNotFoundException($"해당 ID({championId})의 챔피언 숙련도 데이터가 없습니다.");
    }

    public void Upgrade(int championId, StatType statType)
    {
        if (AvailablePoints <= 0)
            throw new InvalidOperationException("포인트가 부족합니다.");

        var board = GetBoard(championId);
        board.Upgrade(statType);
        AvailablePoints--;
    }
}