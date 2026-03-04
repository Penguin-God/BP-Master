using NUnit.Framework;
using System;

public class MasteryBoardTest
{
    MasteryBoard CreateBoard()
    {
        return new MasteryBoard();
    }

    [Test]
    public void 스탯별_숙련도를_올리면_레벨이_오른다()
    {
        var board = CreateBoard();

        Assert.AreEqual(0, board.AttackLevel);
        Assert.AreEqual(0, board.DefenseLevel);
        Assert.AreEqual(0, board.SpeedLevel);

        board.Upgrade(StatType.Attack);
        Assert.AreEqual(1, board.AttackLevel);

        board.Upgrade(StatType.Defense);
        Assert.AreEqual(1, board.DefenseLevel);

        board.Upgrade(StatType.Speed);
        Assert.AreEqual(1, board.SpeedLevel);
    }

    [Test]
    public void 만렙인_1레벨을_초과하여_숙련도를_올리려고_하면_예외를_던진다()
    {
        var board = CreateBoard();
        board.Upgrade(StatType.Attack);

        Assert.Throws<InvalidOperationException>(() => board.Upgrade(StatType.Attack));
    }
}