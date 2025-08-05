using System.Collections;

public interface IBanPickAgent
{
    public IEnumerator WaitSelect();
    public ChampionSO SelectChampion();
}
