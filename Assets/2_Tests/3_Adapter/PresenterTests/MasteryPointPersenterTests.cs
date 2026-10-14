using NUnit.Framework;
using static TestHelper;

public class MasteryPointPersenterTests
{
    class FakeMasterySaver : IMasterySaver
    {
        public bool IsSaved { get; private set; }
        public void Save(MasteryProfile inventory) => IsSaved = true;
    }

    MasteryPointPresenter CreateSut(MasteryProfile inventory, FakeMasterySaver saver)
    {
        var textBuilder = new ChampionTextBuilder(id => new ChampionProfile(id, "펭귄", CreateStat(0, 0, 0), CreateSkill()), new SkillTextBuilder(new FakeTextBuilder()));
        return new MasteryPointPresenter(inventory, textBuilder, saver);
    }

    [Test]
    public void 숙련도_업그레이드_성공_시_인벤토리가_저장되고_현재_포인트를_반환한다()
    {
        var inventory = CreateMasteryInventory(10);
        var saver = new FakeMasterySaver();
        var sut = CreateSut(inventory, saver);

        sut.SelectChampion(1);
        var point = sut.UpgradeCardStat(StatType.Attack);

        Assert.IsTrue(saver.IsSaved);
        Assert.AreEqual(9, point);
    }
}