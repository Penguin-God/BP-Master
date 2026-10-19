using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CardListSO", menuName = "Data/CardList")]
public class CardListSO : ScriptableObject
{
    [OnValueChanged(nameof(RemoveDuplicates), includeChildren: true)]
    [SerializeField] [Header("중복 허용 안하는 리스트")] ChampionSO[] _cardList;
    public IEnumerable<ChampionSO> CardList => _cardList;

    void RemoveDuplicates() => _cardList = _cardList.Distinct().ToArray();
}
