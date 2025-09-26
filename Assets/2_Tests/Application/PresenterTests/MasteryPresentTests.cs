using NUnit.Framework;
using System;
using System.Collections.Generic;

public class MasteryPresentTests
{
    [Test]
    public void 숙련도_텍스트로_변환()
    {
        List<ChampionMastery> mastery = new List<ChampionMastery>();
        mastery.Add(new ChampionMastery(1, 2));
        mastery.Add(new ChampionMastery(2, 32));
        ChampionCatalog catalog = new ChampionCatalog(new Champion[] { TestHelper.CreateChamp(1, "닉스"), TestHelper.CreateChamp(2, "아르카나") });
        MasteryPersenter sut = new(catalog);
        
        string result = sut.BuildMasteriesText(mastery);

        string expected =
            "닉스 : 2" +
            Environment.NewLine +
            "아르카나 : 32";

        Assert.AreEqual(expected, result);
    }
}
