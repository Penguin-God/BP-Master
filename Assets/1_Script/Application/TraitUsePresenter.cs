using System;
using System.Collections.Generic;
using System.Linq;

public enum TraitClickResult
{
    Faild,
    Select,
    Use
}

public class TraitUsePresenter // 타겟들 다 포함
{
    readonly TraitController traitController;
    Team currentTeam = Team.All;

    public event Action<Team> OnTraitUsed;
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

        if (traitController.UseTrait(selected.Value, targetSlot))
        {
            OnTraitUsed?.Invoke(currentTeam);
            selected = null;
            return TraitClickResult.Use;
        }
        else return TraitClickResult.Faild;
    }

    public IEnumerable<ChampionSlot> GetClickableSlots()
    {
        int size = traitController.GetTeamSize(currentTeam);
        // 선택 전: 현재 팀의 미사용 슬롯
        if (IsSelect == false)
        {
            return Enumerable.Range(0, size)
                             .Select(i => new ChampionSlot(currentTeam, i))
                             .Where(slot => traitController.IsTraitUsed(slot) == false);
        }

        // 선택 후: 시전자의 TraitSide에 따라 타겟 후보 생성
        var sel = selected.Value;
        var targetSide = traitController.GetTargetRule(currentTeam, sel.Index).TargetSide;
        return new TraitTargetSelector(size).GetTargetableSlot(currentTeam, targetSide);
    }
}
