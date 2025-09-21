using NUnit.Framework;
using System;
using System.Collections.Generic;

public class MasteryPresentTests
{
    [Test]
    public void 숙련도_텍스트로_변환()
    {
        Dictionary<int, int> mastery = new Dictionary<int, int>();
        mastery.Add(1, 2);
        mastery.Add(2, 32);
        ChampionCatalog catalog = new ChampionCatalog(new Champion[] { TestHelper.CreateChamp(1, "닉스"), TestHelper.CreateChamp(2, "아르카나") });
        MasteryPersenter sut = new(catalog);
        
        string result = sut.Present(mastery);

        string expected =
            "닉스 : 2" +
            Environment.NewLine +
            "아르카나 : 32";

        Assert.AreEqual(expected, result);
    }
}
