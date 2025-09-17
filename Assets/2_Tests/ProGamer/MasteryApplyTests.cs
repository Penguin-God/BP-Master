using NUnit.Framework;

public class MasteryApplyTests
{
    [Test]
    public void 챔프_숙련도_적용()
    {
        var gamer = CreateGamer(1, 10);
        var champ1 = new Champion(1, "", new ChampionStatData(10, 5, 0), default, null);
        var champ2 = new Champion(2, "", new ChampionStatData(10, 5, 0), default, null);
        MasteryApplier sut = new();

        // action
        sut.ApplyMastery(gamer, champ1);
        sut.ApplyMastery(gamer, champ2);

        Assert.AreEqual(20, champ1.StatData.Attack);
        Assert.AreEqual(15, champ1.StatData.Defense);

        Assert.AreEqual(10, champ2.StatData.Attack);
        Assert.AreEqual(5, champ2.StatData.Defense);
    }

    //[Test]
    //public void 슬롯에_있는_챔프_숙련도_적용()
    //{
    //    Dictionary<Team, IReadOnlyList<ProGamer>> gamerMap = new();
    //    gamerMap.Add(Team.Blue, new ProGamer[] { CreateGamer(1, 10), CreateGamer(3, 33) });
    //    gamerMap.Add(Team.Red, new ProGamer[] { CreateGamer(1, 10), CreateGamer(11, 33) });
    //    임시 sut = new(gamerMap);

    //    Dictionary<Team, IReadOnlyList<Champion>> pickChampions = new();
    //    var champ1 = CreateChamp(1);
    //    var champ2 = CreateChamp(2);
    //    var champ3 = CreateChamp(11);
    //    var champ4 = CreateChamp(12);

    //    pickChampions.Add(Team.Blue, new List<Champion>() { champ1, champ2 });
    //    pickChampions.Add(Team.Red, new List<Champion>() { champ3, champ4 });

    //    // action
    //    sut.ApplyMastery(pickChampions);

    //    Assert.AreEqual(10, champ1.StatData.Attack);
    //    Assert.AreEqual(10, champ1.StatData.Defense);

        
    //}

    ProGamer CreateGamer(int id, int level) => new ProGamer(new ChampionMastery[] { new ChampionMastery(id, level) });
}
