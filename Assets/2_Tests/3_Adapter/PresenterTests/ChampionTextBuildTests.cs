using NUnit.Framework;
using static TestHelper;

public class ChampionTextBuildTests
{
    class FakeChampionProvider : IChampionProvider
    {
        public ChampionProfile ProfileToReturn;
        public FakeChampionProvider(ChampionProfile profile) => ProfileToReturn = profile;
        public ChampionProfile GetProfile(int id) => ProfileToReturn;
    }


    [Test]
    public void Id를_받으면_데이터_조회_후_텍스트를_생성한다()
    {
        var stat = CreateStat(1, 2, 3);
        var profile = new ChampionProfile(1, "펭귄", stat, CreateSkill(CreateValueSkillData(StatType.Attack, 0, rule: SelfAllRule)));
        var sut = new ChampionTextBuilder(new FakeChampionProvider(profile), new SkillTextBuilder(new FakeTextBuilder()), new ChampionStatusTextBuilder());

        var result = sut.Build(1);

        Assert.AreEqual("펭귄", result.Name);
        Assert.AreEqual("공 1", result.StatModel.Attack);
        Assert.AreEqual("방 2", result.StatModel.Defense);
        Assert.AreEqual("속도 3", result.StatModel.Speed);
        Assert.AreEqual("아군 전체 액숀", result.SkillText);
    }
}