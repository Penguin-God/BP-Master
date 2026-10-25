using NUnit.Framework;

public class SkillTemplateHelperTests
{
    readonly SkillConvertKey keys = new("{Condition}", "{Rule}", "{Stat}", "{Value}", "{Action}");

    [Test]
    public void 키가_따라서_템플릿이_치환된다()
    {
        var values = new[] { "공격력 120 이상인", "아군 전체", "방어력", "10", "증가" };
        var template = "{Condition} {Rule} {Stat} {Value} {Action}";

        var result = SkillTemplateHelper.ReplaceTemplate(template, keys.Keys, values);

        Assert.AreEqual("공격력 120 이상인 아군 전체 방어력 10 증가", result);
    }

    [Test]
    public void 연속_공백과_앞뒤_공백은_생략()
    {
        var values = new[] { "", "적군 전체", "", "", "" };
        var template = "{Condition} {Rule} {Stat} {Value} {Action}";

        var result = SkillTemplateHelper.ReplaceTemplate(template, keys.Keys, values);

        Assert.AreEqual("적군 전체", result);
    }
}