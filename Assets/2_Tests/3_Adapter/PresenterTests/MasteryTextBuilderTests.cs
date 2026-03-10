using NUnit.Framework;
using System.Collections.Generic;

public class MasteryTextBuilderTests
{
    [Test]
    public void 숙련도_텍스트로_변환()
    {
        List<ChampionMastery> mastery = new List<ChampionMastery>()
        {
            new ChampionMastery(1, 2),
            new ChampionMastery(2, 32)
        };

        Dictionary<int, string> nameCatalog = new Dictionary<int, string>()
        {
            {1, "닉스" },
            {2, "아르카나" },
        };
        MasteryTextBuilder sut = new(nameCatalog);


        string result = sut.BuildMasteriesText(mastery);

        string expected =
            "닉스 : 2" +
            "\n" +
            "아르카나 : 32";

        Assert.AreEqual(expected, result);
    }
}
