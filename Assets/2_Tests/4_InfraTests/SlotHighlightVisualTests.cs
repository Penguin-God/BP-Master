using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class SlotHighlightVisualTests
{
    SlotView CreateSut()
    {
        var go = new GameObject("SlotViewTest", typeof(Image));
        return go.AddComponent<SlotView>();
    }

    [TestCase(true)]
    [TestCase(false)]
    public void 공격_시전자와_피격자_색상이_SlotHighlightPalette_정의와_일치한다(bool isCaster)
    {
        var sut = CreateSut();
        Color expected = isCaster ? SlotHighlightPalette.AttackCaster : SlotHighlightPalette.AttackTarget;

        sut.SetHighlight(expected);

        Assert.AreEqual(expected, sut.CurrentColor);

        Object.DestroyImmediate(sut.gameObject);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void 스킬_시전자와_대상_색상이_SlotHighlightPalette_정의와_일치한다(bool isCaster)
    {
        var sut = CreateSut();
        Color expected = isCaster ? SlotHighlightPalette.SkillCaster : SlotHighlightPalette.SkillTarget;

        sut.SetHighlight(expected);

        Assert.AreEqual(expected, sut.CurrentColor);

        Object.DestroyImmediate(sut.gameObject);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void ClearHighlight_호출_시_슬롯_색상이_기본_상태로_정확히_리셋된다(bool isAttackColor)
    {
        var sut = CreateSut();
        sut.SetHighlight(isAttackColor ? SlotHighlightPalette.AttackCaster : SlotHighlightPalette.SkillCaster);

        sut.ClearHighlight();

        Assert.AreEqual(SlotHighlightPalette.DefaultColor, sut.CurrentColor);

        Object.DestroyImmediate(sut.gameObject);
    }
}
