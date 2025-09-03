using System.Linq;

public enum Side { Self, Opponent, All }
public class ActiveExcuter
{
    readonly StatManager statManager;
    readonly Team Team;
    readonly Trait[] traits;
    readonly bool[] usedChampionFlags;
    
    public ActiveExcuter(StatManager statManager, Team team, Trait[] traits)
    {
        this.traits = traits;
        Team = team;
        this.statManager = statManager;
        this.usedChampionFlags = new bool[traits.Length];
    }

    public bool IsDone => IsTeamDone();

    public void DoActive(int championIndex, int[] targets = null)
    {
        if (VaildIndex(championIndex) == false || IsChampionUsed(championIndex)) return;

        usedChampionFlags[championIndex] = true;
        Trait trait = traits[championIndex];
        
        ApplyTraitEffect(trait, targets);
    }

    bool VaildIndex(int index) => index >= 0 && index < usedChampionFlags.Length;

    public bool IsChampionUsed(int championIndex) => VaildIndex(championIndex) && usedChampionFlags[championIndex];

    public bool IsTeamDone() => usedChampionFlags.All(x => x);


    void ApplyTraitEffect(Trait trait, int[] targets)
    {
        switch (trait.TargetSide)
        {
            case Side.Self: ApplyToAllies(trait, targets); break;
            case Side.Opponent: ApplyToOpponents(trait, targets); break;
            case Side.All: ApplyToAll(trait, targets); break;
        }
    }

    void ApplyToAllies(Trait trait, int[] targets)
    {
        if (targets == null || targets.Length == 0)
            statManager.ChangeAll(Team, trait.TraitAction.Do);
        else
        {
            // 지정된 아군들에게만 적용
            foreach (int target in targets)
                statManager.ChangeSelectData(Team, target, trait.TraitAction.Do);
        }
    }

    void ApplyToOpponents(Trait trait, int[] targets)
    {
        Team opponentTeam = Team == Team.Blue ? Team.Red : Team.Blue;
        
        if (targets == null || targets.Length == 0)
            statManager.ChangeAll(opponentTeam, trait.TraitAction.Do);
        else
        {
            // 지정된 상대들에게만 적용
            foreach (int target in targets)
                statManager.ChangeSelectData(opponentTeam, target, trait.TraitAction.Do);
        }
    }

    void ApplyToAll(Trait trait, int[] targets)
    {
        // Side.All은 모든 팀에게 적용 (현재는 사용되지 않지만 확장성을 위해)
        if (targets == null || targets.Length == 0)
        {
            statManager.ChangeAll(Team.Blue, trait.TraitAction.Do);
            statManager.ChangeAll(Team.Red, trait.TraitAction.Do);
        }
        else
        {
            foreach (int target in targets)
            {
                statManager.ChangeSelectData(Team.Blue, target, trait.TraitAction.Do);
                statManager.ChangeSelectData(Team.Red, target, trait.TraitAction.Do);
            }
        }
    }
}
