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

    TraitUI_Data CreateData(SkillType traitType, int amount, SkillConditionData condition, TraitTargetRule rule) => new TraitUI_Data(traitType, amount, condition, rule);

    [Test]
    public void 특성_타입에_맞는_텍스트_생성()
    {
        var sut = new TraitTextBuilder();
        
        // 편의 함수
        string GetTraitText(SkillType traitType, int amount, TraitTargetRule rule) => sut.BuildTraitText(CreateData(traitType, amount, default, rule));

        Assert.AreEqual("선택한 적군 둘의 공격력 10 증가", GetTraitText(SkillType.AttackChanger, 10, OpponentDoubleRule));
        Assert.AreEqual("선택한 셋의 방어력 10 감소", GetTraitText(SkillType.DefenseChanger, -10, new TraitTargetRule(Side.All, TargetRange.Triple)));
        Assert.AreEqual("선택한 아군 둘의 속도 2 증가", GetTraitText(SkillType.SpeedChanger, 2, SelfDouble));
        Assert.AreEqual("양팀 전체 공격력 50 증가", GetTraitText(SkillType.AttackChanger, 50, AllRule));
        Assert.AreEqual("선택한 아군 셋의 방어력 100으로 고정", GetTraitText(SkillType.DefenseFixer, 100, SelfTriple));
        Assert.AreEqual("선택한 적군 하나의 스탯은 특성으로 인한 변화를 무시", GetTraitText(SkillType.TraitExcluder, 50, OpponentSingleRule));
    }

    [Test]
    public void 기준점_조건_텍스트_생성()
    {
        var sut = new TraitTextBuilder();

        // 편의 함수
        string GetTraitText(StatConditionType conditionType, int thershold) 
            => sut.BuildTraitText(CreateData(SkillType.DefenseChanger, -10, CreateThresholdCondition(conditionType, thershold), OpponentAllRule));

        Assert.AreEqual("방어력 100 이상인 적군 전체 방어력 10 감소", GetTraitText(StatConditionType.DefenseAtLeast, 100));
        Assert.AreEqual("방어력 10 이하인 적군 전체 방어력 10 감소", GetTraitText(StatConditionType.DefenseBelow, 10));
        Assert.AreEqual("공격력 100 이상인 적군 전체 방어력 10 감소", GetTraitText(StatConditionType.AttackAtLeast, 100));
        Assert.AreEqual("공격력 120 이하인 적군 전체 방어력 10 감소", GetTraitText(StatConditionType.AttackBelow, 120));
        Assert.AreEqual("속도 5 이상인 적군 전체 방어력 10 감소", GetTraitText(StatConditionType.SpeedAtLeast, 5));
        Assert.AreEqual("속도 3 이하인 적군 전체 방어력 10 감소", GetTraitText(StatConditionType.SpeedBelow, 3));
    }

    [Test]
    public void 비교_조건_텍스트_생성()
    {
        var sut = new TraitTextBuilder();

        // 편의 함수
        string GetTraitText(StatConditionType conditionType) => sut.BuildTraitText(CreateData(SkillType.DefenseChanger, -10, CreateCompareCondition(conditionType), SelfAllRule));

        Assert.AreEqual("방어력이 자신보다 높은 아군 전체 방어력 10 감소", GetTraitText(StatConditionType.DefenseAtLeast));
        Assert.AreEqual("방어력이 자신보다 낮은 아군 전체 방어력 10 감소", GetTraitText(StatConditionType.DefenseBelow));
        Assert.AreEqual("공격력이 자신보다 높은 아군 전체 방어력 10 감소", GetTraitText(StatConditionType.AttackAtLeast));
        Assert.AreEqual("공격력이 자신보다 낮은 아군 전체 방어력 10 감소", GetTraitText(StatConditionType.AttackBelow));
        Assert.AreEqual("속도가 자신보다 높은 아군 전체 방어력 10 감소", GetTraitText(StatConditionType.SpeedAtLeast));
        Assert.AreEqual("속도가 자신보다 낮은 아군 전체 방어력 10 감소", GetTraitText(StatConditionType.SpeedBelow));
    }

    [Test]
    public void 특성_컬랙션은_텍스트_합쳐서_반환() // 케이스 추가?
    {
        var sut = new TraitTextBuilder();
        var datas = new TraitUI_Data[]
        {
            CreateData(SkillType.AttackChanger, -10, default, OpponentAllRule),
            CreateData(SkillType.DefenseChanger, -10, default, OpponentAllRule),
        };

        string result = sut.BuildTraitText(datas);

        Assert.AreEqual("적군 전체 공격력 10 감소, 적군 전체 방어력 10 감소", result);
    }
}
