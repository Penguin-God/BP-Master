using UnityEngine;
using Match;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AIBattleSimulator", menuName = "AI/BattleSimulator")]
public class AIBattleSimulatorSO : ScriptableObject
{
    [SerializeField] AISelectorFactory aiFactory;
    [SerializeField] MatchCoreFactorySO matchCoreFactorySO;

    public int SimulateMatch(MatchData match, int winGoal = 2, Action<MatchResult> onSingleGameEnd = null, Action onMatchEnd = null)
    {
        MatchContext.MatchInit(match, winGoal, ChampionDataLoder.AllId);
        return RunBattle(match, onSingleGameEnd, onMatchEnd);
    }

    int RunBattle(MatchData match, Action<MatchResult> onSingleGameEnd, Action onMatchEnd)
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

        core.OnGameFinished += OnDone;
        core.PhaseManager.Start();

        return finalWinnerId;


        void OnDone(MatchResult result) // 조건에 따라 RunBattle을 호출하는 재귀용 함수
        {
            core.OnGameFinished -= OnDone;
            onSingleGameEnd?.Invoke(result);

            if (result.Winner == Team.All)
            {
                finalWinnerId = RunBattle(match, onSingleGameEnd, onMatchEnd);
                return;
            }

            int winnerId = result.Winner == Team.Blue ? match.Id1 : match.Id2;

            if (MatchContext.EndMatch(winnerId))
            {
                finalWinnerId = winnerId;
                onMatchEnd?.Invoke();
            }
            else finalWinnerId = RunBattle(match, onSingleGameEnd, onMatchEnd);
        }
    }

    AI_Entry CreateEntry(Team team, int id, MatchCore core, ChampionCatalog catalog, BanPickStorage storage) => new AI_Entry(team, id, aiFactory, storage, core.SkillController, catalog, core.MasteryRegistry, core.BanPickHandler, core.PhaseAdvancer);
}