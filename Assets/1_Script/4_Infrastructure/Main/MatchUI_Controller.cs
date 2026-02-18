using UnityEngine;

public class MatchUI_Controller : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfig;
    [SerializeField] ChampionSelector_UI championSelector;
    [SerializeField] ChampionButtonView championDrawer;
    [SerializeField] SkillUseController_UI skillUseView;
    [SerializeField] SlotViewOrchestrator slotViews;
    [SerializeField] ScoreView scoreView;
    
    [SerializeField] GameFlowView gameFlowView;
    [SerializeField] SkillButtonView skillButtonView;
    [SerializeField] BanView banView;
    
    [SerializeField] MasteryTooltipTrigger redMasteryTooltipTrigger;
    [SerializeField] MasteryTooltipTrigger blueMasteryTooltipTrigger;
    [SerializeField] ParticipantView participantView;

    MasteryButtonHighlighter masteryHighlighter;
    public void Awake()
    {
        masteryHighlighter = GetComponentInChildren<MasteryButtonHighlighter>(includeInactive: true);
    }

    Team team;
    public void Init(Team playerTeam, BanPickStorage storage, PhaseFlowOrchestrator phaseManager, PhaseEventDispatcher eventDispatcher, PickSlotFacade pickSlotFacade, SkillUsecase skillController, MasteryRegistry masteryRegistry, MatchRecord matchRecord)
    {
        team = playerTeam;
        slotViews.InitSlotView(pickSlotFacade.StatusSlots);
        championSelector.Init(storage, phaseManager);

        participantView.ViewParticipant(matchRecord, playerTeam);
        masteryHighlighter.Highlight(playerTeam, masteryRegistry); // championSelector 이후에 시작
        redMasteryTooltipTrigger.Inject(masteryRegistry);
        blueMasteryTooltipTrigger.Inject(masteryRegistry);
        championDrawer.InActiveButtons(storage.SelectableIds);

        storage.OnBan += banView.UpdateBanList;
        storage.OnPick += slotViews.PickChampion;

        storage.OnBan += (_, id) => championDrawer.InActiveButton(id);
        storage.OnPick += (_, id) => championDrawer.InActiveButton(id);

        storage.OnPick += OnPick;

        skillUseView.gameObject.SetActive(false);
        skillButtonView.Init(playerTeam);
        skillUseView.Init(pickSlotFacade.SkillSlots, skillController);

        gameFlowView.Init(pickSlotFacade.IdSlots);
        skillController.OnUseSkill += gameFlowView.UpdateUseSkill;
        scoreView.Init(pickSlotFacade.StatusSlots);

        eventDispatcher.OnGameProgress += gameFlowView.ViewGameFlow;
    }

    void OnPick(SlotData slotData, int id)
    {
        if (slotData.Team != team) return;
        skillUseView.UseSkill(slotData);
    }

    [SerializeField] GameObject scores;
    public void Done(MatchResult result, bool isGameEnd)
    {
        championDrawer.gameObject.SetActive(false);
        Instantiate(scores, transform).GetComponent<MatchResultView>().ShowResult(result, isGameEnd);
    }
}
