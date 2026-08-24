using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardListSO", menuName = "Data/CardList")]
public class CardListSO : ScriptableObject
{
    [SerializeField] ChampionSO[] _cardList;
    public IEnumerable<ChampionSO> CardList => _cardList;
}
