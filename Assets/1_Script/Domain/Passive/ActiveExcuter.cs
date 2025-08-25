using System.Collections.Generic;

public class ActiveExcuter
{
    Queue<IActivePassive> passives;

    public ActiveExcuter(IEnumerable<IActivePassive> passives)
    {
        this.passives = new Queue<IActivePassive>(passives);
    }

    public bool IsDone => passives.Count == 0;

    public void Do(int target)
    {
        var passive = passives.Dequeue();
        passive.Do(target);
    }
}
