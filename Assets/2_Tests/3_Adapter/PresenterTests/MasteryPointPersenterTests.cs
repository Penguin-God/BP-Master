using NUnit.Framework;
using System;
using static TestHelper;

public class MasteryPointPersenterTests
{
    class FakeMasteryPointView : IMasteryPointView
    {
        public int UpdatedPoints { get; private set; }
        public ChampionTextModel ChampModel { get; private set; }
        public MasteryLevelModel MasteryModel { get; private set; }

        public void UpdatePoints(int points) => UpdatedPoints = points;

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

    class FakeChampionProvider : IChampionProvider
    {
        public ChampionProfile GetProfile(int id) => new ChampionProfile(id, "펭귄", CreateStat(0, 0, 0), CreateSkill());
    }

    MasteryPointPresenter CreateSut(MasteryProfile inventory, FakeMasteryPointView view, FakeMasterySaver saver)
    {
        var textBuilder = new ChampionTextBuilder(
            new FakeChampionProvider(),
            new SkillTextBuilder(new FakeTextBuilder()),
            new ChampionStatusTextBuilder()
        );
        return new MasteryPointPresenter(inventory, textBuilder, view, saver);
    }

    [Test]
    public void 초기화_시_보유_포인트를_뷰에_전달한다()
    {
        var inventory = CreateMasteryInventory(10);
        var view = new FakeMasteryPointView();
        var sut = CreateSut(inventory, view, new FakeMasterySaver());

        sut.Initialize();

        Assert.AreEqual(10, view.UpdatedPoints);
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
    public void 숙련도_업그레이드_성공_시_인벤토리가_저장되고_뷰가_갱신된다()
    {
        var inventory = CreateMasteryInventory(10);
        var view = new FakeMasteryPointView();
        var saver = new FakeMasterySaver();
        var sut = CreateSut(inventory, view, saver);

        sut.SelectChampion(1);
        sut.RequestUpgrade(StatType.Attack);

        Assert.IsTrue(saver.IsSaved);
        Assert.AreEqual(9, view.UpdatedPoints);
        Assert.AreEqual("공 : 1", view.MasteryModel.AttackText);
    }

    [Test]
    public void 포인트_없이_업그레이드_시도할_경우_예외()
    {
        var inventory = CreateMasteryInventory(0);
        var view = new FakeMasteryPointView();
        var saver = new FakeMasterySaver();
        var sut = CreateSut(inventory, view, saver);

        sut.SelectChampion(1);
        
        Assert.Throws<InvalidOperationException>(() => sut.RequestUpgrade(StatType.Attack));
        Assert.IsFalse(saver.IsSaved);
    }
}
