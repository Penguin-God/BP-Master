using NUnit.Framework;
using System.Linq;
using static TestHelper;

public class SkillTargetSelectorsTests
{
    [Test]
    public void 지정된_스탯_순으로_정렬하는_함수_반환()
    {
        var statusSlots = new SlotStorage<ChampionStatus>();
        statusSlots.AddSlots(Team.Blue, new ChampionStatus[] { CreateStatus(100), CreateStatus(200), CreateStatus(300) });
        var sut = TargetOrders.StatOrder(StatType.Attack);

        var result = statusSlots.GetTeam(Team.Blue).OrderByDescending(sut).ToArray();

        Assert.AreEqual(300, result[0].Stat.Attack);
        Assert.AreEqual(200, result[1].Stat.Attack);
        Assert.AreEqual(100, result[2].Stat.Attack);
    }
}
