using NUnit.Framework;

public class ChampionPresentTests
{
    [Test]
    public void 챔피언_스탯정보가_텍스트로_출력()
    {
        var sut = new ChampionPersenter();

        ChampionViewModel result = sut.CreateViewModel(new ChampionStatData(10, 12, 6), default);

        Assert.AreEqual("공격력 : 10", result.Attack);
        Assert.AreEqual("방어력 : 12", result.Defense);
        Assert.AreEqual("속도 : 6", result.Speed);
    }

    [Test]
    public void 특성_타입에_맞는_텍스트_출력()
    {
        var sut = new ChampionPersenter();
        
        // 편의 함수
        string GetTraitText(TraitType traitType, Side side, TargetRange range, int amount) => sut.CreateViewModel(default, new TraitUI_Data(traitType, side, range, amount, TraitConditionType.None, 0)).Trait;

        Assert.AreEqual("아군 전체 공격력 10 증가", GetTraitText(TraitType.AttackChanger, Side.Self, TargetRange.All, 10));
        Assert.AreEqual("적군 전체 방어력 10 감소", GetTraitText(TraitType.DefenseChanger, Side.Opponent, TargetRange.All, -10));
        Assert.AreEqual("아군 단일 대상 속도 2 증가", GetTraitText(TraitType.SpeedChanger, Side.Self, TargetRange.Single, 2));
    }
}
