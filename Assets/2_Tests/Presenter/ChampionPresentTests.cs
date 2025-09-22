using NUnit.Framework;

public class ChampionPresentTests
{
    [Test]
    public void 챔피언_스탯정보가_텍스트로_생성()
    {
        var sut = new ChampionPersenter();

        StatViewModel result = sut.CreateStatViewModel(new ChampionStatData(10, 12, 6));

        Assert.AreEqual("공격력 : 10", result.Attack);
        Assert.AreEqual("방어력 : 12", result.Defense);
        Assert.AreEqual("속도 : 6", result.Speed);
    }

    [Test]
    public void 특성_타입에_맞는_텍스트_생성()
    {
        var sut = new TraitPersenter();
        
        // 편의 함수
        string GetTraitText(TraitType traitType, Side side, TargetRange range, int amount) => sut.BuildTraitText(new TraitUI_Data(traitType, side, range, amount, TraitConditionType.None, 0));

        Assert.AreEqual("아군 전체 공격력 10 증가", GetTraitText(TraitType.AttackChanger, Side.Self, TargetRange.All, 10));
        Assert.AreEqual("적군 전체 방어력 10 감소", GetTraitText(TraitType.DefenseChanger, Side.Opponent, TargetRange.All, -10));
        Assert.AreEqual("아군 단일 대상 속도 2 증가", GetTraitText(TraitType.SpeedChanger, Side.Self, TargetRange.Single, 2));
    }

    [Test]
    public void 조건과_액션에_맞는_특성_텍스트_생성()
    {
        var sut = new TraitPersenter();

        // 편의 함수
        string GetTraitText(TraitConditionType conditionType, int thershold) => sut.BuildTraitText(new TraitUI_Data(TraitType.DefenseChanger, Side.Opponent, TargetRange.All, -10, conditionType, thershold));

        Assert.AreEqual("방어력이 100 이상인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.DefenseAtLeast, 100));
        Assert.AreEqual("방어력이 10 이하인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.DefenseBelow, 10));
        Assert.AreEqual("공격력이 100 이상인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.AttackAtLeast, 100));
        Assert.AreEqual("공격력이 120 이하인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.AttackBelow, 120));
        Assert.AreEqual("속도 5 이상인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.SpeedAtLeast, 5));
        Assert.AreEqual("속도 3 이하인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.SpeedBelow, 3));
    }
}
