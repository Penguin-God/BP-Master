using NUnit.Framework;

public class PickHandlerTests
{
    [TestCase(Team.Blue)]
    [TestCase(Team.Red)]
    public void 픽하면_해당_팀에_챔피언이_슬롯에_추가되고_특성_및_숙련도_적용(Team team)
    {
        var champion = CreateChampion(traitType: TraitType.None);
        var champions = StubChampions.With(championId: 10, champion);
        var slots = SpySlots.Create();
        var traits = SpyTraits.Create();
        var masteries = StubMasteries.WithoutMastery();
        var masteryApplier = SpyMasteryApplier.Create();

        var sut = CreateSut(champions, slots, traits, masteries, masteryApplier);

        sut.OnPick(CreateSlot(team), id: 10);

        Assert.AreEqual(team, slots.AddedTeam);
        Assert.AreSame(champion, slots.AddedChampion);
    }
}
