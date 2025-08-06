using System.Collections;

public interface ISelectWait
{
    public IEnumerator WaitSelect();
}

public interface ISelector
{
    public int SelectChampion();
}

public class SelectAgent
{
    public SelectAgent(ISelector selector, ISelectWait selectWait)
    {
        this.selector = selector;
        this.selectWait = selectWait;
    }

    ISelector selector;
    ISelectWait selectWait;

    public IEnumerator Co_SelectWait() => selectWait.WaitSelect();
    public int SelectChampion() => selector.SelectChampion();
}
