using System;
using System.Collections.Generic;
using System.Linq;

public class MasteryInventory
{
    public int AvailablePoints { get; protected set; }
    readonly Dictionary<int, MasteryBoard> boards = new Dictionary<int, MasteryBoard>();
    public IReadOnlyDictionary<int, MasteryBoard> Boards => boards;

    public MasteryInventory(IEnumerable<int> championIds, int startPoints = 0) : this(startPoints, championIds.ToDictionary(x => x, x => new MasteryBoard())) { }

    public MasteryInventory(int point, Dictionary<int, MasteryBoard> savedBoards)
    {
        AvailablePoints = point;
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