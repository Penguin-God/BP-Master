using NUnit.Framework;
using System.Collections.Generic;

public class MasteryPointPersenterTests
{
    [Test]
    public void Id를_주면_숙련도_레벨_모델을_반환()
    {
        var boards = new Dictionary<int, MasteryBoard>
        {
            { 100, new MasteryBoard(attackLevel: 1) }
        };
        var inven = new MasteryInventory(point: 10, boards);

        var sut = new MasteryPointPresenter(inven);

        var result = sut.GetMasteryPointModel(100);

        Assert.AreEqual("공격Lv : 1", result.Attack);
        Assert.AreEqual("방어Lv : 0", result.Defense);
        Assert.AreEqual("속도Lv : 0", result.Speed);
    }
}
