using System.Collections;

public interface ISelectWait
{
    public IEnumerator WaitSelect();
}

public interface ISelector
{
    public int SelectChampion();
}

public interface ISelectStorage
{
    public void Save(int id);
}
