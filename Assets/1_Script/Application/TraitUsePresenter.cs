using System;

public enum TraitClickResult
{
    Faild,
    Select,
    Use
}

public class TraitUsePresenter
{
    readonly TraitController traitController;
    readonly PhaseManager phaseManager;
    Team currentTeam;

    public event Action<Team> OnTraitUsed;

    public TraitUsePresenter(TraitController traitController, PhaseManager phaseManager)
    {
        this.traitController = traitController;
        this.phaseManager = phaseManager;
        phaseManager.OnPhaseTrait += UpdateTeam;
    }

    void UpdateTeam(Team team) => currentTeam = team;
    int selectIndex = -1;
    bool IsSelect => selectIndex > -1;
    public TraitClickResult ClickChampion(Team championTeam, int championIndex)
    {
        if (IsValidTarget(championTeam) == false) return TraitClickResult.Faild;

        if (IsSelect) return UseTrait(championIndex);
        else
        {
            selectIndex = championIndex;
            return TraitClickResult.Select;
        }
    }
    
    bool IsValidTarget(Team buttonTeam) // 나중에는 타겟 범위까지 판단해야 되긴해
    {
        return (IsSelect == false && currentTeam == buttonTeam) || IsSelect;
    }

    TraitClickResult UseTrait(int targetIndex)
    {
        if (traitController.UseTrait(currentTeam, selectIndex, targetIndex))
        {
            OnTraitUsed?.Invoke(currentTeam); // 시간 커플링
            phaseManager.SubmitAction(currentTeam);
            selectIndex = -1;
            return TraitClickResult.Use;
        }
        else return TraitClickResult.Faild;
    }
}
