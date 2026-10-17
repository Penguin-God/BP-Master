using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageRewardDisplayer : MonoBehaviour
{
    [SerializeField] Transform cardContent;
    [SerializeField] Button closeButton;
    [SerializeField] UI_CardItem cardPrefab;

    void Awake()
    {
        if (closeButton != null) closeButton.onClick.AddListener(Close);
    }

    public void Draw(IEnumerable<ChampionSO> cards)
    {
        if (cards == null) return;
        ChampionButtonCreator.CreateCardItems(cardContent, cards, cardPrefab);
    }

    void Close() => Destroy(gameObject);
}