using System;

public enum TraitClickResult
{
    Faild,
    Select,
    Use
}

public struct ChampionSlot
{
    public readonly Team Team;
    public readonly int Index;

    public ChampionSlot(Team team, int index)
    {
        Team = team;
        Index = index;
    }
}

public class TraitUsePresenter // 타겟들 다 포함
{
    readonly TraitController traitController;
    // readonly PhaseManager phaseManager;
    Team currentTeam = Team.All;

    public event Action<Team> OnTraitUsed;

    //public TraitUsePresenter(TraitController traitController, PhaseManager phaseManager)
    //{
    //    this.traitController = traitController;
    //    this.phaseManager = phaseManager;
    //    phaseManager.OnPhaseTrait += UpdateTeam;
    //}

    public TraitUsePresenter(TraitController traitController) => this.traitController = traitController;

    public void ChangeTeam(Team team) => currentTeam = team;

    ChampionSlot? selected; // 선택된 시전자
    bool IsSelect => selected.HasValue;

    public TraitClickResult ClickChampion(ChampionSlot slot)
    {
        if (IsValidTarget(slot.Team) == false) return TraitClickResult.Faild;

        if (IsSelect) return UseTrait(slot);
        else
        {
            selected = slot;
            return TraitClickResult.Select;
        }
    }

    bool IsValidTarget(Team buttonTeam) // 나중에는 타겟 범위까지 판단해야 됨
    {
        return (IsSelect == false && currentTeam == buttonTeam) || IsSelect;
    }

    TraitClickResult UseTrait(ChampionSlot targetSlot)
    {
        var sel = selected.Value;

        if (traitController.UseTrait(currentTeam, sel.Index, targetSlot.Index))
        {
            OnTraitUsed?.Invoke(currentTeam);
            selected = null;
            return TraitClickResult.Use;
        }
        else return TraitClickResult.Faild;
    }
}
