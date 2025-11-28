using NUnit.Framework;
using static TestHelper;

public class AI_SimulateMainTests
{
    [Test]
    public void AI_배틀을_실행시키고_결과를_반환해야_함()
    {
        int teamSize = 2;
        var phases = new PhaseData[] {
            CreatePhaseData(GamePhase.Ban, Team.Blue, Team.Red),
            CreatePhaseData(GamePhase.Pick, Team.Blue, Team.Red, Team.Red, Team.Blue),
            CreatePhaseData(GamePhase.Skill, Team.Blue, Team.Red, Team.Red, Team.Blue) 
        };
        Champion[] champions = new Champion[] { CreateChampion(10), CreateChampion(20), CreateChampion(30), CreateChampion(100), CreateChampion(100), CreateChampion(60), };

        var sut = new AI_SimulateMain(phases, teamSize, champions);

        MatchInfo result = sut.Run();

        Assert.AreEqual(Team.Red, result.Winner);
        Assert.AreEqual(380, result.BlueScore);
        Assert.AreEqual(600, result.RedScore);
    }

    Champion CreateChampion(int skillValue) => new Champion(CreateValueSkill(SkillType.AttackChanger, 10, default, SelfAllRule), CreateStatus(att: 100));
}
