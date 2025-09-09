using System;

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

    public void ClickChampion(Team championTeam, int championIndex) // 나중에 결과 필요하면 Faild, Use, Select enum만들기
    {
        if (IsValidTarget(championTeam) == false) return;

        if (traitController.IsSelected)
            UseTrait(championIndex);
        else
            traitController.SelectTrait(currentTeam, championIndex);
    }

    bool IsValidTarget(Team buttonTeam) // 나중에는 타겟 범위까지 판단해야 되긴해
    {
        return (traitController.IsSelected == false && currentTeam == buttonTeam) || traitController.IsSelected;
    }

    void UseTrait(int targetIndex)
    {
        if (traitController.UseTrait(targetIndex))
            phaseManager.SubmitAction(currentTeam);
    }
}
