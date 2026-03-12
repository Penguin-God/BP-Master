using NUnit.Framework;
using System.Collections.Generic;

public class InventoryMasteryConverterTests
{
    [Test]
    public void 인벤토리에서_id를_찾아_보드_레벨과_배율을_곱해_변환한다()
    {
        var boards = new Dictionary<int, MasteryBoard>() { { 101, new MasteryBoard(attackLevel: 1, speedLevel: 1) } };
        var multiplier = new MasteryMultiplier(Attack: 15, Defense: 15, Speed: 2);
        var sut = new InventoryMasteryConverter(boards, multiplier);

        var result = sut.GetMasteryStat(101);

        Assert.AreEqual(15, result.Attack);
        Assert.AreEqual(0, result.Defense);
        Assert.AreEqual(2, result.Speed);

        // 없는 챔프는 기본값
        result = sut.GetMasteryStat(0);
        Assert.AreEqual(0, result.Attack);
        Assert.AreEqual(0, result.Defense);
        Assert.AreEqual(0, result.Speed);
    }
}