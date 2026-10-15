using NUnit.Framework;
using static TestHelper;
using UnityEngine;

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

    [Test]
    public void 업그레이드가_존재하면_초록색_반환()
    {
        var inventory = CreateMasteryInventory(10);
        var sut = CreateSut(inventory, new FakeMasterySaver());

        sut.SelectChampion(1);
        sut.UpgradeCardStat(StatType.Attack);

        Assert.AreEqual(Color.green, sut.GetButtonColor(1));
        Assert.AreEqual(Color.white, sut.GetButtonColor(2));
    }

    [Test]
    public void 업그레이드한_카드는_스탯이_모델에_반영되어야_함()
    {
        int champId = 1;
        var inventory = CreateMasteryInventory(10);
        var textBuilder = new ChampionTextBuilder(
            id => new ChampionProfile(id, "펭귄", CreateStat(150, 0, 0), CreateSkill()),
            new SkillTextBuilder(new FakeTextBuilder())
        );
        var applier = CreateMasteryApplier(CreateMasteryData(champId, att:30));
        var sut = new MasteryPointPresenter(inventory, textBuilder, new FakeMasterySaver(), applier);

        // Act: 1번 챔피언 선택 후 공격력 업그레이드
        sut.SelectChampion(champId);
        sut.UpgradeCardStat(StatType.Attack);
        var result = sut.CreateModel();

        // Assert: 공격력 강화 적용 됐는지 검증(150 + 30)
        Assert.AreEqual("공 180", result.ChampionModel.StatModel.Attack);
    }
}