using NUnit.Framework;
using static TestHelper;

public class ChampionTextBuilderTests
{
    [Test]
    public void 챔피언_스탯정보가_텍스트로_생성()
    {
        var sut = new StatTextBuilder();

        StatViewModel result = sut.CreateStatViewModel(new ChampionStatData(10, 12, 6));

        Assert.AreEqual("공 10", result.Attack);
        Assert.AreEqual("방 12", result.Defense);
        Assert.AreEqual("속도 6", result.Speed);
    }

    TraitUI_Data CreateData(TraitType traitType, int amount, TraitConditionData condition, TraitTargetRule rule) => new TraitUI_Data(traitType, amount, condition, rule);

    [Test]
    public void 특성_타입에_맞는_텍스트_생성()
    {
        var sut = new TraitTextBuilder();
        
        // 편의 함수
        string GetTraitText(TraitType traitType, int amount, TraitTargetRule rule) => sut.BuildTraitText(CreateData(traitType, amount, default, rule));

        Assert.AreEqual("아군 전체 공격력 10 증가", GetTraitText(TraitType.AttackChanger, 10, SelfAllRule));
        Assert.AreEqual("적군 전체 방어력 10 감소", GetTraitText(TraitType.DefenseChanger, -10, OpponentAllRule));
        Assert.AreEqual("아군 단일 대상 속도 2 증가", GetTraitText(TraitType.SpeedChanger, 2, SelfSingleRule));
        Assert.AreEqual("양팀 전체 공격력 50 증가", GetTraitText(TraitType.AttackChanger, 50, AllRule));
        Assert.AreEqual("아군 전체 방어력 100으로 고정", GetTraitText(TraitType.DefenseFixer, 100, SelfAllRule));
        Assert.AreEqual("적군 단일 대상 특성 제외", GetTraitText(TraitType.TraitExcluder, 50, OpponentSingleRule));
    }

    [Test]
    public void 기준점_조건_텍스트_생성()
    {
        var sut = new TraitTextBuilder();

        // 편의 함수
        string GetTraitText(TraitConditionType conditionType, int thershold) 
            => sut.BuildTraitText(CreateData(TraitType.DefenseChanger, -10, CreateThresholdCondition(conditionType, thershold), OpponentAllRule));

        Assert.AreEqual("방어력이 100 이상인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.DefenseAtLeast, 100));
        Assert.AreEqual("방어력이 10 이하인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.DefenseBelow, 10));
        Assert.AreEqual("공격력이 100 이상인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.AttackAtLeast, 100));
        Assert.AreEqual("공격력이 120 이하인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.AttackBelow, 120));
        Assert.AreEqual("속도 5 이상인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.SpeedAtLeast, 5));
        Assert.AreEqual("속도 3 이하인 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.SpeedBelow, 3));
    }

    [Test]
    public void 비교_조건_텍스트_생성()
    {
        var sut = new TraitTextBuilder();

        // 편의 함수
        string GetTraitText(TraitConditionType conditionType) => sut.BuildTraitText(CreateData(TraitType.DefenseChanger, -10, CreateCompareCondition(conditionType), OpponentAllRule));

        Assert.AreEqual("방어력이 자신보다 높은 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.DefenseAtLeast));
        Assert.AreEqual("방어력이 자신보다 낮은 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.DefenseBelow));
        Assert.AreEqual("공격력이 자신보다 높은 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.AttackAtLeast));
        Assert.AreEqual("공격력이 자신보다 낮은 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.AttackBelow));
        Assert.AreEqual("속도가 자신보다 높은 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.SpeedAtLeast));
        Assert.AreEqual("속도가 자신보다 낮은 적군 전체 방어력 10 감소", GetTraitText(TraitConditionType.SpeedBelow));
    }

    [Test]
    public void 특성_컬랙션은_텍스트_합쳐서_반환() // 케이스 추가?
    {
        var sut = new TraitTextBuilder();
        var datas = new TraitUI_Data[]
        {
            CreateData(TraitType.AttackChanger, -10, default, OpponentAllRule),
            CreateData(TraitType.DefenseChanger, -10, default, OpponentAllRule),
        };

        string result = sut.BuildTraitText(datas);

        Assert.AreEqual("적군 전체 공격력 10 감소, 적군 전체 방어력 10 감소", result);
    }
}
