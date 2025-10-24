using NUnit.Framework;

[TestFixture]
public class GameFlowTextBuilderTests
{
    [TestCase(GamePhase.Ban, Team.Blue, "파랑 팀 밴 단계")]
    [TestCase(GamePhase.Pick, Team.Red, "빨강 팀 픽 단계")]
    [TestCase(GamePhase.Swap, Team.All, "양팀 스왑 단계")]
    [TestCase(GamePhase.Skill, Team.Blue, "파랑 팀 특성 단계")]
    [TestCase(GamePhase.Done, Team.Red, "빨강 팀 끝 단계")]
    public void 각_단계와_팀_조합에_맞는_텍스트_생성(GamePhase phase, Team team, string expected)
    {
        var sut = new GameFlowTextBuilder();
        var flow = new GameFlowData(phase, team);

        var result = sut.BuildFlowText(flow);

        Assert.AreEqual(expected, result);
    }
}