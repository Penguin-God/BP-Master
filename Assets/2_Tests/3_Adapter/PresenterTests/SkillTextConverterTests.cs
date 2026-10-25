using NUnit.Framework;

public class SkillTextConverterTests
{
    [Test]
    public void 딕셔너리로_받은_템플릿을_실제_텍스트로_변환()
    {
        var keyRecord = new SkillConvertKey("Condition", "Rule", "{Stat}", "{Value}", "{Action}");
        SkillAmountData data = TestHelper.CreateSkillAmount(AmountType.Value, StatType.Attack, value: 100);
        var sut = new ActionTextConverter($"{keyRecord.Stat} {keyRecord.Value} {keyRecord.Action}", new SkillAmountTextBuilder(new AmountTextData("증가", "감소", "고정")), keyRecord);

        string result = sut.BuildText(SkillType.StatChanger, data);

        Assert.AreEqual("공격력 100 증가", result);
    }
}