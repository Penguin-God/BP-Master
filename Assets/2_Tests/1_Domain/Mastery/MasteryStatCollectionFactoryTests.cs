using NUnit.Framework;
using static TestHelper;
using System.Collections.Generic;

public class MasteryStatCollectionFactoryTests
{
    [Test]
    public void 숙련도_보드_컬랙션을_스탯_컬랙션으로_변환()
    {
        var multiple = new MasteryMultiplier(15, 15, 1);
        var sut = new MasteryStatCollectionFactory(multiple);

        var result = sut.Create(CreateBoardCollection(new() { { 1, new MasteryBoard(1, 1, 1) } }));
        
        Assert.AreEqual(CreateStat(15, 15, 1), result.GetMasteryStat(1));
        Assert.AreEqual(CreateStat(), result.GetMasteryStat(100));
    }
}
