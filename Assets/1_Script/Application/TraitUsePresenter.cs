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

    public event Action<Team, int> OnTraitSelected;
    public event Action<Team> OnTraitUsed;

    public TraitUsePresenter(TraitController traitController, PhaseManager phaseManager)
    {
        this.traitController = traitController;
        this.phaseManager = phaseManager;
        phaseManager.OnPhaseTrait += UpdateTeam;
    }

    void UpdateTeam(Team team) => currentTeam = team;

    public TraitClickResult ClickChampion(Team championTeam, int championIndex) // 나중에 결과 필요하면 Faild, Use, Select enum만들기
    {
        if (IsValidTarget(championTeam) == false) return TraitClickResult.Faild;

        if (traitController.IsSelected) return UseTrait(championIndex);
        else
        {
            UnityEngine.Debug.Log(traitController.IsSelected);
            traitController.SelectTrait(currentTeam, championIndex);
            return TraitClickResult.Select;
        }
    }

    bool IsValidTarget(Team buttonTeam) // 나중에는 타겟 범위까지 판단해야 되긴해
    {
        return (traitController.IsSelected == false && currentTeam == buttonTeam) || traitController.IsSelected;
    }

    TraitClickResult UseTrait(int targetIndex)
    {
        if (traitController.UseTrait(targetIndex))
        {
            phaseManager.SubmitAction(currentTeam);
            return TraitClickResult.Use;
        }
        else return TraitClickResult.Faild;
    }
}
