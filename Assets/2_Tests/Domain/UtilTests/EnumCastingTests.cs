using NUnit.Framework;
using System;
using System.Linq;

public class EnumCastingTests
{
    [Test]
    public void 타겟_팀_반환()
    {
        Assert.AreEqual(Team.Blue, EnumCaster.GetTargetTeam(Team.Blue, Side.Self));
        Assert.AreEqual(Team.Red, EnumCaster.GetTargetTeam(Team.Blue, Side.Opponent));
    }


    public void 페이즈를_선택으로_반환()
    {
        Assert.AreEqual(SelectType.Ban, EnumCaster.PhaseToSelect(GamePhase.Ban));
        Assert.AreEqual(SelectType.Pick, EnumCaster.PhaseToSelect(GamePhase.Pick));
    }

    [Test]
    public void Side_컬렉션_합치기()
    {
        Assert.AreEqual(Side.Self, MergeSide(Side.Self, Side.Self));
        Assert.AreEqual(Side.Opponent, MergeSide(Side.Opponent, Side.Opponent));
        Assert.AreEqual(Side.All, MergeSide(Side.Self, Side.All));
        Assert.AreEqual(Side.All, MergeSide(Side.Self, Side.Opponent));

        Side MergeSide(params Side[] sides) => EnumCaster.MergeSide(sides);
    }

    [Test]
    public void 동일한_Range면_정상적으로_병합()
    {
        var rules = new[]
        {
            new SkillTargetRule(Side.Self, TargetRange.All),
            new SkillTargetRule(Side.Opponent, TargetRange.All)
        };

        var result = EnumCaster.MergeRule(rules);

        Assert.AreEqual(TargetRange.All, result.TargetRange);
    }

    [Test]
    public void Range가_다르면_예외_발생()
    {
        var rules = new[]
        {
            new SkillTargetRule(Side.Self, TargetRange.Single),
            new SkillTargetRule(Side.Opponent, TargetRange.All)
        };

        var ex = Assert.Throws<Exception>(() => EnumCaster.MergeRule(rules));

        Assert.AreEqual("range가 통일되지 않음 : Single, All", ex.Message);
    }
}
