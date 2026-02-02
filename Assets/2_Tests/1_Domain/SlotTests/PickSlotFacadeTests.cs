using NUnit.Framework;
using static TestHelper;

public class PickSlotFacadeTests
{
    [TestCase(Team.Blue)]
    [TestCase(Team.Red)]
    public void 픽_데이터를_해당_팀_슬롯_스토리지에_추가한다(Team team)
    {
        var sut = new PickSlotFacade();

        var champion = new Champion(10, CreateValueSkill(StatType.Attack, 100), CreateStatus(att: 150));
        sut.Add(team, champion);

        Assert.AreEqual(10, sut.IdSlots.GetSlot(CreateSlot(team, 0)));
        Assert.AreEqual(champion, sut.ChampionSlots.GetSlot(CreateSlot(team, 0)));
        Assert.AreEqual(champion.Status, sut.StatusSlots.GetSlot(CreateSlot(team, 0)));
        Assert.AreEqual(champion.Skill, sut.SkillSlots.GetSlot(CreateSlot(team, 0)));
    }
}
