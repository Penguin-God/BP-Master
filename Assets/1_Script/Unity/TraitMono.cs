using System.Collections;
using System.Collections.Generic;

public interface ITriatAgent
{
    public IEnumerator Delay();
    public SlotData Select();
}

public class TraitMono
{
    ITriatAgent triatAgent;
    IEnumerable<TraitData> trait;
    TraitTargetSelector traitTargetSelector;
    IEnumerator Co_UseTrait()
    {
        yield return triatAgent.Delay();
        triatAgent.Select();
    }
}
