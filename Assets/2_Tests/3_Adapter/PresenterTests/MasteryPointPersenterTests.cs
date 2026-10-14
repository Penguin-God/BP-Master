using NUnit.Framework;
using static TestHelper;

public class MasteryPointPersenterTests
{
    class FakeMasteryPointView : IMasteryPointView
    {
        public ChampionTextModel ChampModel { get; private set; }
        public MasteryLevelModel MasteryModel { get; private set; }

        public void UpdateChampionDetail(ChampionTextModel champModel, MasteryLevelModel masteryModel)
        {
            ChampModel = champModel;
            MasteryModel = masteryModel;
        }
    }

    class FakeMasterySaver : IMasterySaver
    {
        public bool IsSaved { get; private set; }
        public void Save(MasteryProfile inventory) => IsSaved = true;
    }

    MasteryPointPresenter CreateSut(MasteryProfile inventory, FakeMasteryPointView view, FakeMasterySaver saver)
    {
        var textBuilder = new ChampionTextBuilder(id => new ChampionProfile(id, "펭귄", CreateStat(0, 0, 0), CreateSkill()), new SkillTextBuilder(new FakeTextBuilder()));
        return new MasteryPointPresenter(inventory, textBuilder, view, saver);
    }

    [Test]
    public void 챔피언_선택_시_상세_정보를_갱신한다()
    {
        var inventory = CreateMasteryInventory(10);
        var view = new FakeMasteryPointView();
        var sut = CreateSut(inventory, view, new FakeMasterySaver());

        sut.SelectChampion(1);

        Assert.AreEqual("펭귄", view.ChampModel.Name);
        Assert.AreEqual("공 : 0", view.MasteryModel.AttackText);
    }

    [Test]
    public void 숙련도_업그레이드_성공_시_인벤토리가_저장되고_현재_포인트를_반환한다()
    {
        var inventory = CreateMasteryInventory(10);
        var view = new FakeMasteryPointView();
        var saver = new FakeMasterySaver();
        var sut = CreateSut(inventory, view, saver);

        sut.SelectChampion(1);
        var point = sut.UpgradeCardStat(StatType.Attack);

        Assert.IsTrue(saver.IsSaved);
        Assert.AreEqual(9, point);
    }
}