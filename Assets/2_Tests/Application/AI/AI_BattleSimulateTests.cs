using NUnit.Framework;
using static TestHelper;

public class AI_BattleSimulateTests
{
    [Test]
    public void 주입한_AI로_밴픽_진행_후_결과_반환()
    {
        var eventDispatcher = new PhaseEventDispatcher();
        var phaseManager = CreatePhaseManager(eventDispatcher,
            CreatePhaseData(GamePhase.Ban, Team.Blue, Team.Red),
            CreatePhaseData(GamePhase.Pick, Team.Blue, Team.Red, Team.Red, Team.Blue), 
            CreatePhaseData(GamePhase.Skill, Team.Blue, Team.Red, Team.Red, Team.Blue)
            );
        var sut = new AI_BattleSimulator(phaseManager, eventDispatcher);
        //var storage = new GameBanPickStorage();
        //var blue = new AI_BanPickAgent(Team.Blue, phaseManager, );
        //var red = new AI_BanPickAgent(Team.Red, phaseManager);

        // var result = sut.Run(blue, red);

        // Assert.AreEqual(Team.Blue, result.Winner);
    }
}
