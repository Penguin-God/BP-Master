using NUnit.Framework;
using static TestHelper;

public class ChampionTextBuildTests
{
    [Test]
    public void Id를_받으면_데이터_조회_후_텍스트를_생성한다()
    {
        var stat = CreateStat(1, 2, 3);
        var profile = new ChampionProfile(1, "펭귄", stat, System.Array.Empty<SkillViewModel>());
        var sut = new ChampionTextBuilder(id => profile, skills => "아군 전체 공격력 5 증가");

        var result = sut.Build(1, default);

        Assert.AreEqual("펭귄", result.Name);
        Assert.AreEqual("공 1", result.StatModel.Attack);
        Assert.AreEqual("방 2", result.StatModel.Defense);
        Assert.AreEqual("속도 3", result.StatModel.Speed);
        Assert.AreEqual("아군 전체 공격력 5 증가", result.SkillText);
    }
}