using System.Collections.Generic;
using System.Linq;

public enum TraitType
{
    None,
    Charge,
    Guard,
    Amplifier,
}

public class Charge
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
