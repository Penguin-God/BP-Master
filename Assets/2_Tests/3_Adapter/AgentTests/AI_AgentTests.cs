using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static TestHelper;

public class AI_AgentTests
{
    // Fake
    class FirstSelect : IChampionSelector
    {
        public int Select(HashSet<int> ids) => ids.First();
    }

    AI_BanPickAgent CreateFirstSelectSut(Team team, BanPickStorage storage) => new AI_BanPickAgent(team, storage, new FirstSelect(), new FirstSelect());

    [Test]
    public void 규칙에_맞게_밴_선택_후_저장()
    {
        var storage = CreateStorage(3, 7, 9);
        var sut = CreateFirstSelectSut(Team.Blue, storage);
        int ban = 0;
        storage.OnBan += (team, id) => ban = id;

        sut.Ban(Team.Blue);

        Assert.AreEqual(3, ban);
        Assert.IsFalse(storage.SelectableIds.Contains(3));
    }

    [Test]
    public void 규칙에_맞게_픽_선택_후()
    {
        var storage = CreateStorage(2, 5, 8);
        var sut = CreateFirstSelectSut(Team.Red, storage);

        sut.Pick(Team.Red);

        Assert.AreEqual(2, storage.PickIds.GetSlot(RedZeroSlot));
    }

    [Test]
    public void 자기_팀_차례가_아니면_아무_일도_일어나지_않음()
    {
        var storage = CreateStorage(1, 4, 6);
        var sut = CreateFirstSelectSut(Team.Blue, storage);

        // 팀 불일치
        sut.Pick(Team.Red);

        CollectionAssert.IsEmpty(storage.PickIds.GetAll());
    }
}
