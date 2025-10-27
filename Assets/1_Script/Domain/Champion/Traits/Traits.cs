using System.Collections.Generic;
using System.Linq;

public enum TraitType
{
    None,
    Charge,
    Guard,
    Amplifier,
}

public interface ITrait
{
    public void Do();
}

public class NullTrait : ITrait
{
    public void Do() { }
}

public class Charge : ITrait
{
    readonly int AmountPerHolder;
    readonly IEnumerable<ChampionStatus> statuses;

    public Charge(int amountPerHolder, IEnumerable<ChampionStatus> statuses)
    {
        this.AmountPerHolder = amountPerHolder;
        this.statuses = statuses;
    }

    public void Do()
    {
        int totalIncrease = GetChargeCount() * AmountPerHolder;
        foreach (var status in statuses.Where(IsCharge))
        {
            var newStat = status.Stat.ChangeAttack(status.Stat.Attack + totalIncrease);
            status.ChangeStatWithRate(newStat);
        }
    }

    int GetChargeCount() => statuses.Count(IsCharge);

    bool IsCharge(ChampionStatus status) => status.TraitType == TraitType.Charge;
}

public class Guard : ITrait
{
    readonly float GuardBonusRate;
    readonly IEnumerable<ChampionStatus> statuses;

    public Guard(float guardBonusRate, IEnumerable<ChampionStatus> statuses)
    {
        GuardBonusRate = guardBonusRate;
        this.statuses = statuses;
    }

    public void Do()
    {
        foreach (var status in statuses) 
            status.AddDownRate(GuardBonusRate * -1);
    }
}

public class Amplifier : ITrait
{
    readonly float AmpilyRate;
    readonly IEnumerable<ChampionStatus> statuses;

    public Amplifier(float ampliyRate, IEnumerable<ChampionStatus> statuses)
    {
        AmpilyRate = ampliyRate;
        this.statuses = statuses;
    }

    public void Do()
    {
        foreach (var status in statuses)
            status.AddUpRate(AmpilyRate);
    }
}
