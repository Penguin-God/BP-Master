using NUnit.Framework;
using System.Collections.Generic;

public class MasteryBoardTest
{
    MasteryBoard CreateBoard() => new MasteryBoard();

    [Test]
    public void 스탯별_숙련도를_올리면_레벨이_오르고_true를_반환한다()
    {
        var board = CreateBoard();

        Assert.IsTrue(board.Upgrade(StatType.Attack));
        Assert.AreEqual(1, board.AttackLevel);
        Assert.AreEqual(0, board.DefenseLevel);
        Assert.AreEqual(0, board.SpeedLevel);

        Assert.IsTrue(board.Upgrade(StatType.Defense));
        Assert.AreEqual(1, board.DefenseLevel);

        Assert.IsTrue(board.Upgrade(StatType.Speed));
        Assert.AreEqual(1, board.SpeedLevel);
        Assert.IsTrue(board.IsUpgrade);
    }

    [Test]
    public void 스탯별_숙련도를_내리면_레벨이_내려가고_true를_반환한다()
    {
        var board = new MasteryBoard(attackLevel: 1, defenseLevel: 1, speedLevel: 1);

        Assert.IsTrue(board.Downgrade(StatType.Attack));
        Assert.AreEqual(0, board.AttackLevel);
        Assert.AreEqual(1, board.DefenseLevel);
        Assert.AreEqual(1, board.SpeedLevel);

        Assert.IsTrue(board.Downgrade(StatType.Defense));
        Assert.AreEqual(0, board.DefenseLevel);

        Assert.IsTrue(board.Downgrade(StatType.Speed));
        Assert.AreEqual(0, board.SpeedLevel);
    }

    [Test]
    public void 최대_레벨에서_숙련도를_올리면_레벨이_유지되고_false를_반환한다()
    {
        var board = new MasteryBoard(attackLevel: 1);

        bool isChanged = board.Upgrade(StatType.Attack);

        Assert.IsFalse(isChanged);
        Assert.AreEqual(1, board.AttackLevel);
    }

    [Test]
    public void 최소_레벨에서_숙련도를_내리면_레벨이_유지되고_false를_반환한다()
    {
        var board = CreateBoard();

        bool isChanged = board.Downgrade(StatType.Attack);

        Assert.IsFalse(isChanged);
        Assert.AreEqual(0, board.AttackLevel);
        Assert.IsFalse(board.IsUpgrade);
    }
}