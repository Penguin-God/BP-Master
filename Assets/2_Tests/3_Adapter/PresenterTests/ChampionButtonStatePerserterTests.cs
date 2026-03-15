using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ChampionButtonStatePresenterTests
{
    ChampionButtonStatePresenter CreatePresenter(int[] myMasteries, int[] oppMasteries, int[] selectable, Dictionary<int, string> catalog) => new ChampionButtonStatePresenter(myMasteries, oppMasteries, selectable, catalog);

    Dictionary<int, string> CreateNameCatalog() => new Dictionary<int, string> { { 1, "펭귄갓" } };

    [Test]
    public void 나만_숙련도를_달성하면_초록색_버튼을_반환한다()
    {
        var sut = CreatePresenter(new[] { 1 }, new int[0], new[] { 1 }, CreateNameCatalog());

        var result = sut.GetState(1);

        Assert.AreEqual(ChampionButtonPalette.MyMastery, result.ButtonColor);
    }

    [Test]
    public void 상대방만_숙련도를_달성하면_붉은색_버튼을_반환한다()
    {
        var sut = CreatePresenter(new int[0], new[] { 1 }, new[] { 1 }, CreateNameCatalog());

        var result = sut.GetState(1);

        Assert.AreEqual(ChampionButtonPalette.OpponentMastery, result.ButtonColor);
    }

    [Test]
    public void 양쪽_모두_숙련도를_달성하면_노란색_버튼을_반환한다()
    {
        var sut = CreatePresenter(new[] { 1 }, new[] { 1 }, new[] { 1 }, CreateNameCatalog());

        var result = sut.GetState(1);

        Assert.AreEqual(ChampionButtonPalette.BothMastered, result.ButtonColor);
    }

    [Test]
    public void 선택_불가능한_챔피언은_비활성화_상태와_텍스트_색상을_반환한다()
    {
        var sut = CreatePresenter(new int[0], new int[0], new int[0], CreateNameCatalog());

        var result = sut.GetState(1);

        Assert.IsFalse(result.IsEnabled);
        Assert.AreEqual(ChampionButtonPalette.InactiveText, result.TextColor);
    }

    [Test]
    public void 카탈로그에_있는_챔피언_이름을_정확히_매핑하여_반환한다()
    {
        var sut = CreatePresenter(new int[0], new int[0], new[] { 1 }, CreateNameCatalog());

        var result = sut.GetState(1);

        Assert.AreEqual("펭귄갓", result.Name);
    }
}