using Match;
using System.Collections.Generic;
using UnityEngine;

public class MatchUI_Controller : MonoBehaviour
{
    [SerializeField] ChampionSelector_UI championSelector;
    [SerializeField] ChampionButtonView championDrawer;
    [SerializeField] SkillUseController_UI skillUseView;
    [SerializeField] SlotViewOrchestrator slotViews;
    [SerializeField] ScoreView scoreView;
    
    [SerializeField] GameFlowView gameFlowView;
    [SerializeField] BanView banView;
    
    [SerializeField] MasteryTooltipTrigger redMasteryTooltipTrigger;
    [SerializeField] MasteryTooltipTrigger blueMasteryTooltipTrigger;
    [SerializeField] ParticipantView participantView;

    Team team;
    public void Init(Team playerTeam, MatchCore matchCore, Dictionary<Team, int> playerIds)
    {
        team = playerTeam;
        var banPickHandler = matchCore.BanPickHandler;
        var pickSlotFacade = banPickHandler.PickSlotFacade;
        var eventDispatcher = banPickHandler.BanPickEventDispatcher;

        championDrawer.Init(new ChampionButtonStatePresenter(
            GetMasteryIds(playerTeam), GetMasteryIds(EnumCaster.GetOppoentTeam(playerTeam)), MatchContext.CreateFearlessStorage().SelectableIds, ChampionDataLoder.NameCatalog), 
            CardListLoder.LoadAllStageCardIds(new PlayerPrefsStageStorage().LoadUnlockedStage()));

        slotViews.InitSlotView(pickSlotFacade.StatusSlots);
        championSelector.Init(banPickHandler, matchCore.PhaseManager);

        participantView.ViewParticipant(playerIds);
        redMasteryTooltipTrigger.Inject(matchCore.MasteryRegistry);
        blueMasteryTooltipTrigger.Inject(matchCore.MasteryRegistry);

        eventDispatcher.OnTeamBan += banView.UpdateBanList;
        eventDispatcher.OnPick += slotViews.PickChampion;

        eventDispatcher.OnBan += championDrawer.InActiveButton;
        eventDispatcher.OnPick += (_, id) => championDrawer.InActiveButton(id);

        eventDispatcher.OnPick += OnPick;

        skillUseView.gameObject.SetActive(false);
        skillUseView.Init(pickSlotFacade.SkillSlots, matchCore.SkillController);

        gameFlowView.Init(pickSlotFacade.IdSlots);
        matchCore.SkillController.OnUseSkill += gameFlowView.UpdateUseSkill;
        scoreView.Init(pickSlotFacade.StatusSlots);

        matchCore.PhaseEventDispatcher.OnGameProgress += gameFlowView.ViewGameFlow;

        IEnumerable<int> GetMasteryIds(Team team) => matchCore.MasteryRegistry.GetTeamMasteryCollection(team).AllMasteryIds;
    }

    void OnPick(SlotData slotData, int id)
    {
        if (slotData.Team != team) return;
        skillUseView.UseSkill(slotData);
    }
}
