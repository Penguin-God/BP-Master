using NUnit.Framework;
using System.Linq;
using static TestHelper;

public class StatPredictTeamTests
{
    [Test]
    public void 평균_스탯값으로_빈_슬롯_팀_예측_후_빌드()
    {
        StatTeamPredictor sut = new StatTeamPredictor(ChampionStatAverager(50, 100, 150), 5);

        SlotStorage<ChampionStatus> result = sut.BuildPerdictTeam(CreateOneSlotStatus(att: 1000, traitType: TraitType.Charge));

        Assert.AreEqual(1000, result.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(TraitType.Charge, result.GetSlot(BlueZeroSlot).TraitType);

        Assert.AreEqual(100, result.GetSlot(BlueOneSlot).Stat.Attack);
        Assert.AreEqual(TraitType.None, result.GetSlot(BlueOneSlot).TraitType);
        Assert.AreEqual(100, result.GetSlot(RedOneSlot).Stat.Attack);
        Assert.AreEqual(5, result.GetTeamCount(Team.Blue));
    }

    ChampionStatAverager ChampionStatAverager(params int[] stats) => new ChampionStatAverager(stats.Select(x => CreateStat(x, x)));
    [Test]
    public void 챔피언의_평균_스탯_반환()
    {
        var sut = ChampionStatAverager(0, 100);

        ChampionStatData result = sut.GetStatAverage();

        Assert.AreEqual(50, result.Attack);
        Assert.AreEqual(50, result.Defense);
    }
}
