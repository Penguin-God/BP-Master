using NUnit.Framework;
using System.Collections.Generic;

public class SkillTextConverterTests
{
    [Test]
    public void 특정_기호를_값으로_반환()
    {
        Dictionary<SkillType, string> textBySkill = new Dictionary<SkillType, string>();
        textBySkill.Add(SkillType.AttackChanger, "공격력 {Value} {Change}");
        SkillAmountData data = new SkillAmountData(AmountType.Value, value: 100, 0, 0);
        var sut = new SkillTextConverter(textBySkill);

        string result = sut.BuildActionText(SkillType.AttackChanger, data);

        Assert.AreEqual("공격력 100 증가", result);
    }
}