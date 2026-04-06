using System;

public class MasteryProfile
{
    public int AvailablePoints { get; protected set; }
    public MasteryBoardCollection BoardCollection { get; }

    public MasteryProfile(int startPoints = 0) : this(startPoints, new MasteryBoardCollection(new())) { }
    public MasteryProfile(int point, MasteryBoardCollection boards)
    {
        AvailablePoints = point;
        BoardCollection = boards;
        UnityEngine.Debug.Log(BoardCollection.AllBoards);
    }

    public void Upgrade(int championId, StatType statType)
    {
        if (AvailablePoints <= 0)
            throw new InvalidOperationException("포인트가 부족합니다.");

        var board = BoardCollection.GetOrCreateBoard(championId);
        board.Upgrade(statType);
        AvailablePoints--;
    }

    public MasteryBoard GetBoard(int championId) => BoardCollection.TryGetBoard(championId, out var result) ? result : new MasteryBoard();

    public bool TryGetBoard(int championId, out MasteryBoard board)
    {
        if (BoardCollection.TryGetBoard(championId, out board)) return true;
        else return false;
    }
}