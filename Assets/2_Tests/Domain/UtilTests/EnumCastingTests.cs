using NUnit.Framework;

public class EnumCastingTests
{
    [Test]
    public void 타겟_팀_반환()
    {
        Assert.AreEqual(Team.Blue, BanPickEnumCaster.GetTargetTeam(Team.Blue, Side.Self));
        Assert.AreEqual(Team.Red, BanPickEnumCaster.GetTargetTeam(Team.Blue, Side.Opponent));
    }


    public void 페이즈를_선택으로_반환()
    {
        Assert.AreEqual(SelectType.Ban, BanPickEnumCaster.PhaseToSelect(GamePhase.Ban));
        Assert.AreEqual(SelectType.Pick, BanPickEnumCaster.PhaseToSelect(GamePhase.Pick));
    }

    [Test]
    public void PhaseToSelect_Swap_예외()
    {
        Assert.Throws<System.Exception>(() => BanPickEnumCaster.PhaseToSelect(GamePhase.Swap));
    }

    [Test]
    public void Side_컬렉션_합치기()
    {
        Assert.AreEqual(Side.Self, BanPickEnumCaster.MergeSide(new Side[] { Side.Self, Side.Self }));
        Assert.AreEqual(Side.Opponent, BanPickEnumCaster.MergeSide(new Side[] { Side.Opponent, Side.Opponent }));
        Assert.AreEqual(Side.All, BanPickEnumCaster.MergeSide(new Side[] { Side.Self, Side.All }));
        Assert.AreEqual(Side.All, BanPickEnumCaster.MergeSide(new Side[] { Side.Self, Side.Opponent }));
    }
}
