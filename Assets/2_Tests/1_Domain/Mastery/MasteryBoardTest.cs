using NUnit.Framework;
using System;
using System.Collections.Generic;

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

    [Test]
    public void 보드를_생성자에_넣으면_상태가_반영된다()
    {
        var savedBoards = new Dictionary<int, MasteryBoard>
        {
            { 101, new MasteryBoard(attackLevel: 1, defenseLevel: 0, speedLevel: 1) },
            { 102, new MasteryBoard(attackLevel: 0, defenseLevel: 1, speedLevel: 0) }
        };
        var inventory = new MasteryInventory(savedPoints: 15, savedBoards);

        var board101 = inventory.GetBoard(101);
        var board102 = inventory.GetBoard(102);

        Assert.AreEqual(15, inventory.AvailablePoints);
        Assert.AreEqual(1, board101.AttackLevel);
        Assert.AreEqual(1, board101.SpeedLevel);
        Assert.AreEqual(1, board102.DefenseLevel);
    }
}