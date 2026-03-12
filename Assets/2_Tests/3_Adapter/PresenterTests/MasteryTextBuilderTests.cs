using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class MasteryTextBuilderTests
{
    [Test]
    public void 숙련도_스탯을_텍스트로_변환()
    {
        List<ChampionMastery> masteries = new List<ChampionMastery>()
        {
            CreateMasteryData(1, 2),
            CreateMasteryData(2, 32)
        };

        Dictionary<int, string> nameCatalog = new Dictionary<int, string>()
        {
            {1, "닉스" },
            {2, "아르카나" },
        };

        MasteryTextBuilder sut = new(nameCatalog);

        string result = sut.BuildMasteriesText(masteries);

        string expected =
            "닉스 : 공 2, 방 2, 속도 0" +
            "\n" +
            "아르카나 : 공 32, 방 32, 속도 0";

        Assert.AreEqual(expected, result);
    }
}
