using System.Collections;
using UnityEngine;

public class AI_Scene : MonoBehaviour
{
    [SerializeField] int ai_id1;
    [SerializeField] int ai_id2;
    [SerializeField] int matchCount;
    [SerializeField] AIBattleSimulatorSO simulator;

    int blueWin;
    int redWin;
    int blueMatchWin;
    int redMatchWin;

    void Start() => StartCoroutine(Co_RunMatches());

    IEnumerator Co_RunMatches()
    {
        for (int i = 0; i < matchCount; i++)
        {
            var match = new MatchData(ai_id1, ai_id2);
            int finalWinnerId = simulator.SimulateMatch(match, winGoal: 2, onSingleGameEnd: PrintGame);

            // 매치(BO3) 최종 승자 기록
            if (finalWinnerId == ai_id1) blueMatchWin++;
            else redMatchWin++;

            yield return null; // 연산 터지는거 방지
        }

        print($"blue win (단일 라운드) : {blueWin}");
        print($"red win (단일 라운드) : {redWin}");

        print($"블루 매치 승 (최종 승리) : {blueMatchWin}");
        print($"레드 매치 승 (최종 승리) : {redMatchWin}");
    }

    void PrintGame(MatchResult result)
    {
        if (result.Winner == Team.Blue) blueWin++;
        else if (result.Winner == Team.Red) redWin++;
        else print("이게 무승부네 ㅋㅋ");
    }
}

public class AI_Entry : IPhaseEntry
{
    readonly Team Team;
    readonly AI_BanPickAgent banPickAgent;
    readonly AI_SkillExecutionUseCase skillUseCase;

    public void EnterBan() => banPickAgent.Ban(Team);
    public void EnterPick() => banPickAgent.Pick(Team);

    public AI_Entry(Team team, int ai_id, AIFactorySO aiFactory, BanPickStorage storage, SkillUsecase skillUseController, ChampionCatalog championCatalog, MasteryRegistry masteryRegistry, BanPickHandler banPickHandler, PhaseAdvancer phaseAdvancer)
    {
        Team = team;
        AI_SelectorFactory selectorFactory = aiFactory.CreateAI(ai_id, Team, storage, championCatalog, masteryRegistry, banPickHandler, phaseAdvancer);
        banPickAgent = new AI_BanPickAgent(Team, storage, selectorFactory.CreateBanSelector(), selectorFactory.CreatePickSelector(), banPickHandler);
        banPickHandler.BanPickEventDispatcher.OnPick += UseSkill;
        skillUseCase = new AI_SkillExecutionUseCase(banPickHandler.PickSlotFacade.SkillSlots, skillUseController, new SkillTargetService(new HighStatTargetSelector(banPickHandler.PickSlotFacade.StatusSlots)));
    }

    void UseSkill(SlotData slotData, int id)
    {
        if (slotData.Team != Team) return;
        skillUseCase.UseSkill(slotData);
    }
}