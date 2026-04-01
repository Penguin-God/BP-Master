using UnityEngine;
using Match;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AIBattleSimulator", menuName = "SO/Match/AIBattleSimulator")]
public class AIBattleSimulatorSO : ScriptableObject
{
    [SerializeField] AIFactorySO aiFactory;
    [SerializeField] MatchCoreFactorySO matchCoreFactorySO;

    public int SimulateMatch(MatchData match, int winGoal = 2, Action<MatchResult> onSingleGameEnd = null)
    {
        MatchContext.MatchInit(match, winGoal, ChampionDataLoder.AllId);
        return RunBattle(match, onSingleGameEnd);
    }

    int RunBattle(MatchData match, Action<MatchResult> onSingleGameEnd)
    {
        var storage = MatchContext.Storage;
        var catalog = ChampionDataLoder.GetCatalog();

        var idByTeam = new Dictionary<Team, int>
        {
            { Team.Blue, match.Id1 },
            { Team.Red, match.Id2 }
        };

        var core = matchCoreFactorySO.CreateMatchCore(storage, catalog, idByTeam);

        var blueEntry = CreateEntry(Team.Blue, match.Id1, core, catalog, storage);
        var redEntry = CreateEntry(Team.Red, match.Id2, core, catalog, storage);

        core.SetupPhaseManager(blueEntry, redEntry);

        int finalWinnerId = -1;

        core.OnMatchFinished += OnDone;
        core.PhaseManager.Start();

        return finalWinnerId;


        void OnDone(MatchResult result) // 조건에 따라 RunBattle을 호출하는 재귀용 함수
        {
            core.OnMatchFinished -= OnDone;
            onSingleGameEnd?.Invoke(result);

            if (result.Winner == Team.All)
            {
                finalWinnerId = RunBattle(match, onSingleGameEnd);
                return;
            }

            int winnerId = result.Winner == Team.Blue ? match.Id1 : match.Id2;

            if (MatchContext.EndMatch(winnerId) == false) finalWinnerId = RunBattle(match, onSingleGameEnd);
            else finalWinnerId = winnerId;
        }
    }

    AI_Entry CreateEntry(Team team, int id, MatchCore core, ChampionCatalog catalog, BanPickStorage storage) => new AI_Entry(team, id, aiFactory, storage, core.SkillController, catalog, core.MasteryRegistry, core.BanPickHandler, core.PhaseAdvancer);
}