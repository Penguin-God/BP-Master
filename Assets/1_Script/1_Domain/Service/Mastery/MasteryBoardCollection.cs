using System.Collections.Generic;

public class MasteryBoardCollection
{
    readonly Dictionary<int, MasteryBoard> _boards;

    public IReadOnlyDictionary<int, MasteryBoard> AllBoards => _boards;
    public MasteryBoardCollection(Dictionary<int, MasteryBoard> boards) => _boards = boards;
    public bool TryGetBoard(int championId, out MasteryBoard board) => _boards.TryGetValue(championId, out board);

    public MasteryBoard GetOrCreateBoard(int championId)
    {
        if (_boards.TryGetValue(championId, out var board) == false)
        {
            board = new MasteryBoard();
            _boards.Add(championId, board);
        }
        return board;
    }
}