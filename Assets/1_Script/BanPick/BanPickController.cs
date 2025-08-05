using System;
using System.Collections;
using UnityEngine;

public class BanPickController : MonoBehaviour
{
    BanPickManager banPickManager;

    public event Action<SelectData> OnSelectedChampion = null;
    [SerializeField] DraftTurnSO banTurnSO;
    [SerializeField] DraftTurnSO pickTurnSO;
    void Awake()
    {
        banPickManager = new BanPickManager(banTurnSO, pickTurnSO);
        banPickAgent = FindAnyObjectByType<BanPickUI>(); // �������� DI ���ֱ⸦
        StartCoroutine(Co_BanPick());
    }

    void SelectChampion(ChampionSO champion)
    {
        // ���н� �ٽ� ����
        if (banPickManager.TrySelect(champion, out var data) == false)
        {
            StartCoroutine(Co_BanPick());
            return;
        }

        OnSelectedChampion?.Invoke(data);
        if (data.BanPcikPhase < BanPcikPhase.Swap)
            StartCoroutine(Co_BanPick());
    }

    IEnumerator Co_BanPick()
    {
        yield return banPickAgent.WaitSelect();
        SelectChampion(banPickAgent.SelectChampion());
    }

    IBanPickAgent banPickAgent;
}
