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
    [SerializeField] SkillButtonView skillButtonView;
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

        championDrawer.Init(new ChampionButtonStatePresenter(GetMasteryIds(playerTeam), GetMasteryIds(EnumCaster.GetOppoentTeam(playerTeam)), MatchContext.Storage.SelectableIds, ChampionDataLoder.NameCatalog));
        championDrawer.CreateButtons();

        slotViews.InitSlotView(pickSlotFacade.StatusSlots);
        championSelector.Init(banPickHandler, matchCore.PhaseManager);

        participantView.ViewParticipant(playerIds);
        redMasteryTooltipTrigger.Inject(matchCore.MasteryRegistry);
        blueMasteryTooltipTrigger.Inject(matchCore.MasteryRegistry);

        banPickHandler.BanPickEventDispatcher.OnTeamBan += banView.UpdateBanList;
        banPickHandler.BanPickEventDispatcher.OnPick += slotViews.PickChampion;

        banPickHandler.BanPickEventDispatcher.OnBan += championDrawer.InActiveButton;
        banPickHandler.BanPickEventDispatcher.OnPick += (_, id) => championDrawer.InActiveButton(id);

        banPickHandler.BanPickEventDispatcher.OnPick += OnPick;

        skillUseView.gameObject.SetActive(false);
        skillButtonView.Init(playerTeam);
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
