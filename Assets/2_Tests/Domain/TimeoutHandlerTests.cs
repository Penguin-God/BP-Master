using NUnit.Framework;
using static TestHelper;

public class TimeoutHandlerTests
{
    private const int TEST_CHAMP_ID = 99;

    [TestCase(GamePhase.Ban, SelectType.Ban)]
    [TestCase(GamePhase.Pick, SelectType.Pick)]
    public void 타임아웃_시_현재_페이즈에_맞는_타입으로_랜덤_선택이_저장된다(GamePhase currentPhase, SelectType expectedType)
    {
        var storage = CreateStorage(99);

        var advancer = new PhaseAdvancer(new PhaseData[] { CreatePhaseData(GamePhase.Ban, Team.Blue) });

        var handler = new TimeoutHandler(advancer, storage);

        handler.Execute();

        Assert.IsTrue(storage.BanStorage[Team.Blue].Contains(TEST_CHAMP_ID));
    }
}