
public class TargetCounter
{
    public readonly int TeamSize;
    public TargetCounter(int teamSize) => TeamSize = teamSize;

    public int CalculateTargetCount(TraitTargetRule rule)
    {
        if (rule.TargetRange == TargetRange.All)
            return rule.TargetSide == Side.All ? TeamSize * 2 : TeamSize; // side도 all이면 양쪽 다 타게팅 가능

        return rule.TargetRange switch
        {
            TargetRange.Single => 1,
            TargetRange.Double => 2,
            TargetRange.Triple => 3,
            _ => 0
        };
    }
}
