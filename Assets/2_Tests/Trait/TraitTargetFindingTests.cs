using NUnit.Framework;
using System.Linq;

public class TraitTargetFindingTests
{
    [Test]
    public void 싱글은_단일_index_반환()
    {
        var sut = new TraitTargetSeletor(3);

        Assert.AreEqual(1, sut.GetTargetIds(Team.Blue, TargetRange.Single, 1).First());
        Assert.AreEqual(2, sut.GetTargetIds(Team.Red, TargetRange.Single, 2).First());
    }

    [Test]
    public void All은_전체_index_반환()
    {
        var sut = new TraitTargetSeletor(3);

        CollectionAssert.AreEqual(new int[] { 0, 1, 2 }, sut.GetTargetIds(Team.Red, TargetRange.All, 0));
    }
}
