public record TraitConfig(int ChargeAttack, float GuardBonusRate, float AmpilyRate, float BreakRate);

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
