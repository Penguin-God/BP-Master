using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardListSO", menuName = "Data/CardList")]
public class CardListSO : ScriptableObject
{
    [SerializeField] ChampionSO[] _cardList;
    public IEnumerable<ChampionSO> CardList => _cardList;
    public HashSet<int> CardIdSet => new(CardList.Select(x => x.Id));
}
