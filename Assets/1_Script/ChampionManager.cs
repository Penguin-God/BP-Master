using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChampionManager : MonoBehaviour
{
    [SerializeField] ChampionSO[] allChampion;
    public IReadOnlyList<ChampionSO> AllChampion => allChampion;

    void Awake()
    {
        allChampion = LoadAllChampions();
    }

    // ��� è ������ �� ����ִµ� ��ã�°� ���� �ȵǴ� ��Ȳ�̶� First() ���
    public ChampionSO GetChampionData(int id) => allChampion.First(x =>  x.Id == id);

    public static ChampionSO[] LoadAllChampions()
    {
        return Resources.LoadAll<ChampionSO>("SO/Champions");
    }
}
