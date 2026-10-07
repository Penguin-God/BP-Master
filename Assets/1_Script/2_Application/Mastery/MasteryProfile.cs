using System;

public class MasteryProfile
{
    public int TotalPoints { get; private set; }
    public int AvailablePoints { get; private set; }
    public MasteryBoardCollection BoardCollection { get; }

    public MasteryProfile(int startPoints = 0) : this(startPoints, startPoints, new MasteryBoardCollection(new())) { }
    public MasteryProfile(int totalPoints, int availablePoints, MasteryBoardCollection boards)
    {
        TotalPoints = totalPoints;
        AvailablePoints = availablePoints;
        BoardCollection = boards;
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
    public bool TryGetBoard(int championId, out MasteryBoard board) => BoardCollection.TryGetBoard(championId, out board);

    public void EarnPoints(int amount)
    {
        if (amount < 0) throw new ArgumentException("획득 포인트는 음수일 수 없습니다.");
        TotalPoints += amount;
        AvailablePoints += amount;
    }

    public void ResetAll()
    {
        BoardCollection.Clear();
        AvailablePoints = TotalPoints;
    }
}