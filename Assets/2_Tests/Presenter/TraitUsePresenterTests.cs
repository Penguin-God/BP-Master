using NUnit.Framework;
using System.Collections.Generic;

public class TraitUsePresenterTests
{
    PhaseManager phaseManager;
    TraitController traitController;
    TraitUsePresenter presenter;

    [SetUp]
    public void SetUp()
    {
        var blueChamp = new Champion(0, "", default, new Trait(Side.Self, TargetRange.Single, new TestAttackChanger(0)));
        var redChamp = new Champion(0, "", default, new Trait(Side.Self, TargetRange.Single, new TestAttackChanger(0)));

        var teams = new Dictionary<Team, IReadOnlyList<Champion>>
        {
            { Team.Blue, new List<Champion>{ blueChamp } },
            { Team.Red, new List<Champion>{ redChamp } }
        };
        traitController = new TraitController(teams);

        phaseManager = new PhaseManager(new[] { new PhaseData(GamePhase.Trait, new Phase(new[] { Team.Blue, Team.Red })) });
        presenter = new TraitUsePresenter(traitController, phaseManager);

        phaseManager.Start();
    }

    [Test]
    public void 같은_팀만_선택_가능()
    {
        presenter.ClickChampion(Team.Red, 0);
        Assert.IsFalse(traitController.IsSelected);

        presenter.ClickChampion(Team.Blue, 0);
        Assert.IsTrue(traitController.IsSelected);
    }

    [Test]
    public void 선택_후_타겟_결정_시_특성_사용()
    {
        bool submitted = false;
        phaseManager.OnFlowChanged += _ => submitted = true;

        presenter.ClickChampion(Team.Blue, 0);
        presenter.ClickChampion(Team.Red, 0);

        Assert.IsTrue(submitted);
    }
}