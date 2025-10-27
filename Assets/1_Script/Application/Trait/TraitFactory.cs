public readonly struct TraitConfig
{
    public readonly int ChargeAttack;
    public readonly float GuardBonusRate;
    public readonly float AmpilyRate;
    public readonly float BreakRate;

    public TraitConfig(int chargeAttack, float guardBonusRate, float ampliRate, float breakRate)
    {
        this.ChargeAttack = chargeAttack;
        this.GuardBonusRate = guardBonusRate;
        AmpilyRate = ampliRate;
        BreakRate = breakRate;
    }
}

public class TraitFactory
{
    readonly TraitConfig config;
    readonly SlotStorage<ChampionStatus> statusSlots;

    public TraitFactory(TraitConfig config, SlotStorage<ChampionStatus> statusSlots)
    {
        this.config = config;
        this.statusSlots = statusSlots;
    }

    public ITrait Create(Team team, TraitType traitType)
    {
        return traitType switch
        {
            TraitType.None => new NullTrait(),
            TraitType.Charge => new Charge(config.ChargeAttack, statusSlots.GetTeam(team)), // 우리팀
            TraitType.Guard => new Guard(config.GuardBonusRate, statusSlots.GetTeam(team)), // 우리팀
            TraitType.Amplifier => new Amplifier(config.AmpilyRate, statusSlots.GetTeam(team)), // 우리팀
            TraitType.Break => new Break(config.BreakRate, statusSlots.GetTeam(EnumCaster.GetOppoentTeam(team))), // 적팀
        };
    }
}
