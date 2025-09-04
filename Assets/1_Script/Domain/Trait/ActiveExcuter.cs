using System.Linq;

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
            case Side.Self: ApplyToSingleSide(trait, targets, Side.Self); break;
            case Side.Opponent: ApplyToSingleSide(trait, targets, Side.Opponent); break;
            case Side.All: ApplyToAll(trait, targets); break;
        }
    }

    void ApplyToSingleSide(Trait trait, int[] targets, Side targetSide)
    {
        foreach (int index in targets)
            statManager.ChangeSelectData(BanPickEnumCaster.GetTargetTeam(Team, targetSide), index, trait.TraitAction.Do);
    }

    void ApplyToAll(Trait trait, int[] targets)
    {
        foreach (int target in targets)
        {
            statManager.ChangeSelectData(Team.Blue, target, trait.TraitAction.Do);
            statManager.ChangeSelectData(Team.Red, target, trait.TraitAction.Do);
        }
    }
}
