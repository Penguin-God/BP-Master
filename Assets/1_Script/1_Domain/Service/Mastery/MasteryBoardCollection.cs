using System.Collections.Generic;

public class MasteryBoardCollection
{
    readonly IReadOnlyDictionary<int, MasteryBoard> _boards;

    public IReadOnlyDictionary<int, MasteryBoard> AllBoards => _boards;
    public MasteryBoardCollection(IReadOnlyDictionary<int, MasteryBoard> boards) => _boards = boards;
    public bool TryGetBoard(int championId, out MasteryBoard board) => _boards.TryGetValue(championId, out board);

    public MasteryBoard GetBoard(int championId)
    {
        if (_boards.TryGetValue(championId, out var board))
            return board;

        throw new KeyNotFoundException($"해당 ID({championId})의 챔피언 숙련도 데이터가 없습니다.");
    }
}