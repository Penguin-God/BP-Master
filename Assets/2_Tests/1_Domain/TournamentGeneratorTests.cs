using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

public class TournamentGeneratorTests
{
    [Test]
    public void 순위표를_기반으로_1위와_4위_2위와_3위의_4강_대진을_반환한다()
    {
        var records = new List<LeagueRecord>
        { // 순서대로 1~5등
            new LeagueRecord(id: 1, win: 5),
            new LeagueRecord(id: 2, win: 4),
            new LeagueRecord(id: 3, win: 3),
            new LeagueRecord(id: 4, win: 2),
            new LeagueRecord(id: 4, win: 0)
        };
        var generator = new TournamentGenerator();

        var matches = generator.GenerateSemiFinals(records).ToArray();

        Assert.AreEqual(2, matches.Length);

        Assert.AreEqual(1, matches[0].Id1);
        Assert.AreEqual(4, matches[0].Id2);

        Assert.AreEqual(2, matches[1].Id1);
        Assert.AreEqual(3, matches[1].Id2);
    }

    [Test]
    public void 팀이_4개_미만일_경우_예외를_발생시킨다()
    {
        var records = new[] { new LeagueRecord(id: 1, win: 5) };
        var generator = new TournamentGenerator();

        // IEnumerable 지연 평가 때문에 ToArray() 호출해야 예외가 터짐
        Assert.Throws<ArgumentException>(() => generator.GenerateSemiFinals(records).ToArray());
    }
}