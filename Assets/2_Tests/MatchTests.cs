using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MatchTests
{
    [Test]
    public void 매치는_자동_진행()
    {
        var storage = new GameBanPickStorage(new int[] { 0, 1, 2, 3 });
        DraftActionController draftController = new(storage);

        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
        };
        PhaseManager phaseManager = new(phase);

        PhaseActionDispatcher blue = new PhaseActionDispatcher(Team.Blue, new FakeDraftExecutor(storage));
        PhaseActionDispatcher red = new PhaseActionDispatcher(Team.Red, new FakeDraftExecutor(storage));

        MatchManager sut = new(phaseManager, draftController, blue, red);

        sut.GameStart();
        Assert.AreEqual(GamePhase.Done, sut.CurrentPhase);
    }
}

public class FakeDraftExecutor : IDraftActionHandler
{
    readonly GameBanPickStorage storage;

    public FakeDraftExecutor(GameBanPickStorage gameSelectStorage)
    {
        this.storage = gameSelectStorage;
    }

    public void OnRequestBan(Team team, DraftActionController draftAction) => draftAction.Ban(team, storage.SelectableIds[0]);
    public void OnRequestPick(Team team, DraftActionController draftAction) => draftAction.Pick(team, storage.SelectableIds[0]);
    public void OnRequestSwap(Team team, DraftActionController draftAction) => draftAction.SwapDone(team);
}