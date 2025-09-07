using System.Collections.Generic;
using NUnit.Framework;

public class CalculateScoreTests
{
    [Test]
    public void 점수는_공방과_보너스의_합()
    {
        SortedDictionary<int, int> teamBonusData1 = new SortedDictionary<int, int>();
        teamBonusData1.Add(300, 50);

        SortedDictionary<int, int> teamBonusData2 = new SortedDictionary<int, int>();
        teamBonusData2.Add(20, 30);

        var champCal = new ChampionBonusCalculator(CreateBonusCalculator(100, 20), CreateBonusCalculator(100, 20));
        var teamCal = new TeamBonusCalculator(CreateBonusCalculator(300, 50), CreateBonusCalculator(300, 50), CreateBonusCalculator(20, 30));
        TeamScoreCalculator sut = new TeamScoreCalculator(champCal, teamCal);
        ChampionStatData[] team = new ChampionStatData[] { new(150, 150, 10), new(80, 200, 15) };

        int result = sut.CalculateScore(team);

        // 580 + 80 + 60
        Assert.AreEqual(720, result);
    }

    [Test]
    public void 팀_보너스는_스탯_구간_보너스_총합()
    {
        ChampionStatData[] champions = new ChampionStatData[] { new(150, 150, 10), new(200, 300, 15) };

        TeamBonusCalculator sut = new TeamBonusCalculator(
            CreateBonusCalculator(300, 50, 400, 80),
            CreateBonusCalculator(300, 50, 400, 80),
            CreateBonusCalculator(15, 50, 20, 80, 25, 100)
            );

        int result = sut.CalculateTeamBonus(champions);

        // 50 + 80 + 100
        Assert.AreEqual(230, result);
    }

    [Test]
    public void 챔프_공방_스탯_구간별_보너스()
    {
        var sut = new ChampionBonusCalculator(CreateBonusCalculator(300, 100, 400, 150), CreateBonusCalculator(300, 100, 400, 150));

        int result = sut.CalculateBonus(new ChampionStatData(300, 400, 0));

        Assert.AreEqual(250, result);
    }

    BonusCalculator CreateBonusCalculator(int section1 = 0, int bonus1 = 0, int section2 = 0, int bonus2 = 0, int section3 = 0, int bonus3 = 0)
    {
        SortedDictionary<int, int> bonusData = new SortedDictionary<int, int>();
        if(section1 > 0) bonusData.Add(section1, bonus1);
        if (section2 > 0) bonusData.Add(section2, bonus2);
        if (section3 > 0) bonusData.Add(section3, bonus3);
        return new BonusCalculator(bonusData);
    }

    [Test] 
    public void 보너스는_구간별로()
    {
        var sut = CreateBonusCalculator(300, 100, 400, 150);

        Assert.AreEqual(100, sut.CalculateBonus(300));
        Assert.AreEqual(150, sut.CalculateBonus(400));
    }
}
